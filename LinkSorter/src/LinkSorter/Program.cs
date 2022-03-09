// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The main program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LinkSorter;

/// <summary>
/// The main program.
/// </summary>
public static class Program
{
    /// <summary>
    /// The main method.
    /// </summary>
    public static async Task Main(string[] args)
    {
        string? folder = null;

        // Check arguments.
        if (args.Length > 0)
        {
            folder = args[0];
        }

        // Check folder for null value.
        if (folder is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No folder specified, stopping.");
            Console.ReadLine();
            return;
        }

        // Check directory exists.
        if (!Directory.Exists(folder))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The specified folder is invalid.");
            Console.ReadLine();
            return;
        }

        var files = Directory.GetFiles(folder);

        // Iterate all files.
        for (var i = files.Length; i-- > 0;)
        {
            var file = files[i];
            Console.WriteLine($"Sorting links in file {file}.");

            var lines = await File.ReadAllLinesAsync(file);
            var headerLines = new List<string>();
            var sortLines = new List<string>();

            // Iterate all lines.
            foreach (var line in lines)
            {
                // Skip the header and so on.
                if (!line.StartsWith("|["))
                {
                    headerLines.Add(line);
                    continue;
                }

                sortLines.Add(line);
            }

            // Sort the other lines.
            sortLines = sortLines.OrderBy(s => s.Split("]")[0]).ToList();

            // Write the header lines and sorted lines to the file, overwrite it.
            await File.WriteAllLinesAsync(file, headerLines);
            await File.WriteAllLinesAsync(file, sortLines);
        }

        // Service is finished.
        Console.WriteLine("All files sorted.");
        Console.ReadLine();
    }
}
