using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdminPortal.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string PhysicalAddress { get; set; }
        public string PostalAddress { get; set; }
        public int StudentId { get; set; }
    }
}
