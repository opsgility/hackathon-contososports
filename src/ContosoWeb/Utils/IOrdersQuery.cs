using System;
using System.Threading.Tasks;
using Contoso.Models;
using ContosoWeb.ViewModels;

namespace ContosoWeb.Utils
{
    public interface IOrdersQuery
    {
        Task<Order> FindOrderAsync(int id);
        Task<OrdersModel> IndexHelperAsync(string username, DateTime? start, DateTime? end, string invalidOrderSearch, bool isAdminSearch);
    }
}
