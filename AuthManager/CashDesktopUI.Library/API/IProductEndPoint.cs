using CashDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CashDesktopUI.Library.API
{
    public interface IProductEndPoint
    {
        Task<List<ProductModel>> GetAll();
    }
}