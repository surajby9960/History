using History.Context;
using History.Model;
using History.Repositories.Interfaces;
using Dapper;

namespace History.Repositories
{
    public class EmployeeRepository : IEmployeeRepository,ICompanyRepository
    {
        private readonly DapperContext _context;
        public EmployeeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> AddEmployee(Employee employee)
        {
            var qry = @"insert into emp(name,city,isdeleted) 
                            OUTPUT
                            inserted.id,inserted.name,inserted.city into empHis(id,name, city)
                            values(@name, @city,0);
                            select cast(Scope_Identity() as int)";
                
            using(var connection=_context.CreateConnection())
            {
                connection.Open();
                var res=await connection.QuerySingleAsync<int>(qry,employee);
                var resulr=await connection.ExecuteAsync("Update emphis set operation='Inserted' where hid=@id",new { id = res });
                return res;
            }
        }

        public async Task Delete(int id)
        {

            var qry = @"update emp set isdeleted=1 output
                        inserted.id,inserted.name,inserted.city into empHis(id, name, city) where id= @id";

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var obje=await connection.QuerySingleAsync<Employee>("select *  from emp where id=@id",new { id = id });
                var res = await connection.ExecuteAsync(qry, obje);
                var resulr = await connection.ExecuteAsync("Update emphis set operation='Deleted' where hid=(SELECT max(hid) FROM empHis)");
                
            }
        }
        
        public async Task<BaseResponse> GetAll(int pageno,int pagesize)
        {
            List<Employee> employees = new List<Employee>();
            PaginationModel paginationModel = new PaginationModel();
            BaseResponse baseResponse=new BaseResponse();
            var @val = (pageno - 1) * pagesize;
            var qry = @"select ROW_NUMBER() over(order by id desc) as SrNo, * from emp order by id desc
                    Offset @val rows fetch next @pagesize rows only;
                    select @pageno as Pageno,count(distinct id) as totalpages from emp";
            var par = new { pageno, pagesize, val };
            using(var con=_context.CreateConnection())
            {
                con.Open();
                var res = await con.QueryMultipleAsync(qry, par);
                var emp = await res.ReadAsync<Employee>();
                employees = emp.ToList();
                var paginaton=await res.ReadAsync<PaginationModel>();
                paginationModel = paginaton.FirstOrDefault();
                int last = 0;
                int pagecount = 0;
                last = paginationModel.TotalPages % pagesize;
                pagecount= paginationModel.TotalPages / pagesize;
                paginationModel.PageCount = paginationModel.TotalPages;
                paginationModel.TotalPages = pagecount;
                if(last>0)
                {
                    paginationModel.TotalPages = pagecount+1;
                }
                baseResponse.EmpData=employees;
                baseResponse.PageData = paginationModel;
                return baseResponse;
               

            }
           
        }

        public async Task<IEnumerable<Company>> GetallComp()
        {
            using(var con=_context.CreateConnection())
            {
                con.Open();
                var compnay = await con.QueryAsync<Company>("Select * from company");
                return compnay.ToList();
            }
        }

        public async Task<int> InsertCompany(Company company)
        {
            var qry = "insert into company(name,salary)values(@name,@salary)";
            using (var con = _context.CreateConnection())
            {
                con.Open();
                var res = await con.ExecuteAsync(qry,company);
                return res;
            }
        }

        public async Task<int> UpdaateComp(Company company)
        {

            var qry = "Update company set name=@name,salary=@salary where id=@id";
            using (var con = _context.CreateConnection())
            {
                con.Open();
                var res = await con.ExecuteAsync(qry, company);
                return res;
            }
        }

        public async Task<int> UpdateEmp(Employee employee)
        {
            var qry = @"update emp set name=@name,city=@city output
                        inserted.id,inserted.name,inserted.city into empHis(id, name, city) where id= @id";
                          
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var res = await connection.ExecuteAsync(qry, employee);
                var resulr = await connection.ExecuteAsync("Update emphis set operation='Updated' where hid=(SELECT max(hid) FROM empHis)");
                return res;
            }
        }

    }
}
