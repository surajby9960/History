using History.Model;
using History.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace History.Controller
{
    [Route("Emp/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository repository;
        public EmployeeController(IEmployeeRepository repository)
        {
            this.repository = repository;
        }
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> Add(Employee employee)
        {
            try
            {
                var res=await repository.AddEmployee(employee);
                return Ok(res);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> Update(Employee employee)
        {
            try
            {
                var res = await repository.UpdateEmp(employee);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(int pageno,int pagesize)
        {
            try
            {
                var data=await repository.GetAll(pageno,pagesize);
                List<Employee> list=(List<Employee>)data.EmpData;
                BaseResponse baseResponse=new BaseResponse();
                baseResponse.EmpData=list;
                baseResponse.PageData = data.PageData;
                return Ok(baseResponse);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
