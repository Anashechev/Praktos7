using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            Console.WriteLine("Выберите диск:");
            for (int i = 0; i < allDrives.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {allDrives[i].Name} ({allDrives[i].TotalSize / (1024 * 1024 * 1024)} GB)");
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            int selectedIndex = keyInfo.KeyChar - '1';

            if (selectedIndex < 0 || selectedIndex >= allDrives.Length)
            {
                Console.WriteLine("Invalid selection. Try again.");
                continue;
            }

            ExploreDirectory(allDrives[selectedIndex].RootDirectory.FullName);
        }
    }

    static void ExploreDirectory(string directoryPath)
    {
        DirectoryInfo directory = new DirectoryInfo(directoryPath);
        FileSystemInfo[] filesAndDirectories = directory.GetFileSystemInfos();

        while (true)
        {
            Console.WriteLine($"Current directory: {directoryPath}");
            for (int i = 0; i < filesAndDirectories.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {filesAndDirectories[i].Name} ({filesAndDirectories[i].CreationTime})");
            }

            Console.WriteLine("Options:");
            Console.WriteLine("a. Добавить файл");
            Console.WriteLine("b. Добавить дерикторию");
            Console.WriteLine("c. Удалить файл");
            Console.WriteLine("d. Удалить дерикторию");

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                return;
            }

            switch (keyInfo.KeyChar)
            {
                case 'a':
                    Console.Write("Enter file name: ");
                    string fileName = Console.ReadLine();
                    File.Create(Path.Combine(directoryPath, fileName)).Close();
                    break;
                case 'b':
                    Console.Write("Enter directory name: ");
                    string directoryName = Console.ReadLine();
                    Directory.CreateDirectory(Path.Combine(directoryPath, directoryName));
                    break;
                case 'c':
                    Console.Write("Enter file name to delete: ");
                    string deleteFileName = Console.ReadLine();
                    File.Delete(Path.Combine(directoryPath, deleteFileName));
                    break;
                case 'd':
                    Console.Write("Enter directory name to delete: ");
                    string deleteDirectoryName = Console.ReadLine();
                    Directory.Delete(Path.Combine(directoryPath, deleteDirectoryName), true);
                    break;
                default:
                    int selectedIndex = keyInfo.KeyChar - '1';

                    if (selectedIndex < 0 || selectedIndex >= filesAndDirectories.Length)
                    {
                        Console.WriteLine("Invalid selection. Try again.");
                        continue;
                    }

                    if (filesAndDirectories[selectedIndex] is DirectoryInfo selectedDirectory)
                    {
                        ExploreDirectory(selectedDirectory.FullName);
                    }
                    else if (filesAndDirectories[selectedIndex] is FileInfo selectedFile)
                    {
                        Process.Start(new ProcessStartInfo(selectedFile.FullName) { UseShellExecute = true });
                    }
                    break;
            }

            // Refresh the list of files and directories
            filesAndDirectories = directory.GetFileSystemInfos();
        }
    }
}