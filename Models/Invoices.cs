using System;

namespace Auto_Parts_Store.Models
{
    public class InvoiceHeader
    {
        public DateTime Time { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserID { get; set; }
        public int? CustomerID { get; set; }
        public string InvoiceType { get; set; }
        public decimal PaidAmount { get; set; }
    }

    public class InvoiceDetail
    {
        public int PartID { get; set; }
        public decimal Quantity { get; set; }
        public decimal PartPrice { get; set; }
        public decimal Total { get; set; }
    }
}