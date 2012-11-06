using System;
using System.Text;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;

namespace Psi.App
{
	abstract public class ParallelLogProvider : LogProvider
	{
		public ParallelLogProvider(int? n)
			: base(n)
		{
		}

		ConcurrentQueue<Tuple<FileInfo, byte[]>> files = new ConcurrentQueue<Tuple<FileInfo, byte[]>>();

		public void ReadFile(RagelParser parser, Tuple<FileInfo, byte[]> data)
		{
			ReadFile(parser, data.Item1, new ArraySegment<byte>(data.Item2));
		}

		public void ReadFile(RagelParser parser, FileInfo fi, ArraySegment<byte> file)
		{
			int start = file.Offset;
			int end = file.Offset + file.Count;
			int pos = start;
			byte[] data = file.Array;
			for (int j = start; j < end; j++) {
				if (data[j] == '\n') {
					ReadLine(parser, new ArraySegment<byte>(data, pos, j - pos));
					j++;
					pos = j;
				}
			}
		}

		public abstract void ReadLine(RagelParser parser, ArraySegment<byte> line);

		public override void Run(string directory)
		{
			DirectoryInfo di = new DirectoryInfo(directory);

			Files = di.GetFiles();
			if (Count.HasValue) {
				Files = Files.Take(Count.Value).ToArray();
			}

			Begin();

			foreach (FileInfo fi in Files) {
				ThreadPool.QueueUserWorkItem((obj) => {
					var fileinfo = obj as FileInfo;
					var file = File.OpenRead(fileinfo.FullName);
					byte[] data = new byte[fileinfo.Length];
					file.CopyTo(new MemoryStream(data));
					file.Close();
					files.Enqueue(Tuple.Create(fileinfo, data));
				}, fi);
			}

			int i = 0;
			int m = (Count.HasValue ? Count.Value : Files.Length);
			ThreadStart work = () => {
				var p = new RagelParser();
				while (i < m) {
					Tuple<FileInfo, byte[]> tmp;
					if (files.TryDequeue(out tmp)) {
						ReadFile(p, tmp);
						i++;
					}
				}
			};

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
	}
}

