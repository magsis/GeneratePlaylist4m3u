using System;
using System.IO;

namespace GeneratePlaylist4m3u {
    class Program {
        static string[] serchDirectories;

        static void Main(string[] args) {
            Console.Write("Input Playlist Directory => ");
            string targetPlaylistDirectory = Console.ReadLine();

            Console.Write("Output Playlist Directory => ");
            string outputPlaylistDirectory = Console.ReadLine();

            Console.Write("Serch Target Dirctories (split \",\") =>");
            serchDirectories = Console.ReadLine().Split(",");

            try {
                GeneratePlaylist(outputPlaylistDirectory, Directory.GetFiles(targetPlaylistDirectory));
            } catch (Exception e) {
                Console.WriteLine(e);
            }
            Console.WriteLine("finished!");
            Console.ReadKey();
        }

        static void GeneratePlaylist(string outputPlaylistDirectory, string[] fileNames) {
            foreach (string playlistFileName in fileNames) {
                string outputString = GenerateOutputString(playlistFileName);
                string outputPath = outputPlaylistDirectory + @"\" + Path.GetFileName(playlistFileName);
                try {
                    File.WriteAllText(outputPath, outputString, System.Text.Encoding.UTF8);
                } catch(Exception e) {
                    Console.WriteLine(e);
                }
                Console.WriteLine("WRITE FINISHIED => " + outputPath);
            }
        }

        static string GenerateOutputString(string playlistFileName) {
            string outputString = string.Empty;
            try {
                string[] lines = File.ReadAllLines(playlistFileName, System.Text.Encoding.UTF8);
                foreach (string line in lines) {
                    outputString += SerchFilePath(line) + System.Environment.NewLine;
                }
            } catch(Exception e) {
                Console.WriteLine(e);
            }
            return outputString;
            
        }

        static string SerchFilePath(string line) {
            if (line.StartsWith(@"#EXT")) return line;
            foreach (string dir in serchDirectories) {
                string filepath = dir + @"\" + line;
                if (File.Exists(filepath)) {
                    return filepath;
                }
            }
            foreach (string dir in serchDirectories) {
                foreach (string path in Directory.GetFileSystemEntries(dir, Path.GetFileName(line), SearchOption.AllDirectories)) {
                    return path;
                }
            }
            Console.WriteLine("!!!NOT FOUND!!! => " + line);
            return string.Empty;
        }
    }
}
