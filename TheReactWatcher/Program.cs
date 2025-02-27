using System;
using System.IO;

class Program
{
    static void Main()
    {
        string watchDirectory = Directory.GetCurrentDirectory(); // Watches the folder where the app runs
        Console.WriteLine($"Watching for changes in: {watchDirectory}\n");

        using (FileSystemWatcher watcher = new FileSystemWatcher())
        {
            watcher.Path = watchDirectory;
            watcher.IncludeSubdirectories = true;
            watcher.Filter = "*.*"; // Monitor all files
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Event handlers
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnRenamed;

            // Start watching
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press 'q' to exit.\n");
            while (Console.ReadKey().Key != ConsoleKey.Q) { }
        }
    }

    private static void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (IsValidFileType(e.FullPath))
        {
            Console.WriteLine($"[{DateTime.Now:T}] {e.ChangeType}: {e.FullPath}");
        }
    }

    private static void OnRenamed(object sender, RenamedEventArgs e)
    {
        if (IsValidFileType(e.FullPath))
        {
            Console.WriteLine($"[{DateTime.Now:T}] RENAMED: {e.OldFullPath} -> {e.FullPath}");
        }
    }

    private static bool IsValidFileType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        return extension == ".js" || extension == ".jsx" || extension == ".css";
    }
}