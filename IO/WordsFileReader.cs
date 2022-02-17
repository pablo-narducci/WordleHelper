using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WordleHelper.Solver;

namespace WordleHelper.IO
{
    public static class WordsFileReader
    {
        public static List<Word> ReadWords(string filename,  bool writeSortedList = false)
        {
            try
            {
                var lines = filename != null ? File.ReadAllLines(filename) : StaticList.Get();
                if (writeSortedList)
                {
                    WriteToOutput(lines);
                }

                return lines.Select(x => new Word(x)).ToList();
            }
            catch
            {
                throw new Exception($"Error trying to read file {filename}");
            }
        }

        private static void WriteToOutput(string[] lines)
        {
            lines = lines.OrderBy(x => x).ToArray();
            using (var writer = File.CreateText("output.txt"))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}
