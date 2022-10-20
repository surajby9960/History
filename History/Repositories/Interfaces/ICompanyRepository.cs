using History.Model;

namespace History.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        public Task<IEnumerable<Company>> GetallComp();
        public Task<int> InsertCompany(Company company);
        public Task<int> UpdaateComp(Company company);
    }
}
