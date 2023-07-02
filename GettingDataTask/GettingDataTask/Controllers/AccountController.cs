using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicrotechTask.BL.Helper;
using MicrotechTask.BL.Interface;
using MicrotechTask.BL.Repository;
using MicrotechTask.DAL.Entities;
using MicrotechTask.Models;
using System.Linq;

namespace MicrotechTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountRepository accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TopLevelAccountVM>> GetTopLevelAccounts()
        {
            var accounts = accountRepository.GetAllData();

            var topLevels = accounts.Where(a => a.AccParent == null).ToList();

            GraphHelper graphHelper = GetGraph(accounts);

            List<TopLevelAccountVM> topLevelAccount = new List<TopLevelAccountVM>();

            foreach (var t in topLevels)
            {
                graphHelper.DfsForTopLevelTotalBalance(t.AccNumber, "-1");
                topLevelAccount.Add(new TopLevelAccountVM
                {
                    TopAccount = t.AccNumber,
                    TotalBalance = graphHelper.TotalBalane
                });
                graphHelper.TotalBalane = 0;
            }

            return topLevelAccount.AsEnumerable().ToList();
        }

      

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<DetailsForTotalBalanceVM>> GetDetailsForTotalBalance(string id)
        {
            if (!accountRepository.CheckIdIsExist(id))
                return NotFound();
            var accounts = accountRepository.GetAllData();
            GraphHelper graphHelper =  GetGraph(accounts);
            graphHelper.DfsForDetailsOfTotalBalane(id, "-1", "");
            IDictionary<string, decimal> paths = graphHelper.paths;
            List<DetailsForTotalBalanceVM> details = new List<DetailsForTotalBalanceVM>();
            foreach (var path in paths)
            {
                details.Add(
                    new DetailsForTotalBalanceVM
                    {
                        Path=path.Key,
                        Balance=path.Value
                    }
                    );
            }
            return details;

        }
        private static GraphHelper GetGraph(IQueryable<Account> accounts)
        {
            GraphHelper graphHelper = new GraphHelper(accounts);
            graphHelper.ConstructGraphFromAccounts();
            graphHelper.GetBalancesFromAccounts();
            return graphHelper;
        }
    }

}
  
