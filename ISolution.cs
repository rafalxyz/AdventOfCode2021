using System.Collections.Generic;

namespace AdventOfCode21
{
    internal interface ISolution
    {
        long GetFirstAnswer(IEnumerable<string> data);

        long GetSecondAnswer(IEnumerable<string> data);
    }
}
