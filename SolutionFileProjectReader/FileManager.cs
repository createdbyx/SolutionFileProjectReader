using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace SolutionFileProjectReader
{
    public class FileManager
    {
        public static List<string> GetFiles(string sourcePath, string pattern)
        {
            return pattern.Split('|').SelectMany(filter => Directory.GetFiles(sourcePath, filter, SearchOption.AllDirectories)).ToList();
        }

        public static string GetContents(string file)
        {
            try
            { 
                var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch
            {
                return string.Empty;
            }

        }
    }
}
