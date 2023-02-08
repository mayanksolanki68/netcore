using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmpManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details()
        {
            Employee model = _employeeRepository.GetEmployee(1);
            ViewData["PageTitle"] = "Employee Details";
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit()
        {
            Employee model = _employeeRepository.GetEmployee(1);
            ViewData["PageTitle"] = "Employee Details";
            return View(model);
        }


    }
}