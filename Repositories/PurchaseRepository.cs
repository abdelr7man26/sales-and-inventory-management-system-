// This file is intentionally kept as a thin pass-through to PurchasesRepository.
// The original PurchaseRepository had a critical data-corruption bug: it passed
// header.CustomerID into the supplierID column, wrote NULL for every purchase
// supplier, and never updated supplier balances or inventory transaction logs.
// PurchasesRepository (purchasesRepository.cs) is the correct, complete implementation.
// IPurchaseRepository is retained here only so existing callers still compile;
// all new code must use IPurchasesRepository / PurchasesRepository directly.

using Auto_Parts_Store.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly PurchasesRepository _inner = new PurchasesRepository();

        public async Task<int> SavePurchaseInvoiceAsync(InvoiceHeader header, List<InvoiceDetail> details)
        {
            await _inner.SavePurchaseInvoiceAsync(header, details);
            return 0; // PurchasesRepository does not return the new ID; callers should migrate to IPurchasesRepository
        }
    }
}