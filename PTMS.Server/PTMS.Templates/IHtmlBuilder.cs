using PTMS.Templates.Models;
using System.Threading.Tasks;

namespace PTMS.Templates
{
    public interface IHtmlBuilder
    {
        Task<string> GetObjectsTable(ObjectsPrintModel model);
    }
}
