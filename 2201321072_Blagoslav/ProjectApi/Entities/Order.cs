using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.SqlServer;
using ManagementApi.Entities;

namespace ManagementApi.Entities
{
    public class Order : BaseEntity
    {
        public Order(string details, int quantity, double tip, double total, bool isExpress, DateTime placedOn, int customer_ID, int employee_ID)
        {
            Details = details;
            Quantity = quantity;
            Tip = tip;
            Total = total;
            IsExpress = isExpress;
            PlacedOn = placedOn;
            Customer_ID = customer_ID;
            Employee_ID = employee_ID;
        }

        public Order()
        {
            
        }

        public string Details { get; set; }

        public int Quantity { get; set; }

        public double Tip { get; set; }

        public double Total { get; set; }

        public bool IsExpress { get; set; }

        public DateTime PlacedOn { get; set; }

        #region Foreign Keys
        public int Customer_ID { get; set; }

        public int Employee_ID { get; set; }

        [ForeignKey("Customer_ID")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("Employee_ID")]
        public virtual Employee? Employee { get; set; }
        #endregion
    }
}
