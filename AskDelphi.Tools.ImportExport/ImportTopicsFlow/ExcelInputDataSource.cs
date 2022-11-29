using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.ImportExport.ImportTopicsFlow
{
    public class ExcelInputDataSource : IEnumerable<Dictionary<string, string>>
    {
        readonly string filename;

        public ExcelInputDataSource(string filename)
        {
            this.filename = filename;
        }

        public IEnumerator<Dictionary<string, string>> GetEnumerator()
        {
            return new ExcelInputDataSourceEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ExcelInputDataSourceEnumerator(this);

        }

        private class ExcelInputDataSourceEnumerator : IEnumerator<Dictionary<string, string>>
        {
            private ExcelInputDataSource excelInputDataSource;
            private readonly ExcelPackage excel;
            private readonly ExcelWorksheet worksheet;
            private int total = 0;
            private int index = -1;
            private List<string> columns = new List<string>();

            public ExcelInputDataSourceEnumerator(ExcelInputDataSource excelInputDataSource)
            {
                this.excelInputDataSource = excelInputDataSource;

                excel = new ExcelPackage(new System.IO.FileInfo(excelInputDataSource.filename));
                worksheet = excel.Workbook.Worksheets.First();
                for (int column = 1; ; column++)
                {
                    string value = $"{worksheet.Cells[1, column].Value}";
                    if (string.IsNullOrWhiteSpace(value)) break;
                    columns.Add(value);
                }
                total = worksheet.Dimension.End.Row - 1;
            }

            public Dictionary<string, string> Current => GetCurrent();

            object IEnumerator.Current => GetCurrent();

            private Dictionary<string, string> GetCurrent()
            {
                Dictionary<string, string> result = new();
                for (int column = 1; column <= columns.Count; column++)
                {
                    int row = index + 2; // 1-based and skipping header row
                    var cell = worksheet.Cells[row, column];
                    result[columns[column - 1]] = $"{worksheet.Cells[row, column].Value}";
                }
                return result;
            }

            public void Dispose()
            {
                excel?.Dispose();
            }

            public bool MoveNext()
            {
                index++;
                return index < total;
            }

            public void Reset()
            {
                index = -1;
            }
        }
    }
}
