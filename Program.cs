using System;
using System.IO;
using System.IO.Compression;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string pt = "C:\\Users\\User\\Desktop";
        string filePath = FindFile(pt, "test.txt");

        if (!string.IsNullOrEmpty(filePath))
        {
            Console.WriteLine($"Файл найден: {filePath}");

            string fileContent = ReadFile(filePath);
            Console.WriteLine($"Содержимое файла: \n{fileContent}");

            string compressedFilePath = CompressFile(filePath);
            Console.WriteLine($"Сжатый файл сохранен как: {compressedFilePath}");
        }
        else
        {
            Console.WriteLine("Файл не найден.");
        }
    }

    static string FindFile(string rootPath, string fileName)
    {
        if (File.Exists(Path.Combine(rootPath, fileName)))
        {
            return Path.Combine(rootPath, fileName);
        }

        foreach (var directory in Directory.GetDirectories(rootPath))
        {
            try
            {
                string foundFile = FindFile(directory, fileName);
                if (!string.IsNullOrEmpty(foundFile))
                {
                    return foundFile;
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                //Console.WriteLine(ex.Message);
            }
        }

        return null;
    }

    static string ReadFile(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }

    static string CompressFile(string filePath)
    {
        string compressedFilePath = filePath + ".gz";

        using (FileStream originalFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (FileStream compressedFileStream = new FileStream(compressedFilePath, FileMode.Create))
            {
                using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                {
                    originalFileStream.CopyTo(compressionStream);
                }
            }
        }

        return compressedFilePath;
    }
}
