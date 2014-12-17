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
            RemoveIndices("0_Dev");
            RemoveIndices("01_Dev");
            RemoveIndices("012_Dev");
            RemoveIndices("Dev_2_CyberSource");
            RemoveIndices("Dev_23_CyberSource");
            RemoveIndices("Dev_234_CyberSource");
            RemoveIndices("Developer_5");
            RemoveIndices("Developer_56");
            RemoveIndices("Developer_567");
            RemoveIndices("Developer_890_");
            RemoveIndices("1_Dev_2_CyberSource_3_Developer_4");
            RemoveIndices("12_Dev_34_CyberSource_56_Developer_78");
            RemoveIndices("123_Dev_456_CyberSource_789_Developer_456");
            RemoveIndices("Dev_CyberSource_dontnet");
            RemoveIndices("CyberSourceDeveloper");
            RemoveIndices("Developer123_");
        }

        private static void RemoveIndices(string input)
        {
            Console.WriteLine(
                "Before: *" + input + "*\tAfter: *" +
                SafeFields.RemoveIndices(input) + "*");
        }
    }
}
