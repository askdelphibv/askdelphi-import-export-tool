using AskDelphi.Tools.EditingAPI.EditingAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OfficeOpenXml;
using AskDelphi.Tools.ImportExport.Model;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.Style;

namespace AskDelphi.Tools.ImportExport.ExportTopicsFlow
{
    public class ExcelOutputFile : IOutputFile
    {
        private readonly string path;
        private readonly OperationContext operationContext;
        private readonly ExcelWorksheet sheet;
        private ExcelPackage excel;
        int row = 2;

        public ExcelOutputFile(string path, OperationContext operationContext)
        {
            this.path = path;
            this.operationContext = operationContext;

            excel = new ExcelPackage();
            sheet = excel.Workbook.Worksheets.Add($"Export {DateTime.Now.ToString("d")}");
            if (null != operationContext?.Configuration?.Mappings)
            {
                int column = 1;
                foreach (var mapping in operationContext.Configuration.Mappings)
                {
                    var cell = sheet.Cells[1, column];
                    cell.Value = mapping.TargetField;
                    StyleHeaderCell(cell);
                    column++;
                }
            }
        }

        public void AddTopic(TopicTypeEditorDefinition parts)
        {
            JObject source = JObject.FromObject(parts);
            int column = 1;
            foreach (var mapping in operationContext.Configuration.Mappings?.ToList() ?? new List<MappingEntry>())
            {
                JToken token = source.SelectToken(mapping.JSONPath);
                var cell = sheet.Cells[row, column];

                string stringValue = TokenAsString(token);

                cell.Value = stringValue;
                StyleValueCell(cell);

                column++;
            }
            row++;
        }

        private static void StyleHeaderCell(ExcelRange cell)
        {
            cell.Style.Font.Bold = true;
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
        }

        private static void StyleValueCell(ExcelRange cell)
        {
            cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
            cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
            cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
        }

        private static string TokenAsString(JToken token)
        {
            string stringValue;
            if (token.Type == JTokenType.String || token.Type == JTokenType.Guid)
            {
                stringValue = token.ToObject<string>();
            }
            else
            {
                stringValue = token.ToString(Newtonsoft.Json.Formatting.None);
            }

            return stringValue;
        }

        public void Close()
        {
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(dir))
            {
                Directory.CreateDirectory(dir);
            }
            excel.SaveAs(new FileInfo(path));
        }
    }
}
