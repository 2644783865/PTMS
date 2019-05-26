using System.Threading.Tasks;
using System.Transactions;

namespace PTMS.DataServices.Infrastructure
{
    public interface IDbTransactionHelper
    {
        TransactionScope RequireReadCommitedTransaction();
        Task<int> SaveChangesAsync();
    }
}
