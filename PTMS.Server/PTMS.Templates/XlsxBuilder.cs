using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PTMS.Templates.Models;
using System.Collections.Generic;
using System.IO;

namespace PTMS.Templates
{
    public class XlsxBuilder : IXlsxBuilder
    {
        public byte[] GetObjectsTable(ObjectsPrintModel model)
        {
            using (var ms = new MemoryStream())
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Лист 1");

                AddTable(excelSheet, model.HeadColumns, model.TableColumns);

                workbook.Write(ms);

                return ms.ToArray();
            }
        }

        private void AddTable(ISheet excelSheet, List<string> headColumns, List<List<string>> tableColumns)
        {
            IRow row = excelSheet.CreateRow(0);
            AddRow(row, headColumns);

            for (var index = 0; index < tableColumns.Count; index++)
            {
                row = excelSheet.CreateRow(index + 1);
                var columns = tableColumns[index];

                AddRow(row, columns);
            }
        }

        private void AddRow(IRow row, List<string> columns)
        {
            for (var index = 0; index < columns.Count; index++)
            {
                row.CreateCell(index).SetCellValue(columns[index]);
            }
        }
    }
}
