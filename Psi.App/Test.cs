using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Psi.App
{
	public class Test : LogProvider
	{
		public Test(int? n)
			: base(n)
		{
		}

		public override void ReadLine(ArraySegment<byte> seg)
		{
			string line = Encoding.ASCII.GetString(seg.Array, seg.Offset, seg.Count);
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
				Console.WriteLine("{0}:{1} {2}", CurrentFile.Name, CurrentLineNumber, line);
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				Console.WriteLine();
			}
		}
	}
}

