using WebApplication1.Models;

namespace WebApplication1.repo
{
    public class CustomerRepository:ICustomerRepository
    {
        Context context;
        public CustomerRepository(Context context)
        {
            this.context = context;
        }
        public Customer GetByID(int id)
        {
            Customer customer = context.Customers.FirstOrDefault(c=>c.ID== id&& c.ApplicationUser.Id==c.ApplicationUserId);
            return customer;
        }
    }
}
