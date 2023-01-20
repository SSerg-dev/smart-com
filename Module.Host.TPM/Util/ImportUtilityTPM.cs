using Core.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Import;
using Utility.Import.ImportModelBuilder;

namespace Module.Host.TPM.Util
{
    public class ImportUtilityTPM
    {
        /// <summary>
        /// Распарсить csv-строку по колонкам
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string[] ParseCSVRow(string source, char separator, char? quote)
        {
            if (quote.HasValue)
            {
                List<string> tokens = new List<string>();
                int last = -1;
                int current = 0;
                bool inText = false;
                while (current < source.Length)
                {
                    char ch = source[current];
                    if (ch == quote.Value)
                    {
                        inText = !inText;
                    }
                    else if (ch == separator)
                    {
                        if (!inText)
                        {
                            tokens.Add(source.Substring(last + 1, (current - last - 1)));
                            last = current;
                        }
                    }
                    current++;
                }
                if (last != source.Length - 1)
                {
                    tokens.Add(source.Substring(last + 1));
                }
                return tokens.ToArray();
            }
            else
            {
                return source.Split(new char[] { separator }, StringSplitOptions.None);
            }
        }

        private static bool ParseRow(string row, Guid? importId, IImportModelBuilder<string[]> builder, IImportValidator validator, char separator, char? quote, bool hasHeader, IList<IEntity<Guid>> records, ref List<string> errors, out IList<Tuple<string, string>> notBuildedRecords, out IList<Tuple<IEntity<Guid>, string>> notValidRecords, string[] splitedRow = null)
        {
            bool isEmpty = true;
            notBuildedRecords = new List<Tuple<string, string>>();
            notValidRecords = new List<Tuple<IEntity<Guid>, string>>();
            if (!String.IsNullOrEmpty(row) || splitedRow != null)
            {
                isEmpty = false;
                if (splitedRow == null)
                {
                    splitedRow = ParseCSVRow(row, separator, quote);
                }
                IEntity<Guid> item;
                IList<string> buildErrors;
                if (builder.Build(splitedRow, importId, out item, out buildErrors))
                {
                    IList<string> validateErrors;
                    if (validator.Validate(item, out validateErrors))
                    {
                        records.Add(item);
                    }
                    else
                    {
                        errors.AddRange(validateErrors);
                        notValidRecords.Add(new Tuple<IEntity<Guid>, string>(item, String.Join(", ", validateErrors)));
                    }
                }
                else
                {
                    errors.AddRange(buildErrors);
                    notBuildedRecords.Add(new Tuple<string, string>(row, String.Join(", ", buildErrors)));
                }
            }
            return !isEmpty;
        }

        private static int ColumnIndex(string reference)
        {
            int ci = 0;
            reference = reference.ToUpper();
            for (int ix = 0; ix < reference.Length && reference[ix] >= 'A'; ix++)
                ci = (ci * 26) + ((int)reference[ix] - 64);
            return ci;
        }

        /// <summary>
        /// Разбор XLSX-файла импорта
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="importId"></param>
        /// <param name="builder"></param>
        /// <param name="separator"></param>
        /// <param name="sourceFileRecordCount"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static IList<IEntity<Guid>> ParseXLSXFile(string filePath, Guid? importId, IImportModelBuilder<string[]> builder, IImportValidator validator, string separator, string quote, bool hasHeader, out int sourceFileRecordCount, out List<string> errors, out IList<Tuple<string, string>> notBuildedRecords, out IList<Tuple<IEntity<Guid>, string>> notValidRecords)
        {
            sourceFileRecordCount = 0;
            IList<IEntity<Guid>> result = new List<IEntity<Guid>>();
            notBuildedRecords = new List<Tuple<string, string>>();
            notValidRecords = new List<Tuple<IEntity<Guid>, string>>();
            errors = new List<string>();
            SpreadsheetDocument book = SpreadsheetDocument.Open(filePath, false);
            StringValue sID = book.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault().Id;
            WorksheetPart wSheet = (WorksheetPart)book.WorkbookPart.GetPartById(sID.ToString());
            using (OpenXmlReader reader = OpenXmlReader.Create(wSheet))
            {
                Row row;
                int columnsCount = 0;
                while (reader.Read())
                {
                    if (reader.ElementType == typeof(Row))
                    {
                        row = (Row)reader.LoadCurrentElement();
                        if (row.RowIndex == 1 && hasHeader)
                        {
                            foreach (Cell cell in row.Elements<Cell>())
                            {
                                if (cell.CellValue != null)
                                {
                                    columnsCount++;
                                }
                            }
                            continue;
                        }
                        if (!row.HasChildren)
                        {
                            continue;
                        }
                        string text;
                        List<string> strRow = new List<string>();
                        int lastCellIndex = 0;
                        foreach (Cell cell in row.Elements<Cell>())
                        {
                            int cureCellIndex = ColumnIndex(cell.CellReference);
                            int emptyCellsCount = cureCellIndex - lastCellIndex;
                            if (emptyCellsCount != 1)
                            {
                                for (int i = 1; i < emptyCellsCount; i++)
                                {
                                    strRow.Add(String.Empty);
                                }
                            }
                            lastCellIndex = cureCellIndex;
                            if (cell.CellValue != null)
                            {
                                text = String.Empty;
                            }
                            else
                            {
                                strRow.Add(String.Empty);
                                continue;
                            }
                            if (cell.DataType != null)
                            {
                                if (cell.DataType == CellValues.SharedString)
                                {
                                    int id = -1;
                                    if (Int32.TryParse(cell.InnerText, out id))
                                    {
                                        SharedStringItem item = book.WorkbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                        if (item.Text != null)
                                        {
                                            text = item.Text.Text;
                                        }
                                        else if (item.InnerText != null)
                                        {
                                            text = item.InnerText;
                                        }
                                        else if (item.InnerXml != null)
                                        {
                                            text = item.InnerXml;
                                        }

                                    }
                                }
                                else if (cell.DataType == CellValues.Number)
                                {
                                    text = cell.InnerText;
                                }
                            }
                            else if (cell.InnerText != null)
                            {
                                if(cell.CellFormula !=null && !String.IsNullOrEmpty(cell.CellFormula.InnerText))
                                {
                                    text = cell.CellValue.InnerText;
                                }
                                else
                                {
                                    text = cell.InnerText;
                                }
                            }
                            strRow.Add(text);
                        }
                        while (strRow.Count < columnsCount)
                        {
                            strRow.Add(String.Empty);
                        }
                        if (strRow.All(x => String.IsNullOrEmpty(x) || String.IsNullOrWhiteSpace(x)))
                        {
                            continue;
                        }
                        if (ParseRow(null, importId, builder, validator, ';', null, hasHeader, result, ref errors, out notBuildedRecords, out notValidRecords, splitedRow: strRow.ToArray()))
                        {
                            sourceFileRecordCount++;
                        }

                    }
                }
            }
            return result;
        }     
    }
}
