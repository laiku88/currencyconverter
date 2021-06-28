using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CurrencyConverter.Models
{
    public class RetailPriceModel
    {
        public string CurrencyCode { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Location { get;set; }
        public string SkuId { get; set; }
        public string Type { get; set; }
        public double RetailPrice { get; set; }
    }
}