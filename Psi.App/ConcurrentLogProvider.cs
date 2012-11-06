using System;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;

namespace Psi.App
{
	abstract public class ConcurrentLogProvider : LogProvider
	{
		public ConcurrentLogProvider(int? n)
			: base(n)
		{
		}

		protected ConcurrentQueue<Tuple<FileInfo, ArraySegment<byte>>> files = new ConcurrentQueue<Tuple<FileInfo, ArraySegment<byte>>>();

		protected virtual void ReadFile(RagelParser parser, Tuple<FileInfo, byte[]> data)
		{
			ReadFile(parser, data.Item1, new ArraySegment<byte>(data.Item2));
		}

		protected virtual void ReadFile(RagelParser parser, Tuple<FileInfo, ArraySegment<byte>> data)
		{
			ReadFile(parser, data.Item1, data.Item2);
		}

		protected virtual void ReadFile(RagelParser parser, FileInfo fi, ArraySegment<byte> file)
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

		protected void ParallelDirectoryRead()
		{
			foreach (FileInfo fi in Files) {
				ThreadPool.QueueUserWorkItem((obj) => {
					var fileinfo = obj as FileInfo;
					var file = File.OpenRead(fileinfo.FullName);
					byte[] data = new byte[fileinfo.Length];
					file.CopyTo(new MemoryStream(data));
					file.Close();
					files.Enqueue(Tuple.Create(fileinfo, new ArraySegment<byte>(data)));
				}, fi);
			}
		}

		protected void LinearDirectoryRead()
		{
			foreach (FileInfo fi in Files) {
				Size += fi.Length;
			}

			byte[] data = new byte[Size];

			var ms = new MemoryStream(data) { Position = 0 };
			int i = 0;
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
				files.Enqueue(Tuple.Create(fi, new ArraySegment<byte>(data, start, end - start)));
			}
		}

		protected void ParallelWork(int n)
		{
			int i = 0;
			int m = (Count.HasValue ? Count.Value : Files.Length);
			ThreadStart work = () => {
				var p = new RagelParser();
				p.AddEvent((_) => { });
				while (i < m) {
					Tuple<FileInfo, ArraySegment<byte>> tmp;
					if (files.TryDequeue(out tmp)) {
						ReadFile(p, tmp);
						i++;
					}
				}
			};

			Thread[] threads = new Thread[n];
			for (int j = 0; j < threads.Length; j++) {
				threads[j] = new Thread(work);
				threads[j].Start();
			}

			foreach (var thread in threads) {
				thread.Join();
			}
		}
	}
}

