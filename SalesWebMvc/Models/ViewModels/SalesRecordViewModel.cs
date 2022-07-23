using System.Collections.Generic;
using SalesWebMvc.Models.Enums;
using System;
using System.Linq;

namespace SalesWebMvc.Models.ViewModels {
    public class SalesRecordViewModel {
        public SalesRecord SalesRecord { get; set; }
        public ICollection<Seller> Sellers { get; set; } 
    }
}
