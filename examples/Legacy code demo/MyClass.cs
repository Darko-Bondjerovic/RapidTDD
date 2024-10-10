using System;
using System.Text;
using System.Collections.Generic;

public class MyClass
{
    private StringBuilder str = null;
    
    public MyClass(int count)
    {
        str = new StringBuilder();

        for (int i = 1; i < 2 * count; i++)
        {
            var m = 0;
            if (i < count)
                m = i;
            else
                m = (2 * count) - i;

            var s = new StringBuilder();
            for (int k = 0; k < m; k++)
                s.Append(m);

            str.AppendLine(s.ToString());
        }
    }
}