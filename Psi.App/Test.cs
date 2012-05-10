using System;
using System.IO;

namespace Psi.App
{
	public class Test
	{
		private static int currentLineNumber = -1;
		private static FileInfo currentFile = null;

		public static void ReadDirectory(string directory)
		{
			DateTime dt = DateTime.Now;
			DirectoryInfo di = new DirectoryInfo(directory);

			foreach (FileInfo fi in di.GetFiles()) {
				ReadFile(fi);
			}

			currentLineNumber = -1;

			Console.WriteLine("Total time: {0}s", (DateTime.Now - dt).TotalSeconds);
		}

		public static void ReadFile(FileInfo fi)
		{
			currentFile = fi;
			StreamReader sr = null;
			try {
				sr = new StreamReader(File.Open(fi.FullName, FileMode.Open));
				currentLineNumber = 0;
				while (!sr.EndOfStream) {
					currentLineNumber++;
					ReadLine(sr.ReadLine());
				}
			} finally {
				if (sr != null) {
					sr.Close();
				}
			}
		}

		public static void ReadLine(string line)
		{
			try {
				LogEvent log = MainClass.Parser.UnsafeParse(line);
				if (log != null) {
					if (log.Log != line) {
						Console.WriteLine("match failure!");
						Console.WriteLine(log.Log);
						Console.WriteLine(line);
						Console.WriteLine();
					}
				}
			} catch (Exception e) {
				Console.WriteLine("{0}:{1} {2}", currentFile.Name, currentLineNumber, line);
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				Console.WriteLine();
			}
		}

		public static void Run(string lines)
		{
			ReadDirectory(lines);
		}
	}
}

