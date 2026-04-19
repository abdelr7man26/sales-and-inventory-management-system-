using System;
namespace Auto_Parts_Store.Models
{
    public class ReturnHeaderDTO
    {
        public int InvoiceID { get; set; }
        public string ReturnType { get; set; }    
        public string PaymentMethod { get; set; } 
        public decimal TotalAmount { get; set; }
        public int UserID { get; set; }
        public string Notes { get; set; }
    }

    public class ReturnDetailDTO
    {
        public int PartID { get; set; }
        public int Quantity { get; set; }
        public decimal RefundAmount { get; set; }
    }
}