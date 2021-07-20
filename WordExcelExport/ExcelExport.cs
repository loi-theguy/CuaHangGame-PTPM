using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using BLDAL;

namespace WordExcelExport
{
    public class ExcelExport
    {
        #region ---- Constructors ----

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelExport"/> class.
        /// </summary>
        public ExcelExport()
        {

        }

        #endregion

        #region ---- Export Excel Template ----

        #region ---- Constants ----
        // Tuyen Dung
        public const string T_KeHoachTuyenDung = "KeHoachTD";
        // Khai báo trùng với tên đặt trong file cần điền dữ liệu ra
        public const string T_BieuMau = "BieuMau";
        public const string T_DanhMucKhoa = "DanhMucKhoa";
        #endregion

        // Utility                        
        private const string TMP_SHEET = "TMP";
        private const string TMP_ROW = "[TMP]";
        #endregion

        /// <summary>
        /// Replace the specified value in work sheet.
        /// </summary>
        /// <param name="workSheet">The work sheet.</param>
        /// <param name="findValue">The find value.</param>
        /// <param name="replaceValue">The replace value.</param>
        private static void Replace(IWorksheet workSheet, string findValue, string replaceValue)
        {
            // Find and replace
            if (workSheet != null && !string.IsNullOrEmpty(findValue))
            {
                // Get current cells
                IRange[] cells = workSheet.Range.Cells;
                IRange range = null;

                // Loop cells to replace
                for (int i = 0; i < cells.Count(); i++)
                {
                    // Current cell
                    range = cells[i];

                    // Find and replace values
                    if (range != null && range.DisplayText.Contains(findValue))
                    {
                        range.Text = range.Text.Replace(findValue, replaceValue);
                    }
                }
            }
        }

        /// <summary>
        /// Prints the excel.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public static void PrintExcel(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook wb = null;

            try
            {
                wb = excelApp.Workbooks.Open(fileName);

                if (wb != null)
                {
                    // Show print preview
                    excelApp.Visible = true;
                    wb.PrintPreview(true);
                }
            }
            catch (Exception ex)
            {
                //ShowMessage
            }
            finally
            {
                // Cleanup:
                GC.Collect();
                GC.WaitForPendingFinalizers();

                wb.Close(false, Type.Missing, Type.Missing);
                Marshal.FinalReleaseComObject(wb);

                excelApp.Quit();
                Marshal.FinalReleaseComObject(excelApp);
            }
        }

        /// <summary>
        /// Builds the replacer current date.
        /// </summary>
        /// <param name="pReplacer">The p replacer.</param>
        private void BuildCurrentDateReplacer(ref Dictionary<string, string> pReplacer)
        {
            if (pReplacer != null)
            {
                DateTime currentDate = DateTime.Now;
                string ngay = "Ngày " + currentDate.Day + " tháng " + currentDate.Month + " năm " + currentDate.Year;
                pReplacer.Add("%NgayThangNam", ngay);
            }
        }

