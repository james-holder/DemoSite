using CashDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace CashDesktopUI.Library.API
{
    public interface ISaleEndPoint
    {
        Task PostSale(SaleModel sale);
    }
}