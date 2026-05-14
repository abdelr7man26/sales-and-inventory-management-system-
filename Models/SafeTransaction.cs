using System;

namespace Auto_Parts_Store.Models
{
    public class SafeTransaction
    {
        public int      ID              { get; set; }
        public decimal  Amount          { get; set; }
        public string   TransactionType { get; set; }
        public string   Description     { get; set; }
        public DateTime TransactionDate { get; set; }
        public int      UserID          { get; set; }
        public int?     InvoiceID       { get; set; }
        // FK to person.ID — required for reliable account-statement lookups.
        // NULL is allowed for manual safe deposits/withdrawals not tied to a person.
        public int?     PersonID        { get; set; }
    }
}