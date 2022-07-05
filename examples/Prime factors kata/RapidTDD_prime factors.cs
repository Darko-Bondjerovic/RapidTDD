using System;
using System.Collections.Generic;

public class Program
{
    static public void Main(string[] args)
    {
        Print("Prime factors kata");
        
        //https://github.com/Darko-Bondjerovic/RapidTDD

        RunTest(1, ""); //pass
        RunTest(2, "2"); // if (num > 1) list.Add(2);
        RunTest(3, "3"); // if (num > 1) list.Add(3);
                          //if (num % 2 == 0)
        RunTest(4, "2,2"); // if (num % 2 == 0)
        RunTest(6, "2,3"); // pass
        RunTest(8, "2,2,2"); // if (num % 2 == 0)
        RunTest(9, "3,3"); // if (num % 3 == 0)
                          
    }
    
    static public void RunTest(int num, string exp)
    {
        var list = new List<int>();
        
        // refactor
        // convert if-s into while => while (num % 2 == 0) 
        // convert if-s into while => while (num % 3 == 0)
        // add div variable  var div = 2;
        // convert if-s into while => while (num > 1)
        
        // convert while into for loop
        
        // Done! :) 
        
        for (var div = 2;num > 1;div++)
            for (;num % div == 0;num /= div)
               list.Add(div);
        
        Assert(exp, string.Join(",", list));
    }

    static void Assert(string exp, string act)
    {
        if (act.Equals(exp))
            Print($"PASS [{exp}]==[{act}]");
        else
            Print($"FAIL [{exp}]!=[{act}]");
    }

    static void Print(string str = "")
    {
        Console.WriteLine(str);
    }

    
}