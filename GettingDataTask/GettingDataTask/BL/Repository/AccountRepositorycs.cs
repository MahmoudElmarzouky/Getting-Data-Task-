using Microsoft.Identity.Client;
using MicrotechTask.BL.Interface;
using MicrotechTask.DAL.Database;
using MicrotechTask.DAL.Entities;

namespace MicrotechTask.BL.Repository
{
    public class AccountRepositorycs : IAccountRepository
    {
        private readonly ApplicationDbContext dbContext;

        public AccountRepositorycs(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool CheckIdIsExist(string id)
        {
            return dbContext.Accounts.Find(id)!=null;
        }

        public IQueryable<Account> GetAllData()
        {
            var accounts= dbContext.Accounts.Select(account=>new Account {
                AccNumber=account.AccNumber.Trim(),
                AccParent= (account.AccParent!=null)? account.AccParent.Trim():null,
                Balance=account.Balance
            }
            ).AsQueryable();

            return accounts;
        }
    }
}
