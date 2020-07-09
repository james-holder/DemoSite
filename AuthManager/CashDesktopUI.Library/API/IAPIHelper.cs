using CashDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace CashDesktopUI.Library.API
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}