using CashDestopUI.Models;
using System.Threading.Tasks;

namespace CashDestopUI.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}