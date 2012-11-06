using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Psi.App
{
	public class MultiCpuBenchmark
	{
		public int? CurrentLineNumber { get; protected set; }
		public FileInfo CurrentFile { get; protected set; }
		public FileInfo[] Files { get; protected set; }
		public int CurrentFileNumber { get; protected set; }

		public int? Count { get; protected set; }

		public long Size { get; protected set; }

		public MultiCpuBenchmark(int? count)
		{
			Count = count;
		}

		ConcurrentQueue<ArraySegment<byte>> files = new ConcurrentQueue<ArraySegment<byte>>();
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

			i = 0;
			int count = 0;
			int m = (Count.HasValue ? Count.Value : Files.Length);
			ThreadStart work = () => {
				var p = new RagelParser();
				p.AddEvent((_) => { count++; }); 
				ArraySegment<byte> file;
				while (i < m) {
					if (files.TryDequeue(out file)) {
						int start = file.Offset;
						int end = file.Offset + file.Count;
						int pos = start;
						for (int j = start; j < end; j++) {
							if (data[j] == '\n') {
								p.Parse(new ArraySegment<byte>(data, pos, j - pos));
								j++;
								pos = j;
							}
						}
						i++;
					};
				}
			};

			Begin();
			Thread[] threads = new Thread[2];
			for (int j = 0; j < threads.Length; j++) {
				threads[j] = new Thread(work);
				threads[j].Start();
			}

			foreach (var thread in threads) {
				thread.Join();
			}
			End();
			Console.WriteLine(TimeSpan);
		}

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

