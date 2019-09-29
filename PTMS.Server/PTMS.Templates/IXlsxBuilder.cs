using PTMS.Templates.Models;

namespace PTMS.Templates
{
    public interface IXlsxBuilder
    {
        byte[] GetObjectsTable(ObjectsPrintModel model);
    }
}
