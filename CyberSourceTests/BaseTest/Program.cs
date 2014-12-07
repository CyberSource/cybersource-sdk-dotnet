using CyberSource.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            RemoveIndices(null);
            RemoveIndices("");
            RemoveIndices("0_");
            RemoveIndices("_0");
            RemoveIndices("0_0");
            RemoveIndices("_0_");
            RemoveIndices("0_Ever");
            RemoveIndices("01_Ever");
            RemoveIndices("012_Ever");
            RemoveIndices("Ever_2_Aragon");
            RemoveIndices("Ever_23_Aragon");
            RemoveIndices("Ever_234_Aragon");
            RemoveIndices("Olano_5");
            RemoveIndices("Olano_56");
            RemoveIndices("Olano_567");
            RemoveIndices("Olano_890_");
            RemoveIndices("1_Ever_2_Aragon_3_Olano_4");
            RemoveIndices("12_Ever_34_Aragon_56_Olano_78");
            RemoveIndices("123_Ever_456_Aragon_789_Olano_456");
            RemoveIndices("Ever_Aragon_Olano");
            RemoveIndices("EverAragonOlano");
            RemoveIndices("Olano123_");
        }

        private static void RemoveIndices(string input)
        {
            Console.WriteLine(
                "Before: *" + input + "*\tAfter: *" +
                SafeFields.RemoveIndices(input) + "*");
        }
    }
}
