using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

static class EncodingEtensions
{
	public static string GetString(this Encoding enc, ArraySegment<byte> segment)
	{
		return enc.GetString(segment.Array, segment.Offset, segment.Count);
	}
}

class Benchmark
{
	public static void Print(Encoding enc, ArraySegment<byte> arr)
	{
		Console.WriteLine(enc.GetString(arr.Array, arr.Offset, arr.Count));
	}

	public static void Print(ArraySegment<byte> arr)
	{
		Print(Encoding.ASCII, arr);
	}

	public Benchmark(int maxSize)
	{
		Parser = new Parser();
		MaxSize = maxSize;
	}

	public Parser Parser { get; protected set; }

	Queue<ArraySegment<byte>> queue = new Queue<ArraySegment<byte>>();

	public int Size { get; protected set; }
	public int MaxSize { get; protected set; }

	public bool Done {
		get {
			return Size >= MaxSize;
		}
	}

	public void Parse(string dir)
	{
		var now = DateTime.Now;
		var files = Directory.GetFiles(dir, "*.log");

		foreach (var file in files) {
			if (Done) {
				break;
			}
			Parse(File.OpenRead(file));
		}
		Console.WriteLine(DateTime.Now - now);
		Test();
	}

	int i = 0;

	public void Parse(FileStream file)
	{
		var sr = new StreamReader(file);
		string line = null;
		while ((line = sr.ReadLine()) != null) {
			if (Done) {
				break;
			}
			var bytes = Encoding.ASCII.GetBytes(line);
			queue.Enqueue(new ArraySegment<byte>(bytes));
			Size += bytes.Length;
			if (i == 0) {
				Console.WriteLine("{0}/{1}", Size, MaxSize);
			}
			i++;
			i %= 10000;
		}
		file.Close();
	}

	void Test()
	{
		Console.WriteLine(queue.Count);
		var now = DateTime.Now;
		while (queue.Count > 0) {
			var data = queue.Dequeue();
			Parser.Execute(data);
		}
		Console.WriteLine(DateTime.Now - now);
	}

	public static void Main(string[] args)
	{
		var b = new Benchmark(int.Parse(args[1]));
		var p = b.Parser;
		b.Parse(args[0]);
	}
}
