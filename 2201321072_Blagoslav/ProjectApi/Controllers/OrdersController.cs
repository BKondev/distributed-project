using ManagementApi.Repositories;
using ManagementApi.CommConstants;
using ManagementApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersRepository _ordersRepo;
        private readonly CustomersRepository _customersRepo;
        private readonly EmployeesRepository _EmployeesRepo;

        public OrdersController()
        {
            _ordersRepo = new OrdersRepository();
            _customersRepo = new CustomersRepository();
            _EmployeesRepo = new EmployeesRepository();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            try
            {
                var order = new Order(request.Details, request.Quantity, request.Tip, request.Total, request.IsExpress, request.PlacedOn, request.Customer_ID, request.Employee_ID);
                _ordersRepo.Save(order);

                var response = GenerateResponse(order);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            try
            {
                var order = _ordersRepo.GetAll(n => n.Id == id).Find(i => i.Id == id);
                if (order == null)
                {
                    return NotFound();
                }

                var response = GenerateResponse(order);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var allOrders = _ordersRepo.GetAll(i => true);
                var response = allOrders.Select(order => GenerateResponse(order)).ToList();
                return Ok(response); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, UpdateOrderRequest request)
        {
            try
            {
                var order = _ordersRepo.GetAll(n => n.Id == id).Find(i => i.Id == id);
                if (order == null)
                {
                    return NotFound();
                }

                order.Details = request.Details;
                order.Quantity = request.Quantity;
                order.Tip = request.Tip;
                order.Total = request.Total;
                order.IsExpress = request.IsExpress;
                order.PlacedOn = request.PlacedOn;
                order.Customer_ID = request.Customer_ID;
                order.Employee_ID = request.Employee_ID;

                _ordersRepo.Save(order);
                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                var order = _ordersRepo.GetAll(n => n.Id == id).Find(i => i.Id == id);
                if (order == null)
                {
                    return NotFound(); 
                }

                _ordersRepo.Delete(order);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpGet("search/{details}")]
        public IActionResult SearchOrdersByDetails(string details)
        {
            try
            {
                var ordersSearchResult = _ordersRepo.GetAll(n => n.Details.ToUpper().Replace(" ", "").Contains(details.ToUpper()));
                var response = ordersSearchResult.Select(order => GenerateResponse(order)).ToList();
                return Ok(response);  
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        private OrderResponse GenerateResponse(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                Details = order.Details,
                Tip = order.Tip,
                Total = order.Total,
                IsExpress = order.IsExpress,
                PlacedOn = order.PlacedOn,
                Quantity = order.Quantity,
                Customer = _customersRepo.GetAll(n => n.Id == order.Customer_ID).Find(i => i.Id == order.Customer_ID),
                Employee = _EmployeesRepo.GetAll(n => n.Id == order.Employee_ID).Find(i => i.Id == order.Employee_ID)
            };
        }
    }
}
