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

		public LogProvider(int? count)
		{
			Count = count;
		}

		Queue<Queue<ArraySegment<byte>>> queue = new Queue<Queue<ArraySegment<byte>>>();
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

			long size = 0;

			foreach (FileInfo fi in Files) {
				size += fi.Length;
			}

			data = new byte[size];

			var ms = new MemoryStream(data) { Position = 0 };

			Queue<ArraySegment<byte>> file = new Queue<ArraySegment<byte>>();

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
				int pos = start;
				for (int j = start; j < end; j++) {
					if (data[j] == '\n') {
						file.Enqueue(new ArraySegment<byte>(data, pos, j - pos));
						j++;
						pos = j;
					}
				}
				f.Close();
			}

			queue.Enqueue(file);

			Console.WriteLine("Running benchmark");
			Begin();
			while (queue.Count > 0) {
				var lines = queue.Dequeue();
				while (lines.Count > 0) {
					ReadLine(lines.Dequeue());
				}
			}
			End();

			CurrentLineNumber = null;
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
			Console.WriteLine("Done in: {0}", TimeSpan);
		}
	}
}

