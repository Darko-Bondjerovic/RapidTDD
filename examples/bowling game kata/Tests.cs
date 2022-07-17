using System;
using System.Collections.Generic;
using System.Linq;

public class Tests
{

    List<Frame> frames = new List<Frame>();
    Frame frame = new Frame(1);

    public void Execute()
    {
        frames.Add(frame);

        RunTest(1, 4);
        RunTest(4, 5);
        RunTest(6, 4);
        RunTest(5);
        RunTest(5);
        RunTest(10);
        RunTest(0,1);
        RunTest(7,3);
        RunTest(6,4);
        RunTest(10);
        RunTest(2,8,6);
        
        Print("[TEST] perfect game");
        
        frames = new List<Frame>();
        frame = new Frame(1);
        frames.Add(frame);
        
        for(int i=1; i<13; i++)
            AddPinToGame(10);
        
        PrintScore();

    }

    public void RunTest(params int[] pins)
    {
        Print("[TEST]test.");

        foreach (var pin in pins)
        {
            AddPinToGame(pin);
        }

        PrintScore();
    }

    public void PrintScore()
    {
        var score = 0;
        foreach (var f in frames)
        {
            score += f.Sum();
            Print($"{f}={score}");
        }
    }

    static void Print(string str = "")
    {
        Console.WriteLine(str);
    }

    public void AddPinToGame(int pin)
    {
        frame.AddPin(pin);
            
        if (frame.IsDone())
        {
            frame = frame.MakeNext();
            frames.Add(frame);
        }
    }
}