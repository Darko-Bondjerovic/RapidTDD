using System;
using System.Collections.Generic;
using System.Linq;

public class Frame : List<int>
{
    int num;

    public Action<int> WhenAddPin = (p) => { };

    public Frame(int num)
    {
        this.num = num;
    }

    public override string ToString()
    {
        return $"{num}.[{string.Join(",", this)}]";
    }

    public void AddPin(int pin)
    {
        if (this.Count == 3)
            return;

        this.Add(pin);
        WhenAddPin(pin);
    }

    public bool IsDone()
    {
        return (this.Count == 2 || IsStrike()) && (this.num != 10);
    }

    public void AddToPrevious(int pin)
    {
        if (IsSpare() || IsStrike())
            this.AddPin(pin);
    }

    public Frame MakeNext()
    {
        var frame = new Frame(num + 1);
        frame.WhenAddPin = AddToPrevious;
        return frame;
    }

    public bool IsSpare()
    {
        return this.Take(2).Sum() == 10;
    }

    public bool IsStrike()
    {
        return this.Take(1).Sum() == 10;
    }
}