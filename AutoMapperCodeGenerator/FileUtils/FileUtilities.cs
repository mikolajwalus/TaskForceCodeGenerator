using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutoMapperCodeGenerator.FileUtils
{
    public static class FileUtilities
    {
        public static List<string> GetAllClassPropertyNames(string className, string filePath)
        {
            return 
                GetAllProperties(className, filePath)
                .Select(x => x.Split(" ").TakeLast(5).First())
                .ToList();
        }

        public static List<string> GetAllProperties(string className, string filePath)
        {
            var fullPath = Path.Combine(filePath, className + ".cs");

            if (!File.Exists(fullPath))
                throw new ArgumentException($"File doesn't exists {fullPath}");

            var lines = File.ReadAllLines(fullPath);

            return lines
                .Select(x => x.Trim())
                //Filter non props and non public
                .Where(x => x.StartsWith("public"))
                //Filter ctors
                .Where(x => !x.Contains("()"))
                //skip class declaration
                .Skip(1)
                .ToList();
        }

        public static void SaveToFile(string fileName, string text, string passedPath = null)
        {
            if (passedPath is null)
                passedPath = Directory.GetCurrentDirectory();
            var path = Path.Combine(passedPath, fileName + ".cs");

            File.WriteAllText(path, text);
        }

        public static void SaveToFileWithPath(string fileName, string path, string text)
        {
            var fullPath = Path.Combine(path, fileName, ".cs");

            File.WriteAllText(fullPath, text);
        }
    }
}