        /// <summary>
        /// Outs the simple report.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource">The data source.</param>
        /// <param name="replaceValues">The replace values.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="isPrintPreview">if set to <c>true</c> [is print preview].</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private bool OutSimpleReport<T>(List<T> dataSource, Dictionary<string, string> replaceValues,
            string fullPathTemplateFileName, bool isPrintPreview, ref string outputFileName)
        {
            string file = string.Empty;
            bool result = false;

            // Get template stream
            MemoryStream stream = GetTemplateStream(fullPathTemplateFileName);

            // Check if data is null
            if (stream == null)
            {
                return false;
            }

            // Create excel engine
            ExcelEngine engine = new ExcelEngine();
            IWorkbook workBook = engine.Excel.Workbooks.Open(stream);

            IWorksheet workSheet = workBook.Worksheets[0];
            ITemplateMarkersProcessor markProcessor = workSheet.CreateTemplateMarkersProcessor();

            // Replace value
            if (replaceValues != null && replaceValues.Count > 0)
            {
                // Find and replace values
                foreach (KeyValuePair<string, string> replacer in replaceValues)
                {
                    Replace(workSheet, replacer.Key, replacer.Value);
                }
            }

            // Fill variables
            markProcessor.AddVariable("Var", dataSource);
            markProcessor.ApplyMarkers(UnknownVariableAction.ReplaceBlank);

            // Delete temporary row
            IRange range = workSheet.FindFirst(TMP_ROW, ExcelFindType.Text);

            // Delete
            if (range != null)
            {
                workSheet.DeleteRow(range.Row);
            }

            file = Path.GetTempFileName() + Constants.FILE_EXT_XLS;

            outputFileName = file;

            // Output file
            if (!FileCommon.IsFileOpenOrReadOnly(file))
            {
                workBook.SaveAs(file);
                result = true;
            }

            // Close
            workBook.Close();
            engine.Dispose();

            // Print preview
            if (result && isPrintPreview)
            {
                PrintExcel(file);
                File.Delete(file);
            }

            return result;
        }
        /// <summary>
        /// Gets the template stream.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns></returns>
        private MemoryStream GetTemplateStream(string fullPathTemplateFileName)
        {
            MemoryStream stream = null;
            byte[] arrByte = new byte[0];

            //Create Temp Folder if it does not exist
            if (!Directory.Exists(Global.AppPath + Constants.FOLDER_TEMPLATES))
            {
                Directory.CreateDirectory(Global.AppPath + Constants.FOLDER_TEMPLATES);
            }
            arrByte = File.ReadAllBytes(fullPathTemplateFileName).ToArray();
            // Get stream
            if (arrByte.Count() > 0)
            {
                stream = new MemoryStream(arrByte);
            }
            return stream;
        }

        public void BuildGameReportReplacer(ref Dictionary<string, string> replacer, string tongSoLuong, string tongThanhTien, string tenNguoiLap)
        {
            if (replacer != null)
            {
                replacer.Add("%TongSoLuong", tongSoLuong);
                replacer.Add("%TongThanhTien", tongThanhTien);
                replacer.Add("%TenNguoiLap", tenNguoiLap);
            }
        }

        public void BuildNhanVienReportReplacer(ref Dictionary<string, string> replacer, string tongSoLuong, string tongLuongThang,string tenNguoiLap)
        {
            if (replacer != null)
            {
                replacer.Add("%TongSoLuong", tongSoLuong);
                replacer.Add("%TongLuongThang", tongLuongThang);
                replacer.Add("%TenNguoiLap", tenNguoiLap);

            }
        }
        public bool ThongKeGame(List<GameReportResult> dataSource, ref string fileName, bool isPrintPreview, string tenNguoiLap)
        {
            // Check if data is null
            if (dataSource == null || (dataSource != null && dataSource.Count == 0))
            {
                return false;
            }

            int totalCount = 0;
            double totalRevenue = 0;
            for (int i = 0; i < dataSource.Count; i++)
            {
                dataSource[i].STT = (i+1).ToString();
                totalCount += int.Parse(dataSource[i].SoLuong);
                totalRevenue += double.Parse(dataSource[i].ThanhTien);
            }
            // Create replacer
            Dictionary<string, string> replacer = new Dictionary<string, string>();

            BuildCurrentDateReplacer(ref replacer);
            BuildGameReportReplacer(ref replacer, totalCount.ToString(),totalRevenue.ToString(),tenNguoiLap);
            return OutSimpleReport(dataSource, replacer, "Templates/ThongKeGame.xls", isPrintPreview, ref fileName);
        }
        public bool ThongKeNhanVien(List<NhanVienReportInfo> dataSource, ref string fileName, bool isPrintPreview, string tenNguoiLap)
        {
            // Check if data is null
            if (dataSource == null || (dataSource != null && dataSource.Count == 0))
            {
                return false;
            }

            double totalPayment = 0;
            for (int i = 0; i < dataSource.Count; i++)
            {
                totalPayment += double.Parse(dataSource[i].LuongThang);
            }
            // Create replacer
            Dictionary<string, string> replacer = new Dictionary<string, string>();

            BuildCurrentDateReplacer(ref replacer);
            BuildNhanVienReportReplacer(ref replacer,dataSource.Count.ToString(), totalPayment.ToString(), tenNguoiLap);
            return OutSimpleReport(dataSource, replacer, "Templates/ThongKeNhanVien.xls", isPrintPreview, ref fileName);
        }
    }
}
