using System;

namespace Auto_Parts_Store.Models
{
    public class AutoPart
    {
        public int PartID { get; set; }
        public string PartName { get; set; }
        public string PartNumber { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
        public int MinimumStock { get; set; }
        public int CategoryID { get; set; }
        public string Notes { get; set; }
    }
   
}