using PTMS.Persistance;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace PTMS.DataServices.Infrastructure
{
    public class DbTransactionHelper : IDbTransactionHelper
    {
        private readonly ApplicationDbContext _dbContext;

        public DbTransactionHelper(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TransactionScope RequireReadCommitedTransaction()
        {
            var transactionOptions =
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TimeSpan.FromMinutes(1)
                };

            var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);

            return transactionScope;
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}
