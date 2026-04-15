using Auto_Parts_Store.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Auto_Parts_Store.Repositories;


namespace Auto_Parts_Store.Repositories
{
    public interface IPurchasesRepository
    {
        Task<bool> SavePurchaseInvoiceAsync(InvoiceHeader header, List<InvoiceDetail> details);
    }
}
