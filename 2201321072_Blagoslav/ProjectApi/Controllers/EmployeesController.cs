using ManagementApi.Repositories;
using ManagementApi.CommConstants;
using ManagementApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        public EmployeesController()
        {
        }

        [HttpPost]
        public IActionResult CreateEmployee(CreateEmployeeRequest request)
        {
            try
            {

                EmployeesRepository EmployeesRepo = new EmployeesRepository();
                Employee Employee = new Employee(request.FirstName, request.LastName, request.EmailAddress, request.Salary, request.IsFullTime, request.RegisteredOn);
                EmployeesRepo.Save(Employee);

                var response = GenerateResponse(Employee);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            try
            {
                EmployeesRepository repo = new EmployeesRepository();
                Employee Employee = repo.GetAll(n => n.Id == id).Find(i => i.Id == id);

                if (Employee == null)
                {
                    return NotFound();
                }

                var response = GenerateResponse(Employee);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            try
            {
                EmployeesRepository EmployeesRepo = new EmployeesRepository();
                OrdersRepository ordersRepo = new OrdersRepository();
                List<Employee> allEmployees = EmployeesRepo.GetAll(i => true);
                List<Order> allOrders = ordersRepo.GetAll(i => true);

                foreach (var Employee in allEmployees)
                {
                    int currentCount = 0;
                    foreach (var order in allOrders)
                    {
                        if (order.Employee_ID == Employee.Id)
                        {
                            currentCount++;
                        }
                    }

                    Employee.TotalOrders = currentCount;
                }

                var response = allEmployees.Select(Employee => GenerateResponse(Employee)).ToList();
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, UpdateEmployeeRequest request)
        {
            try
            {
                EmployeesRepository repo = new EmployeesRepository();

                var existingEmployee = repo.GetAll(n => n.Id == id).Find(i => i.Id == id);
                if (existingEmployee == null)
                {
                    return NotFound();
                }
                existingEmployee.FirstName = request.FirstName;
                existingEmployee.LastName = request.LastName;
                existingEmployee.EmailAddress = request.EmailAddress;
                existingEmployee.Salary = request.Salary;
                existingEmployee.IsFullTime = request.IsFullTime;
                existingEmployee.RegisteredOn = request.RegisteredOn;

                repo.Save(existingEmployee);
                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                EmployeesRepository repo = new EmployeesRepository();

                Employee Employee = repo.GetAll(n => n.Id == id).Find(i => i.Id == id);

                if (Employee == null)
                {
                    return NotFound();
                }

                repo.Delete(Employee);
                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpGet("search/{searchWord}")]
        public IActionResult SearchEmployeesByFirstName(string searchWord)
        {
            try
            {
                EmployeesRepository repo = new EmployeesRepository();
                List<Employee> EmployeesSearchResult = repo.GetAll(n => n.FirstName.ToUpper().Replace(" ", "").Contains(searchWord.ToUpper()));

                var response = EmployeesSearchResult.Select(Employee => GenerateResponse(Employee)).ToList();
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        private EmployeeResponse GenerateResponse(Employee Employee)
        {
            var response = new EmployeeResponse
            {
                Id = Employee.Id,
                FirstName = Employee.FirstName,
                LastName = Employee.LastName,
                EmailAddress = Employee.EmailAddress,
                Salary = Employee.Salary,
                IsFullTime = Employee.IsFullTime,
                RegisteredOn = Employee.RegisteredOn,
                TotalOrders = Employee.TotalOrders
            };

            return response;
        }
    }
    
}