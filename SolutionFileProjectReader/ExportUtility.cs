using System;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;

using OfficeOpenXml;

namespace SolutionFileProjectReader
{
    public class ExportUtility
    {
        private const string ExcelExe = "excel.exe";

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public static DataTable GetReferenceTable()
        {
            // Create table.
            DataTable table = new DataTable();

            // Add columns.
            table.Columns.Add("SNo", typeof(int));
            table.Columns.Add("ProjectPath", typeof(string));
            table.Columns.Add("SolutionFile", typeof(string));
            table.Columns.Add("ProjectFile", typeof(string));
            table.Columns.Add("ProjectFilePath", typeof(string));

            return table;
        }

        public static void ExportTable(DataTable labelTable, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Construct the excel package with header and contents.
            using (ExcelPackage pck = new ExcelPackage())
            {
                // Create labels sheet & load table.
                ExcelWorksheet labelSheet = pck.Workbook.Worksheets.Add("References");
                labelSheet.Cells["A1"].LoadFromDataTable(labelTable, true);

                // Save the file.
                FileInfo fileInfo = new FileInfo(fileName);
                pck.SaveAs(fileInfo);

                ConsoleUtility.WriteInfo($"Exported to file '{fileInfo.Name}'");

                try
                {
                    // Open the file in the default editor.
                    using (Process proc = new Process())
                    {
                        proc.StartInfo.FileName = ExcelExe;
                        proc.StartInfo.Arguments = fileName;
                        proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                        proc.Start();

                        SetForegroundWindow(proc.MainWindowHandle);
                    }
                }
                catch (Exception exp)
                {
                    ConsoleUtility.WriteError(exp.Message);
                    ConsoleUtility.WriteInfo("Error in opening the file. Please open it manually");
                }
            }
        }
    }
}
