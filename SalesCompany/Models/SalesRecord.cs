using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SalesCompany.Models.Enums;

namespace SalesCompany.Models
{
    public class SalesRecord
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double Quantidade { get; set; }

        public SaleStatus Status { get; set; }

        public Seller Vendendor { get; set; }

        public SalesRecord()
        {
        }

        public SalesRecord(DateTime data, double quantidade, SaleStatus status, Seller vendendor)
        {
            Data = data;
            Quantidade = quantidade;
            Status = status;
            Vendendor = vendendor;
        }
    }
}
