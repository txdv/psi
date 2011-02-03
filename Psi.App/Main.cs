/*
  This file is part of Psi.

  Copyright (C) 2011 Andrius Bentkus

  Psi is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  Foobar is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with Psi.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.IO;
using Psi;

namespace Psi.App
{
  public class Benchmark
  {
    private static List<double> times = new List<double>();
    private static int currentLineNumber = -1;

    public static void ReadDirectory(string directory)
    {
      DateTime dt = DateTime.Now;
      DirectoryInfo di = new DirectoryInfo(directory);

      foreach (FileInfo fi in di.GetFiles()) {
        ReadFile(fi);
      }

      currentLineNumber = -1;

      double duration = (DateTime.Now - dt).TotalSeconds;
      times.Add(duration);
      Console.WriteLine("Total time: {0}s", duration);
    }

    public static void ReadFile(FileInfo fi)
    {
      //DateTime dt;
      StreamReader sr = null;
      try {
        sr = new StreamReader(File.Open(fi.FullName, FileMode.Open));
        currentLineNumber = 0;
        //dt = DateTime.Now;
        while (!sr.EndOfStream) {
          currentLineNumber++;
          ReadLine(sr.ReadLine());
        }
        //TimeSpan ts = (DateTime.Now - dt);
        //Console.Write(fi.FullName);
        //Console.WriteLine(" [{0}]\t[{1}ms]", fi.Length, ts.TotalMilliseconds);
      } finally {
        if (sr != null) sr.Close();
      }
    }

    public static void ReadLine(string line)
    {
      try {
        Psi.Parser.UnsafeParse(line);
      } catch { }
    }

    public static void Run(string dir, int times)
    {
      for (int i = 0; i < times; i++) {
        ReadDirectory(dir);
      }
    }
  }

  public class Test
  {
    private static int currentLineNumber = -1;
    private static FileInfo currentFile = null;

    public static void ReadDirectory(string directory)
    {
      DateTime dt = DateTime.Now;
      DirectoryInfo di = new DirectoryInfo(directory);

      foreach (FileInfo fi in di.GetFiles()) {
        ReadFile(fi);
      }

      currentLineNumber = -1;

      Console.WriteLine("Total time: {0}s", (DateTime.Now - dt).TotalSeconds);
    }

    public static void ReadFile(FileInfo fi)
    {
      currentFile = fi;
      StreamReader sr = null;
      try {
        sr = new StreamReader(File.Open(fi.FullName, FileMode.Open));
        currentLineNumber = 0;
        while (!sr.EndOfStream) {
          currentLineNumber++;
          ReadLine(sr.ReadLine());
        }
      } finally {
        if (sr != null) sr.Close();
      }
    }

    public static void ReadLine(string line)
    {
      try {
        Base log = Psi.Parser.UnsafeParse(line);
        if (log != null) {
          if (log.Log != line) {
            Console.WriteLine("match failure!");
            Console.WriteLine(log.Log);
            Console.WriteLine(line);
            Console.WriteLine();
          }
        }
      } catch (Exception e) {
        Console.WriteLine("{0}:{1} {2}", currentFile.Name, currentLineNumber, line);
        Console.WriteLine(e.Message);
        Console.WriteLine(e.StackTrace);
        Console.WriteLine();
      }
    }

    public static void Run(string lines)
    {
      ReadDirectory(lines);
    }
  }

  public class MainClass
  {
    public static void Main(string[] args)
    {
      if (args.Length < 2)
        return;

      switch (args[0]) {
      case "test":
        Test.Run(args[1]);
        break;
      case "bench":
        Benchmark.Run(args[1], 5);
        break;
      }

    }
  }
}
