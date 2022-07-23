using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMvc.Models {
    public class Seller {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1}")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} required")]
        [Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")]
        [Display(Name = "Base Salary")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double BaseSalary { get; set; }
        [Display(Name = "Birth Date")]
        [Required(ErrorMessage = "{0} required")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller() { }

        public Seller(int id, string name, string email, double baseSalary, DateTime birthDate, Department department) {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.BaseSalary = baseSalary;
            this.BirthDate = birthDate;
            this.Department = department;
        }

        public void AddSales(SalesRecord sr) {
            this.Sales.Add(sr);
        }

        public void RemoveSales(SalesRecord sr) {
            this.Sales.Remove(sr);
        }

        public double TotalSales(DateTime initial, DateTime final) {
            return this.Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr => sr.Amount);
        }
    }
}
