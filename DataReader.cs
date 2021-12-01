using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AdventOfCode21
{
    internal class DataReader
    {
        public IEnumerable<string> Read(string dayNo)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", dayNo + ".txt");

            return File.ReadAllLines(path);
        }
    }
}
