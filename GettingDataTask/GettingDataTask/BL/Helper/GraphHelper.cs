using Microsoft.Extensions.Primitives;
using MicrotechTask.DAL.Entities;

namespace MicrotechTask.BL.Helper
{
    public class GraphHelper
    {
        // Array of lists for
        // Adjacency List Representation 
        private IDictionary<string, List<string>> adj;

        // To get balance for each Node 
        private IDictionary<string, decimal> Balances = new Dictionary<string, decimal>();
        
        // to save paths for each root 
        public IDictionary<string, decimal> paths = new Dictionary<string, decimal>();

        // list of accounts 
        private readonly IQueryable<Account> accounts;
        // to get total of balances for each top levels (root)
        public decimal TotalBalane { 
                    get; set;
        }
        public GraphHelper(IQueryable<Account> accounts)
        {
            this.accounts = accounts;
        }

        // Construct the Graph
        public void ConstructGraphFromAccounts()
        {
            int numberOfVertices = accounts.Count();
            adj = new Dictionary<string, List<string>>();
            foreach(Account account in accounts)
            {
                AddAdgeFromAccount(account);
            }
            
        }

        private void AddAdgeFromAccount(Account account)
        {
           
            if (account.AccParent == null)
            {
                return;
            }
            account.AccParent = account.AccParent;
            account.AccNumber = account.AccNumber;
            if (!adj.ContainsKey(account.AccParent))
            {
                adj.Add(account.AccParent, new List<string>());
            }
            adj[account.AccParent].Add(account.AccNumber);

            if (!adj.ContainsKey(account.AccNumber))
            {
                adj.Add(account.AccNumber, new List<string>());
            }
            adj[account.AccNumber].Add(account.AccParent);
        }

        public void GetBalancesFromAccounts()
        {
            foreach (Account account in accounts)
            {
                if (account.Balance != null)
                {
                    Balances.Add(account.AccNumber, (decimal)account.Balance);
                }        
            }
        }
        public void DfsForTopLevelTotalBalance(string topLevel, string parent)
        {
            bool ifLeafNode = true;

            foreach(var child in adj[topLevel])
            {
                if(child != parent)
                {
                    ifLeafNode = false;
                    DfsForTopLevelTotalBalance(child,topLevel);
                }
            }
            if (ifLeafNode) 
                TotalBalane += Balances[topLevel];
        }
        public void DfsForDetailsOfTotalBalane(string topLevel,string parent,string path)
        {
            bool ifLeafNode = true;
            path += (topLevel+"-");

            foreach (var child in adj[topLevel])
            {
                if (child != parent)
                {
                    ifLeafNode = false;
                    DfsForDetailsOfTotalBalane(child, topLevel,path);
                }
            }
            if (ifLeafNode)
                paths.Add(path,Balances[topLevel]);
        }

    }
}
