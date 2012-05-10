using System;
using System.IO;
using System.Collections.Generic;

namespace Psi.App
{
	public class Benchmark
	{
		private static List<double> times = new List<double>();
		private static int currentLineNumber = -1;

		public static void ReadDirectory(string directory)
		{
			DateTime dt = DateTime.Now;
			DirectoryInfo di = new DirectoryInfo(directory);

			foreach (FileInfo fi in di.GetFiles()) {
				ReadFile(fi);
			}

			currentLineNumber = -1;

			double duration = (DateTime.Now - dt).TotalSeconds;
			times.Add(duration);
			Console.WriteLine("Total time: {0}s", duration);
		}

		public static void ReadFile(FileInfo fi)
		{
			//DateTime dt;
			StreamReader sr = null;
			try {
				sr = new StreamReader(File.Open(fi.FullName, FileMode.Open));
				currentLineNumber = 0;
				//dt = DateTime.Now;
				while (!sr.EndOfStream) {
					currentLineNumber++;
					ReadLine(sr.ReadLine());
				}
				//TimeSpan ts = (DateTime.Now - dt);
				//Console.Write(fi.FullName);
				//Console.WriteLine(" [{0}]\t[{1}ms]", fi.Length, ts.TotalMilliseconds);
			} finally {
				if (sr != null) {
					sr.Close();
				}
			}
		}

		public static void ReadLine(string line)
		{
			try {
				MainClass.Parser.UnsafeParse(line);
			} catch {
			}
		}

		public static void Run(string dir, int times)
		{
			for (int i = 0; i < times; i++) {
				ReadDirectory(dir);
			}
		}
	}
}

