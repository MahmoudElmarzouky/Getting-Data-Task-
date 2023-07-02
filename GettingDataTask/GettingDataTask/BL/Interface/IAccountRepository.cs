using MicrotechTask.DAL.Entities;

namespace MicrotechTask.BL.Interface
{
    public interface IAccountRepository
    {
        IQueryable<Account> GetAllData();
        bool CheckIdIsExist(string id);
    }
}
