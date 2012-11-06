using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Psi.App
{
	public abstract class LogProvider
	{
		public int? CurrentLineNumber { get; protected set; }
		public FileInfo CurrentFile { get; protected set; }
		public FileInfo[] Files { get; protected set; }
		public int CurrentFileNumber { get; protected set; }

		public int? Count { get; protected set; }

		public long Size { get; protected set; }

		public LogProvider(int? count)
		{
			Count = count;
		}

		Queue<ArraySegment<byte>> files = new Queue<ArraySegment<byte>>();
		byte[] data;

		public int TotalFiles { get; protected set; }

		public virtual void ReadDirectory(string directory)
		{
			int i;
			DirectoryInfo di = new DirectoryInfo(directory);

			Files = di.GetFiles();
			if (Count.HasValue) {
				Files = Files.Take(Count.Value).ToArray();
			}
			TotalFiles = Files.Length;

			foreach (FileInfo fi in Files) {
				Size += fi.Length;
			}

			data = new byte[Size];

			var ms = new MemoryStream(data) { Position = 0 };

			Console.WriteLine(Util.Readable(Size));

			i = 0;
			Console.WriteLine();
			foreach (FileInfo fi in Files) {
				Console.CursorTop--;
				Console.WriteLine("Fetching {0}/{1}", i + 1, Files.Length);
				i++;
				var f = File.OpenRead(fi.FullName);
				int start = (int)ms.Position;
				f.CopyTo(ms);
				int end = (int)ms.Position;
				f.Close();
				files.Enqueue(new ArraySegment<byte>(data, start, end - start));
			}


			Console.WriteLine("Running benchmark");
			Begin();
			while (files.Count > 0) {
				ReadFile(files.Dequeue());
			}
			End();

			CurrentLineNumber = null;
		}

		public virtual void ReadFile(ArraySegment<byte> file)
		{
			int start = file.Offset;
			int end = file.Offset + file.Count;
			int pos = start;
			for (int j = start; j < end; j++) {
				if (data[j] == '\n') {
					ReadLine(new ArraySegment<byte>(data, pos, j - pos));
					j++;
					pos = j;
				}
			}
		}

		public abstract void ReadLine(ArraySegment<byte> line);

		public virtual void Run(string directory)
		{
			ReadDirectory(directory);
		}

		public DateTime StartTime { get; protected set; }
		public DateTime EndTime { get; protected set; }
		public TimeSpan TimeSpan { get; protected set; }

		public virtual void Begin()
		{
			StartTime = DateTime.Now;
		}

		public virtual void End()
		{
			EndTime = DateTime.Now;
			TimeSpan = EndTime - StartTime;
		}
	}
}

