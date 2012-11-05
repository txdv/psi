using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Psi;

static class EncodingEtensions
{
	public static string GetString(this Encoding enc, ArraySegment<byte> segment)
	{
		if (segment == default(ArraySegment<byte>)) {
			return null;
		}
		return enc.GetString(segment.Array, segment.Offset, segment.Count);
	}
}

class Util
{
	static string[] sizes = { "B", "KB", "MB", "GB" };

	public static string Readable(int size)
	{
		int order = 0;
		while (size >= 1024 && order + 1 < size) {
			order++;
			size = size/1024;
		}
		return string.Format("{0:0.##} {1}", size, sizes[order]);
	}
}

class Benchmark
{
	public static void Print(Encoding enc, ArraySegment<byte> arr)
	{
		Console.Write(enc.GetString(arr.Array, arr.Offset, arr.Count));
	}

	public static void Print(ArraySegment<byte> arr)
	{
		Print(Encoding.ASCII, arr);
	}

	public static void Puts(Encoding enc, ArraySegment<byte> arr)
	{
		Console.WriteLine(enc.GetString(arr.Array, arr.Offset, arr.Count));
	}

	public static void Puts(ArraySegment<byte> arr)
	{
		Puts(Encoding.ASCII, arr);
	}
	public Benchmark(int maxSize)
	{
		Parser = new RawParser();
		MaxSize = maxSize;
	}

	public RawParser Parser { get; protected set; }

	Queue<ArraySegment<byte>> queue = new Queue<ArraySegment<byte>>();
	Queue<FileInfo> fqueue = new Queue<FileInfo>();

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

		Console.WriteLine("Inspecting files");

		foreach (var file in files) {
			var fi = new FileInfo(file);
			Size += (int)fi.Length;
			fqueue.Enqueue(fi);
			if (Done) {
				break;
			}
		}

		Console.WriteLine("Fetched {0} in {1} files", Util.Readable(Size), fqueue.Count);

		Console.WriteLine("Allocating memory");

		var data = new byte[Size];
		var ms = new MemoryStream(data);

		Console.WriteLine("Reading into memory");

		while (fqueue.Count > 0) {
			var fi = fqueue.Dequeue();
			var f = File.OpenRead(fi.FullName);
			f.CopyTo(ms);
			f.Close();
		}

		Console.WriteLine("Slicing the lines");

		int last = 0;
		for (int i = 0; i < Size; i++) {
			if (data[i] == '\n') {
				queue.Enqueue(new ArraySegment<byte>(data, last, i - last));
				i++;
				last = i;
			}
		}

		Console.WriteLine("Fetched {0} messages with the total size of {1} in {2}",
			queue.Count, Util.Readable(Size), DateTime.Now - now);

		Console.WriteLine("Benchmarking parser");

		now = DateTime.Now;
		while (queue.Count > 0) {
			var d = queue.Dequeue();
			Parser.Execute(d);
		}
		Console.WriteLine("Messages parsed in {0}", DateTime.Now - now);
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
				Console.WriteLine("{0}/{1}", Util.Readable(Size), Util.Readable(MaxSize));
			}
			i++;
			i %= 10000;
		}
		file.Close();
	}

	public static void Main(string[] args)
	{
		var b = new Benchmark(int.Parse(args[1]));
		var p = b.Parser;
		b.Parse(args[0]);
	}
}
