using System;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        new Tests().Execute();
    }
}

public class Tests
{
    public void Execute()
    {
//        var lines = File.ReadAllLines(@"c:\Users\bondjasan\Documents\Book1.csv");
//
//        foreach (var line in lines)
//        {
//            var data = line.Split(';');
//            var num = Int16.Parse(data[0]);
//
//            RunTest(num, data[1]);
//        }

            for (int i =1; i<=100; i++)
                RunTest(i);
    }

    static void Print(string str = "")
    {
        Console.WriteLine(str);
    }

    public void RunTest(int num)//, string exp)
    {
        Print($"[TEST] test {num}");
        
        var result = "";
        
        if (num % 3 == 0)
            result = "Fizz";
        
        if (num % 5 == 0)
            result += "Buzz";
        
        if (result == "")
            result = num.ToString();
        
        Print(result);
        
        
        //Print($"[EXPC]{exp}");
        
    }
}










