using System;

namespace Auto_Parts_Store.Models
{
    // ── Inventory reports ──────────────────────────────────────

    public class StockAuditItem
    {
        public int     PartID            { get; set; }
        public string  PartName          { get; set; }
        public string  PartNumber        { get; set; }
        public string  Category          { get; set; }
        public int     Quantity          { get; set; }
        public decimal PurchasePrice     { get; set; }
        public decimal SellingPrice      { get; set; }
        public decimal TotalCostValue    { get; set; }
        public decimal TotalSellingValue { get; set; }
        public decimal PotentialProfit   { get; set; }
        public int     MinimumStock      { get; set; }
        public string  StockStatus       { get; set; }
    }

    public class StagnantItem
    {
        public int     PartID            { get; set; }
        public string  PartName          { get; set; }
        public string  PartNumber        { get; set; }
        public string  Category          { get; set; }
        public int     Quantity          { get; set; }
        public decimal PurchasePrice     { get; set; }
        public decimal TotalValue        { get; set; }
        public int     DaysSinceLastSale { get; set; }
    }

    public class LowStockItem
    {
        public int     PartID                     { get; set; }
        public string  PartName                   { get; set; }
        public string  PartNumber                 { get; set; }
        public string  Category                   { get; set; }
        public int     CurrentStock               { get; set; }
        public int     MinimumStock               { get; set; }
        public int     Shortage                   { get; set; }
        public decimal PurchasePrice              { get; set; }
        public decimal EstimatedReplenishmentCost { get; set; }
    }

    // ── Sales & profit reports ─────────────────────────────────

    public class ProfitReportItem
    {
        public int      InvoiceID       { get; set; }
        public DateTime SaleDate        { get; set; }
        public int      PartID          { get; set; }
        public string   PartName        { get; set; }
        public string   Category        { get; set; }
        public decimal  Quantity        { get; set; }
        public decimal  SellingPrice    { get; set; }
        public decimal  PurchasePrice   { get; set; }
        public decimal  Revenue         { get; set; }
        public decimal  GrossProfit     { get; set; }
        public decimal  ProfitMarginPct { get; set; }
    }

    public class TopSellingItem
    {
        public int     PartID       { get; set; }
        public string  PartName     { get; set; }
        public string  PartNumber   { get; set; }
        public string  Category     { get; set; }
        public int     TotalSold    { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalProfit  { get; set; }
        public decimal AvgMarginPct { get; set; }
    }

    // ── Financial reports ──────────────────────────────────────

    public class SafeLedgerEntry
    {
        public int      ID              { get; set; }
        public DateTime TransactionDate { get; set; }
        public string   TransactionType { get; set; }
        public decimal  Amount          { get; set; }
        public string   Description     { get; set; }
        public string   UserName        { get; set; }
        public string   InvoiceType     { get; set; }
        public int?     InvoiceID       { get; set; }
        public decimal  RunningBalance  { get; set; }
    }

    public class PersonBalance
    {
        public int     PersonID           { get; set; }
        public string  PersonName         { get; set; }
        public string  Phone              { get; set; }
        public string  Address            { get; set; }
        public decimal OutstandingBalance { get; set; }
        public int     InvoiceCount       { get; set; }
    }

    // ── Summary aggregates (calculated in service layer) ───────

    public class StockSummary
    {
        public int     TotalItems            { get; set; }
        public decimal TotalCostValue        { get; set; }
        public decimal TotalSellingValue     { get; set; }
        public decimal TotalPotentialProfit  { get; set; }
        public int     LowStockCount         { get; set; }
        public int     OutOfStockCount       { get; set; }
    }

    public class ProfitSummary
    {
        public decimal TotalRevenue      { get; set; }
        public decimal TotalCost         { get; set; }
        public decimal TotalGrossProfit  { get; set; }
        public decimal AvgMarginPct      { get; set; }
        public int     TotalTransactions { get; set; }
    }
}
