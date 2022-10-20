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
        private readonly ICompanyRepository companyRepository;
        public EmployeeController(IEmployeeRepository repository, ICompanyRepository companyRepository)
        {
            this.repository = repository;
            this.companyRepository = companyRepository;
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
        [HttpDelete("Deleteemp")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                 await repository.Delete(id);
                return Ok(1);
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

        [HttpGet("AllCompany")]
        public async Task <IActionResult> GetAllcomp()
        {
            try
            {
                var comp = await companyRepository.GetallComp();
                return Ok(comp);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("AddComp")]
        public async Task <IActionResult > AddCompp(Company company)
        {
            try
            {
                var res = await companyRepository.InsertCompany(company);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateComp")]
        public async Task<IActionResult> Updatecomp(Company company)
        {
            try
            {
                var res = await companyRepository.UpdaateComp(company);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
