using PTMS.Common.Enums;

namespace PTMS.BusinessLogic.Models.Shared
{
    public class FileModel
    {
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
        public FileFormatEnum FileFormat { get; set; }
    }
}
