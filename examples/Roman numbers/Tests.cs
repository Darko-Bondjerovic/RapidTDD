using System;
using System.Collections.Generic;
using System.Linq;

public class Tests
{
    public void Execute()
    {
        RunTest(100, "C");
        RunTest(200, "CC");
        RunTest(240, "CCXL");
        RunTest(245, "CCXLV");
        RunTest(247, "CCXLVII");
        RunTest(3479, "MMMCDLXXIX");
    }

    static void Print(string str = "")
    {
        Console.WriteLine(str);
    }

    Dictionary<int, string> dict = new Dictionary<int, string>()
    {
        {1000, "M"}, {900, "CM"}, {500, "D"}, {400, "CD"},
        {100,  "C"}, {90,  "XC"}, {50,  "L"}, {40,  "XL"},
        {10,   "X"}, {9,   "IX"}, {5,   "V"}, {4,   "IV"},   
        {1,    "I"},   
    };
    
    public void RunTest(int num, string exp)
    {
        Print($"[TEST]test {num}:{exp}");
        
        var result = "";
        
        foreach(var value in dict.Keys)
        {
            while (num >= value)
            {
                num -= value;
                result += dict[value];
            }
        }
        
        Print(result);
        
        Print($"[EXPC]{exp}");
    }
}