using System.Text.RegularExpressions;

namespace Codefarts.CodeInspection;

public class SolutionReader
{
    public static List<ProjectEntry> Read(string file)
    {
        var projectPath = Path.GetDirectoryName(file);
        var entries = new List<ProjectEntry>();

        var basePath = Path.GetDirectoryName(file);
        var Content = File.ReadAllText(file);
        var projReg = new Regex("Project\\(\"\\{[\\w-]*\\}\"\\) = \"([\\w _]*.*)\", \"(.*\\.(cs|vcx|vb)proj)\"", RegexOptions.Compiled);
        var matches = projReg.Matches(Content).Cast<Match>();
        var projects = matches.Select(x => x.Groups[2].Value).ToList();
        foreach (var project in projects)
        {
            var projectFile = Path.GetFileName(project);
            var projectFilePath = $"{basePath}\\{project}";

            var entry = new ProjectEntry();
            entry.ProjectPath = new DirectoryInfo(projectPath).Name;
            entry.SolutionFile = new FileInfo(file).Name;
            entry.ProjectFile = projectFile;
            entry.ProjectFilePath = projectFilePath;

            entries.Add(entry);
        }

        return entries;
    }
}