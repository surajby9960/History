using History.Model;

namespace History.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {

        public Task<BaseResponse> GetAll(int pageno,int pagesize);
        public Task <int> AddEmployee(Employee employee);
        public Task<int> UpdateEmp(Employee employee);
        public Task Delete(int id);
    }
}
