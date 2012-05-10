using System;
using System.IO;

namespace Psi.App
{
	public class Test : LogProvider
	{
		public override void ReadLine(string line)
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
				Console.WriteLine("{0}:{1} {2}", CurrentFile.Name, CurrentLineNumber, line);
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				Console.WriteLine();
			}
		}
	}
}

