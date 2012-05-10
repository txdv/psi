using System;
using System.IO;

namespace Psi.App
{
	public abstract class LogProvider
	{
		public int? CurrentLineNumber { get; protected set; }
		public FileInfo CurrentFile { get; protected set; }
		public FileInfo[] Files { get; protected set; }
		public int CurrentFileNumber { get; protected set; }
		public int TotalFiles { get; protected set; }

		public virtual void ReadDirectory(string directory)
		{
			DirectoryInfo di = new DirectoryInfo(directory);

			Files = di.GetFiles();
			TotalFiles = Files.Length;

			foreach (FileInfo fi in Files) {
				CurrentFileNumber++;
				ReadFile(fi);
			}

			CurrentLineNumber = null;
		}

		public virtual void ReadFile(FileInfo fi)
		{
			CurrentFile = fi;
			StreamReader sr = null;
			try {
				sr = new StreamReader(File.Open(fi.FullName, FileMode.Open));
				CurrentLineNumber = 0;
				while (!sr.EndOfStream) {
					CurrentLineNumber++;
					ReadLine(sr.ReadLine());
				}
			} finally {
				if (sr != null) {
					sr.Close();
				}
			}
		}

		public abstract void ReadLine(string line);

		public virtual void Run(string directory)
		{
			ReadDirectory(directory);
		}
	}
}

