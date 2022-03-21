namespace Codefarts.CodeInspection;

public class ProjectEntry
{
    public ProjectEntry()
    {
    }

    public ProjectEntry(string projectPath, string solutionFile, string projectFile, string projectFilePath)
    {
        this.ProjectPath = projectPath;
        this.SolutionFile = solutionFile;
        this.ProjectFile = projectFile;
        this.ProjectFilePath = projectFilePath;
    }

    public string ProjectPath { get; set; }
    public string SolutionFile { get; set; }
    public string ProjectFile { get; set; }
    public string ProjectFilePath { get; set; }
}