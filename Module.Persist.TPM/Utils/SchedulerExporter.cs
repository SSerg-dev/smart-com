using Core.MarsCalendar;
using Core.Settings;
using Module.Persist.TPM.Model.TPM;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Persist;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Module.Persist.TPM.Utils {
    /// <summary>
    /// Класс для экспорта календаря в Эксель
    /// </summary>
    public class SchedulerExporter {

        public SchedulerExporter() {
            isYearExport = false;
        }

        public SchedulerExporter(DateTime startDate, DateTime endDate) {
            isYearExport = true;
            yearStartDate = startDate;
            yearEndDate = endDate;
        }

        private readonly bool isYearExport;
        private readonly DateTime yearStartDate;
        private readonly DateTime yearEndDate;

        /// <summary>
        /// Получение пути к файлу
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string GetExportFileName(string userName) {
            string exportDir = AppSettingsManager.GetSetting("EXPORT_DIRECTORY", "~/ExportFiles");
            string userShortName = GetUserName(userName);
            string filename = String.Format("{0}_{1}_{2:yyyyMMddHHmmss}.xlsx", "Schedule", userShortName, DateTime.Now);
            if (!Directory.Exists(exportDir)) {
                Directory.CreateDirectory(exportDir);
            }
            return Path.Combine(exportDir, filename);
        }

        /// <summary>
        /// Выполнение экспорта календаяря в Эксель
        /// </summary>
        /// <param name="promoes"></param>
        /// <param name="clientIDs"></param>
        /// <param name="filePath"></param>
        /// <param name="context"></param>
        public void Export(List<Promo> promoes, IEnumerable<int> clientIDs, string filePath, DatabaseContext context) {
            IEnumerable<Promo> filteredPromoes = promoes.Where(p => !String.IsNullOrEmpty(p.BaseClientTreeIds) && p.StartDate.HasValue && p.EndDate.HasValue);
            List<Promo> promoDTOs = new List<Promo>();
            foreach (Promo promo in filteredPromoes) {
                string[] clientsIds = promo.BaseClientTreeIds.Split('|');
                if (promo.BaseClientTreeIds.Split('|').Length == 1) {
                    promoDTOs.Add(new Promo() {
                        BaseClientTreeIds = clientsIds[0],
                        StartDate = promo.StartDate,
                        EndDate = promo.EndDate,
                        CalendarPriority = promo.CalendarPriority,
                        ColorId = promo.ColorId,
                        Color = promo.Color,
                        Name = promo.Name
                    });
                } else {
                    // Промо для нескольких клиентов разбиваем, создаём модель Promo для каждого, добавляем результат в общий список
                    foreach (string id in clientsIds) {
                        promoDTOs.Add(new Promo() {
                            BaseClientTreeIds = id,
                            StartDate = promo.StartDate,
                            EndDate = promo.EndDate,
                            CalendarPriority = promo.CalendarPriority,
                            ColorId = promo.ColorId,
                            Color = promo.Color,
                            Name = promo.Name
                        });
                    }
                }
            }
            // сгруппированный по клиентам список промо
            IEnumerable<IGrouping<int, Promo>> promoesByClients = promoDTOs.GroupBy(p => Int32.Parse(p.BaseClientTreeIds));

            promoesByClients = promoesByClients.Where(p => clientIDs.Contains(p.Key));
            DateTime dt = DateTime.Now;
            // Получаем словарь клиентов
            IDictionary<int, ClientTree> clients = context.Set<ClientTree>().Where(cl => cl.IsBaseClient && clientIDs.Contains(cl.ObjectId) && (DateTime.Compare(cl.StartDate, dt) <= 0 && (!cl.EndDate.HasValue || DateTime.Compare(cl.EndDate.Value, dt) > 0))).Distinct().ToDictionary(x => x.ObjectId);
            Dictionary<int, DateTime> colToDateMap = new Dictionary<int, DateTime>();

            DateTime startdDate = promoDTOs.Min(p => p.StartDate).Value.DateTime;
            DateTime endDate = promoDTOs.Max(p => p.EndDate).Value.DateTime;

            if (isYearExport) {
                startdDate = startdDate > yearStartDate ? yearStartDate : startdDate;
                endDate = endDate < yearEndDate ? yearEndDate : endDate;
            }

            // создание файла
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
                IWorkbook wb = new XSSFWorkbook();
                ISheet sheet = wb.CreateSheet("Schedule");
                ICellStyle style = GetCellStyle(ref wb);
                // запись заголовка
                WriteHeader(startdDate, endDate, style, ref wb, out colToDateMap);
                int curRow = 3;
                // Для каждого клиента заполняем его промо
                foreach (IGrouping<int, Promo> clientPromo in promoesByClients) {
                    ClientTree client = null;
                    if (clients.TryGetValue(clientPromo.Key, out client)) {
                        PromoSortComparer cmp = new PromoSortComparer();
                        IEnumerable<Promo> sorted = clientPromo.OrderBy((p => p), cmp);
                        // Разбиваем Промо по строкам, получаем индекс последней строки
                        int lastRow = SplitPromoByRows(curRow, sorted.ToList(), sheet, colToDateMap, ref wb);
                        ICell clientCell = sheet.GetRow(curRow).CreateCell(0);

                        // Вертикальный текст
                        ICellStyle verticalStyle = GetCellStyle(ref wb);
                        verticalStyle.Rotation = 90;
                        verticalStyle.WrapText = true;

                        clientCell.SetCellValue(new XSSFRichTextString(client.Name));
                        clientCell.Row.Height = (short) -1;
                        if (lastRow - curRow > 1) {
                            sheet.AddMergedRegion(new CellRangeAddress(curRow, lastRow - 1, 0, 0));
                        }
                        clientCell.CellStyle = verticalStyle;
                        curRow = lastRow;
                    }
                }
                // Авто-ширину необходимо задавать после того, как все колонки заполнены и смержены
                for (int i = 0; i < colToDateMap.Count + 1; i++) {
                    sheet.AutoSizeColumn(i, true);
                }
                // Высоту необходимо задавать после того, как заполнены все строки и смержены все необходимые ячейки
                for (int i = 3; i < curRow; i++) {
                    sheet.GetRow(i).Height = 30 * 20;
                }
                wb.Write(stream);
            }
        }

        /// <summary>
        /// Разбиение Промо по строкам с учётом приоритета и диапазона дат
        /// </summary>
        /// <param name="curRow"></param>
        /// <param name="sortedPromoes"></param>
        /// <param name="sheet"></param>
        /// <param name="colToDateMap"></param>
        /// <param name="wb"></param>
        /// <returns></returns>
        private int SplitPromoByRows(int curRow, List<Promo> sortedPromoes, ISheet sheet, Dictionary<int, DateTime> colToDateMap, ref IWorkbook wb) {
            do {    // Перебираем Промо для Клиента
                IRow row = sheet.CreateRow(curRow);
                Promo promo = sortedPromoes.FirstOrDefault();
                // Записываем в строку все промо, которые можно(по дате и приоритету), потом переходим на следующую
                while (promo != null) {
                    // Промо отсортированы по приоритету и Дате, берём первое Промо
                    sortedPromoes.Remove(promo);
                    // Определение диапазона колонок, которые занимает промо
                    int startCol = colToDateMap.FirstOrDefault(x => promo.StartDate.Value.DateTime >= x.Value && promo.StartDate.Value.DateTime < x.Value.AddDays(7)).Key;
                    int endCol = colToDateMap.FirstOrDefault(x => promo.EndDate.Value.DateTime >= x.Value && promo.EndDate.Value.DateTime <= x.Value.AddDays(7)).Key;

                    ICell cell = row.CreateCell(startCol);
                    cell.SetCellValue(new XSSFRichTextString(String.IsNullOrEmpty(promo.Name) ? String.Empty : promo.Name));

                    bool hasColor = promo.ColorId.HasValue;
                    IFont promoFont = wb.CreateFont();
                    promoFont.Color = IndexedColors.Black.Index;
                    promoFont.FontHeightInPoints = 11;
                    promoFont.FontName = "Calibri";
                    promoFont.Boldweight = (short) NPOI.SS.UserModel.FontBoldWeight.Bold;
                    // Цвет по умолчанию - серый
                    short fillColor = NPOI.HSSF.Util.HSSFColor.Grey50Percent.Index;

                    XSSFCellStyle colorStyle = GetPromoCellStyle(ref wb, fillColor, promoFont);

                    // Если у Промо есть цвет, устанавливаем его и определяем цвет шрифта в зависимости от яркости
                    if (hasColor) {
                        System.Drawing.Color promoColor = System.Drawing.ColorTranslator.FromHtml(promo.Color.SystemName);
                        float brightness = promoColor.GetBrightness();
                        promoFont.Color = brightness > 0.5 ? IndexedColors.Black.Index : IndexedColors.White.Index;
                        XSSFColor cellColor = new XSSFColor(promoColor);
                        colorStyle.SetFillForegroundColor(cellColor);
                    }
                    cell.CellStyle = colorStyle;
                    // Объединяем ячейки, добавляем рамку (при мерже ячеек не подтягивается стиль рамки первой)
                    if (startCol != endCol) {
                        CellRangeAddress range = new CellRangeAddress(curRow, curRow, startCol, endCol);
                        RegionUtil.SetBorderTop(1, range, sheet, wb);
                        RegionUtil.SetBorderLeft(1, range, sheet, wb);
                        RegionUtil.SetBorderRight(1, range, sheet, wb);
                        RegionUtil.SetBorderBottom(1, range, sheet, wb);
                        sheet.AddMergedRegion(range);
                    }
                    // Если есть подходящее промо - записывается в эту же строку, если нет, переход на новую строку
                    promo = FindClosestPromoInRow(promo, sortedPromoes);
                }
                curRow++;
            } while (sortedPromoes.Count > 0);
            return curRow;
        }

        /// <summary>
        /// Поиск ближаешего промо, которое можно записать в эту же строку
        /// </summary>
        /// <param name="promo"></param>
        /// <param name="sortedPromoes"></param>
        /// <returns></returns>
        private Promo FindClosestPromoInRow(Promo promo, List<Promo> sortedPromoes) {
            int compareDiff = Int32.MaxValue;
            Promo closerPromo = null;
            // Находим следующее промо, дата начала у которого ближайшая к дате окончания текущего
            for (int i = 0, d = sortedPromoes.Count; i < d; i++) {
                int dateDiff = (sortedPromoes[i].StartDate - promo.EndDate).Value.Days;
                MarsDate endDate = new MarsDate(promo.EndDate.Value);
                MarsDate startDate = new MarsDate(sortedPromoes[i].StartDate.Value);
                bool isEqualMarsWeek = endDate.WeekStartDate() == startDate.WeekStartDate();
                if (dateDiff >= 0 && dateDiff < compareDiff && !isEqualMarsWeek) {
                    if (dateDiff >= 0 || sortedPromoes[i].CalendarPriority <= promo.CalendarPriority) { // sort verticaly by Priority
                        closerPromo = sortedPromoes[i];
                        compareDiff = dateDiff;
                    }
                }
            }
            return closerPromo;
        }

        /// <summary>
        /// Запись заголовка
        /// </summary>
        /// <param name="startdDate"></param>
        /// <param name="endDate"></param>
        /// <param name="style"></param>
        /// <param name="wb"></param>
        /// <param name="colToDateMap"></param>
        private void WriteHeader(DateTime startdDate, DateTime endDate, ICellStyle style, ref IWorkbook wb, out Dictionary<int, DateTime> colToDateMap) {
            colToDateMap = new Dictionary<int, DateTime>();
            MarsDate startMarsDate = new MarsDate(startdDate);
            MarsDate endMarsDate = new MarsDate(endDate);
            endDate = endMarsDate.WeekEndDate().DateTime;
            MarsDate curMarsDate = new MarsDate(startMarsDate.WeekStartDate());
            // Получение списка Период - Неделя - Дата Начала недели
            List<Tuple<int, int, int, DateTime>> weeks = GetWeeks(curMarsDate, endDate);
            ISheet sheet1 = wb.GetSheet("Schedule");

            // Создание строк заголовка, первых ячеек.
            IRow row0 = sheet1.CreateRow(0);
            IRow row1 = sheet1.CreateRow(1);
            IRow row2 = sheet1.CreateRow(2);
            ICell cell0 = row0.CreateCell(0);
            ICell cell1 = row1.CreateCell(0);
            ICell cell2 = row2.CreateCell(0);
            cell0.SetCellValue("P");
            cell1.SetCellValue("W");
            cell2.SetCellValue("Dates");
            short rowHeight = 15 * 20;
            row0.Height = rowHeight;
            row1.Height = rowHeight;
            row2.Height = rowHeight;

            //Группировка по Периодам
            var weeksByPeriod = weeks.GroupBy(w => new { w.Item1, w.Item2 });
            int curCell = 1;
            // Для каждого периода создаём колонку для каждой недели, запоняем заголовок
            foreach (var period in weeksByPeriod) {
                //workaround из-за того, что недель может быть 5
                int cellsToMerge = period.Count();
                ICell cell = row0.CreateCell(curCell);
                cell.SetCellValue(new XSSFRichTextString(String.Format("P{0}", period.Key.Item2.ToString())));
                int endColumn = curCell + cellsToMerge - 1;
                if (curCell != endColumn) {
                    sheet1.AddMergedRegion(new CellRangeAddress(0, 0, curCell, endColumn));
                }
                cell.CellStyle = style;
                foreach (Tuple<int, int, int, DateTime> week in period) {
                    ICell weekCell = row1.CreateCell(curCell);
                    weekCell.SetCellValue(new XSSFRichTextString(String.Format("W{0}", week.Item3.ToString())));
                    weekCell.CellStyle = style;

                    ICell dateCell = row2.CreateCell(curCell);
                    dateCell.SetCellValue(week.Item4.ToString("dd.MM.yyyy"));
                    dateCell.CellStyle = style;

                    colToDateMap.Add(curCell, week.Item4);
                    curCell++;
                }
            }
            // Фиксация заголовка
            sheet1.CreateFreezePane(1, 3);
            cell0.CellStyle = cell1.CellStyle = cell2.CellStyle = style;
        }
        /// <summary>
        /// Получение последовательности Период, Неделя, Дата начала недели для записи заголовка
        /// </summary>
        /// <param name="curMarsDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<Tuple<int, int, int, DateTime>> GetWeeks(MarsDate curMarsDate, DateTime endDate) {
            List<Tuple<int, int, int, DateTime>> weeks = new List<Tuple<int, int, int, DateTime>>();
            while (curMarsDate.StartDate() <= endDate) {
                weeks.Add(new Tuple<int, int, int, DateTime>(curMarsDate.Year, curMarsDate.Period, curMarsDate.Week, curMarsDate.StartDate().DateTime));
                curMarsDate = curMarsDate.AddWeeks(1);
            }
            weeks.Distinct();
            return weeks;
        }

        /// <summary>
        /// Создание стиля ячеек - все границы чёрные medium, выравнивание по центру
        /// </summary>
        /// <param name="wb"></param>
        /// <returns></returns>
        private ICellStyle GetCellStyle(ref IWorkbook wb) {
            ICellStyle style = wb.CreateCellStyle();
            style.BorderBottom = BorderStyle.Medium;
            style.BottomBorderColor = IndexedColors.Black.Index;
            style.BorderTop = BorderStyle.Medium;
            style.TopBorderColor = IndexedColors.Black.Index;
            style.BorderLeft = BorderStyle.Medium;
            style.LeftBorderColor = IndexedColors.Black.Index;
            style.BorderRight = BorderStyle.Medium;
            style.RightBorderColor = IndexedColors.Black.Index;
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            return style;
        }

        /// <summary>
        /// Стиль ячеек Промо с заливкой цветом, тонкой рамкой и применением шрифта
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="fillColor"></param>
        /// <param name="promoFont"></param>
        /// <returns></returns>
        private XSSFCellStyle GetPromoCellStyle(ref IWorkbook wb, short fillColor, IFont promoFont) {
            XSSFCellStyle colorStyle = (XSSFCellStyle) wb.CreateCellStyle();
            colorStyle.FillForegroundColor = fillColor;
            colorStyle.FillPattern = FillPattern.SolidForeground;
            colorStyle.SetFont(promoFont);
            colorStyle.VerticalAlignment = VerticalAlignment.Center;
            colorStyle.Alignment = HorizontalAlignment.Center;
            colorStyle.BorderBottom = BorderStyle.Thin;
            colorStyle.BottomBorderColor = IndexedColors.Black.Index;
            colorStyle.BorderTop = BorderStyle.Thin;
            colorStyle.TopBorderColor = IndexedColors.Black.Index;
            colorStyle.BorderLeft = BorderStyle.Thin;
            colorStyle.LeftBorderColor = IndexedColors.Black.Index;
            colorStyle.BorderRight = BorderStyle.Thin;
            colorStyle.RightBorderColor = IndexedColors.Black.Index;
            return colorStyle;
        }

        protected string GetUserName(string userName) {
            string[] userParts = userName.Split(new char[] { '/', '\\' });
            return userParts[userParts.Length - 1];
        }
    }
}
