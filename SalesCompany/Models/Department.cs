using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesCompany.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} obrigatório")]
        [Display(Name = "Nome")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tamanho do {0} mínimo 3, máximo 60")]
        public string Name { get; set; }

        private ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        public Department()
        {
        }
        
        public Department(string name)
        {
            Name = name;
        }
        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }
        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sellers.Sum(vs => vs.TotalSales(initial, final));
        }
    }
}
