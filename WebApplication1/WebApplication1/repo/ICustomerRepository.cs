using WebApplication1.Models;

namespace WebApplication1.repo
{
    public interface ICustomerRepository
    {
         Customer GetByID(int id);
    }
}
