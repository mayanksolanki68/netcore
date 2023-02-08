using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmpManagement.Models;
using EmpManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmpManagement.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        private IHostingEnvironment hostingEnvironment;
        private ILogger _Logger;

        public HomeController(IEmployeeRepository employeeRepository, 
            IHostingEnvironment hostingEnvironment,
            ILogger<HomeController> logger)
        {
            
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            _Logger = logger;
        }
        [AllowAnonymous]
        public ViewResult Index()
        {
            try
            {
                var data = _employeeRepository.GetAllEmployees();
                return View(data);
            }
            catch (Exception ex)
            {
                return View();
            }

            //return this.Json(new { id=1,Name="Mayank"});
        }
        public ViewResult Details(int id)
        {
            //throw new Exception("Error in Details View");
            _Logger.LogTrace("Trace Log");
            _Logger.LogDebug("Debug Log");
            _Logger.LogInformation("Information Log");
            _Logger.LogWarning("Warning Log");
            _Logger.LogCritical("Critical Log");

            var employeeDetail = _employeeRepository.GetEmployee(id);
            if (employeeDetail == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound",id);
            }
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(id),
                PageTitle = "Employee Details"
            };

            // Pass the ViewModel object to the View() helper method
            return View(homeDetailsViewModel);
        }

        
        public ViewResult Edit(int id)
        {
            Employee employee = new Employee();
            employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEdit = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEdit);
        }
        
        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel employee)
        {
            if (ModelState.IsValid)
            {
                string filename = string.Empty;
                if (employee.Photo != null)
                {
                    if (employee.ExistingPhotoPath != null)
                    {
                        string filepath = Path.Combine(hostingEnvironment.WebRootPath, "images", employee.ExistingPhotoPath);
                        System.IO.File.Delete(filepath);
                    }
                    filename = EmployeeFileUploadProcess(employee);
                }

                Employee newemployee = new Employee
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Department = employee.Department,
                    PhotoPath = filename
                };


                _employeeRepository.Update(newemployee);
                return RedirectToAction("index");
            }
            return View();
        }
        
        public ViewResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                string filename = EmployeeFileUploadProcess(employee);
                Employee newemployee = new Employee
                {
                    Name = employee.Name,
                    Email = employee.Email,
                    Department = employee.Department,
                    PhotoPath = filename
                };
                _employeeRepository.Add(newemployee);
                return RedirectToAction("details", new { id = newemployee.Id });
            }
            return View();
        }

        private string EmployeeFileUploadProcess(EmployeeViewModel employee)
        {
            string filename = string.Empty;
            if (employee.Photo != null)
            {

                string uploadfolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                filename = Guid.NewGuid().ToString() + " _ " + employee.Photo.FileName;
                string FilePath = Path.Combine(uploadfolder, filename);
                using (var fileStream = new FileStream(FilePath, FileMode.Create))
                {
                    employee.Photo.CopyTo(fileStream);
                }
            }
            return filename;
        }
    }
}