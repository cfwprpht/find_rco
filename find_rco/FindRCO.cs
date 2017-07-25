using System;
using System.IO;

namespace find_rco {
    static class StringExtension {
        /// <summary>
        /// Trims the path out of a name. Name can be last folder in string or file.
        /// </summary>
        /// <param name="source">The path string to trim.</param>
        /// <returns>The name without any back slashes or path in it.</returns>
        public static string GetName(this string source) {
            string[] splitted = source.Split('\\');
            return splitted[splitted.Length - 1];
        }
    }

    class FindRCO {                
        /// <summary>
        /// A StreamWriter to write out the path of the founed files.
        /// </summary>
        private static StreamWriter writer;

        /// <summary>
        /// Counter to determine total amount of founded rcos.
        /// </summary>
        private static int counter = 0;

        /// <summary>
        /// Show the Usage.
        /// </summary>
        private static void ShowUsage() {
            Console.WriteLine("simple rco finder\nby cfwprpht\n\nUsage: find_rco.exe <folder>");
            Environment.Exit(0);
        }

        /// <summary>
        /// Check Aegument.
        /// </summary>
        /// <param name="args"></param>
        private static void CheckArgs(string[] args) { if (args.Length > 1 || args.Length < 1) ShowUsage(); }

        /// <summary>
        /// Search for Files recursive.
        /// </summary>
        /// <param name="_path">The path where to search for.</param>
        /// <param name="path">The path where to copy to.</param>
        /// <param name="writer">The StreamWriter to write into the output text file.</param>
        private static void SearchRecursive(string _path, string path, StreamWriter writer) {
            foreach (string found in Directory.GetDirectories(_path)) {
                foreach (string rco in Directory.GetFiles(found, "*.rco")) {
                    counter++;
                    writer.WriteLine(rco);
                    if (!File.Exists(path + rco.GetName())) File.Copy(rco, path + rco.GetName());
                    else {
                        int count = 0;
                        while (true) {
                            if (!File.Exists(path + rco.GetName() + "_" + count.ToString())) {
                                File.Copy(rco, path + rco.GetName() + "_" + count.ToString());
                                break;
                            } else count++;
                        }
                    }
                }
                SearchRecursive(found, path, writer);
            }            
        }

        /// <summary>
        /// Main entry.
        /// </summary>
        /// <param name="args">The path to search for.</param>
        static void Main(string[] args) {
            CheckArgs(args);
            if (!Directory.Exists(args[0])) {
                Console.WriteLine("Can not find or access the given directory !\nMay try to run this app from the same disk then the folder to search for.");
                Environment.Exit(0);
            }

            string path = Directory.GetCurrentDirectory() + @"\found\";
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);

            string foundText = path + "found.txt";
            File.Create(foundText).Close();

            writer = new StreamWriter(foundText);
            SearchRecursive(args[0], path, writer);
            writer.Close();
            
            Console.WriteLine("simple rco finder\nby cfwprpht\n\n");
            Console.WriteLine("Done!\nFound " + counter.ToString() + " RCOs within the given folder !\nTHX for using my Tool\nTill next time ! :)");
            Environment.Exit(0);
        }
    }
}
