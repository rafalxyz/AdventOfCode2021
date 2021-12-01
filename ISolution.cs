using System.Collections.Generic;

namespace AdventOfCode21
{
    internal interface ISolution
    {
        int GetFirstAnswer(IEnumerable<string> data);

        int GetSecondAnswer(IEnumerable<string> data);
    }
}
