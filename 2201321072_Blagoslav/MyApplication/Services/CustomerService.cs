namespace ManagementApp.Services
{
    public class CustomerService : BaseService
    {

        public static CustomerService Instance { get; } = new CustomerService();

        public CustomerService() : base("Customers")
        {

        }
    }
}
