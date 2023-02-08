using EmpManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpManagement.ViewModels
{
    public class EmployeeEditViewModel : EmployeeViewModel
    {
        public int Id { get; set; }
        public string  ExistingPhotoPath { get; set; }
    }
}
