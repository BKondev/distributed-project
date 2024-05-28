using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ManagementApi.Entities;

namespace ManagementApp.ViewModels.Orders
{
    public class AddVM
    {

        public AddVM()
        {
            Customers = new List<Customer>();
            Employees = new List<Employee>();
        }

        [DisplayName("Details: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string Details { get; set; }

        [DisplayName("Quantity: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public int Quantity { get; set; }

        [DisplayName("Tip (BGN): ")]
        [Required(ErrorMessage = "This field is Required!")]
        public double Tip { get; set; }

        [DisplayName("Total (BGN): ")]
        [Required(ErrorMessage = "This field is Required!")]
        public double Total { get; set; }

        [DisplayName("Express Delivery: ")]
        public bool IsExpress { get; set; }

        [DisplayName("Customer: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public int Customer_ID { get; set; }

        public List<Customer> Customers { get; set; }

        [DisplayName("Employee: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public int Employee_ID { get; set; }

        public List<Employee> Employees { get; set; }
    }
}
