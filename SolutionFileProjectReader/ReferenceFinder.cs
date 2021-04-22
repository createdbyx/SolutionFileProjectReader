using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SolutionFileProjectReader
{
    public class ReferenceFinder
    {
        public static void Run()
        {
            try
            {
                ConsoleUtility.WriteInfo("-- Started running project finder tool --", true);

                string solutionFileSearchPattern = ConfigurationManager.AppSettings["SolutionFileSearchPattern"];
                GeneralUtility.ThrowIfInvalid(solutionFileSearchPattern, "Solution file search pattern");

                bool exportFullFileName = Convert.ToBoolean(ConfigurationManager.AppSettings["ExportFullFileName"]);

                DataTable referenceTable = ExportUtility.GetReferenceTable();

                // Process all project base paths.
                List<string> projectBasePaths = ConfigUtil.GetSectionValues("ProjectBasePath");

                if (projectBasePaths == null || !projectBasePaths.Any())
                {
                    ConsoleUtility.WriteWarning("No project base paths found to process");
                    return;
                }

                int tableRowNo = 1;

                foreach (string projectPath in projectBasePaths)
                {
                    string projectPathFolderName = new DirectoryInfo(projectPath).Name;
                    List<string> files = FileManager.GetFiles(projectPath, solutionFileSearchPattern);

                    ConsoleUtility.WriteInfo($"Processing project path '{projectPathFolderName}'");

                    if (files == null || !files.Any())
                    {
                        ConsoleUtility.WriteWarning($"No files found to process for project path '{projectPathFolderName}'");
                        continue;
                    }

                    ConsoleUtility.WriteInfo($"{files.Count} files found for project path '{projectPathFolderName}'");
                    int cursorTop = Console.CursorTop;
                    int fileIndex = 0;
                    
                    foreach (string file in files)
                    {
                        fileIndex++;

                        ConsoleUtility.ClearLine(cursorTop);
                        ConsoleUtility.GoToLine(cursorTop);
                        ConsoleUtility.WriteStep($"Processing file {fileIndex} of {files.Count} - {Path.GetFileName(file)} of project path '{projectPathFolderName}'");

                        // Delay.
                        GeneralUtility.ProcessDelay();

                        TryConstructProjects(projectPath, file, ref tableRowNo, referenceTable);

                        //TryConstructDotNetCoreReferences(projectPath, file, ref tableRowNo, referenceTable);
                    }

                    // Clear the processing line.
                    ConsoleUtility.ClearLine(cursorTop);
                    ConsoleUtility.GoToLine(cursorTop);
                
                    ConsoleUtility.WriteInfo($"Completed processing {files.Count} files for project path '{projectPathFolderName}'");
                    ConsoleUtility.NewLine();
                }

                if (referenceTable.Rows.Count > 0)
                {
                    ConsoleUtility.WriteInfo($"{referenceTable.Rows.Count} rows identified to export");

                    string fileName = $"{Guid.NewGuid()}.xlsx";
                    ExportUtility.ExportTable(referenceTable, fileName);
                }

                ConsoleUtility.NewLine();
                ConsoleUtility.WriteInfo("-- Reference finder tool ran successfully --");
            }
            catch (Exception exp)
            {
                ConsoleUtility.WriteError(exp.Message);
            }
        }

        /// <summary>
        /// Construct project references.
        /// </summary>
        private static void TryConstructProjects(string projectPath, string file, ref int tableRowNo, DataTable referenceTable)
        {
            try
            {
                var basePath = Path.GetDirectoryName(file);
                var Content = File.ReadAllText(file);
                Regex projReg = new Regex("Project\\(\"\\{[\\w-]*\\}\"\\) = \"([\\w _]*.*)\", \"(.*\\.(cs|vcx|vb)proj)\"", RegexOptions.Compiled);
                var matches = projReg.Matches(Content).Cast<Match>();
                var projects = matches.Select(x => x.Groups[2].Value).ToList();
                for (int i = 0; i < projects.Count; ++i)
                {
                    string projectFile = Path.GetFileName(projects[i]);
                    string projectFilePath = $"{basePath}\\{projects[i]}";
                    ConstructRow(projectPath, file, ref tableRowNo, projectFile, projectFilePath, referenceTable);
                }
            }
            catch (Exception exp)
            {
                ConsoleUtility.WriteError(exp.Message);
            }
        }

        private static void ConstructRow(string projectPath, string solutionFile, ref int tableRowNo, 
            string projectFile, string projectFilePath, DataTable referenceTable)
        {
            // Add the row to the reference table.
            DataRow referenceTableRow = referenceTable.NewRow();
            referenceTableRow["SNo"] = tableRowNo.ToString();
            referenceTableRow["ProjectPath"] = new DirectoryInfo(projectPath).Name;
            referenceTableRow["SolutionFile"] = new FileInfo(solutionFile).Name;
            referenceTableRow["ProjectFile"] = projectFile;
            referenceTableRow["ProjectFilePath"] = projectFilePath;
            referenceTable.Rows.Add(referenceTableRow);

            tableRowNo++;
        }
    }
}
