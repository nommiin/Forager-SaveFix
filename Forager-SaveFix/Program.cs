using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
using System.IO;
using System.Threading;

namespace Forager_SaveFix {
    class Program {
        static string BaseDirectory = Environment.GetEnvironmentVariable("LocalAppData") + "/Forager/";
        static void Main(string[] args) {
            if (SteamAPI.Init() == true) {
                Console.Clear();
                Console.WriteLine("SteamAPI has been initalized");
                if (Directory.Exists(BaseDirectory) == false) {
                    Console.WriteLine("ERROR: Forager directory does not exist!\nPress any key to exit");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                Console.WriteLine("WARNING: This program will delete any Forager save files you have, are you sure you want to continue? [Y/N]");
                if (Console.ReadKey(true).Key == ConsoleKey.Y) {
                    Console.Write("Disabling Steam Cloud... ");
                    SteamRemoteStorage.SetCloudEnabledForApp(false);
                    Console.WriteLine("DONE");
                    Console.Write("Creating backup folder... ");
                    if (Directory.Exists(BaseDirectory + "backup") == true) {
                        Directory.Delete(BaseDirectory + "backup", true);
                    }
                    Directory.CreateDirectory(BaseDirectory + "backup");
                    Console.WriteLine("DONE");
                    foreach (string FileFind in Directory.GetFiles(BaseDirectory, "*.txt")) {
                        if (Path.GetFileNameWithoutExtension(FileFind) == "config") continue;
                        Console.Write("Copying \"{0}\"->\"backup/{0}\"... ", Path.GetFileName(FileFind));
                        File.Copy(FileFind, BaseDirectory + "backup//" + Path.GetFileName(FileFind));
                        Console.WriteLine("DONE");
                        Console.Write("Deleting \"{0}\"... ", Path.GetFileName(FileFind));
                        File.Delete(FileFind);
                        Console.WriteLine("DONE");
                    }
                    Console.Write("Cleaning thumbnails... ");
                    foreach (string FileFind in Directory.GetFiles(BaseDirectory, "*.png")) {
                        File.Copy(FileFind, BaseDirectory + "backup//" + Path.GetFileName(FileFind));
                        File.Delete(FileFind);
                    }
                    Console.WriteLine("DONE");
                }
                Console.WriteLine("Process completed");
            } else Console.WriteLine("SteamAPI failed to initalize");
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
        }
    }
}
