using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TK.BaseLib.Time;

namespace BaseLib_Tester
{
    static class Test_TimeCode
    {
        public static void Execute()
        {
            TimeCode tc = new TimeCode(25, 400);

            Console.WriteLine(tc.ToLongString() + " " + tc.ToString());
            while (true)
            {
                tc = ReadOperation(tc);
            }
        }

        private static TimeCode ReadOperation(TimeCode inTc)
        {
            string op = Console.ReadLine();
            int nbFrames = 0;
            int.TryParse(op.Substring(1), out nbFrames);

            if (op == "=")
            {
                Console.WriteLine(inTc.TotalFrames.ToString() + " frames.");
            }
            else
            {
                if (op.StartsWith("+"))
                {
                    if (op.Contains(":"))
                    {
                        TimeCode tc = TimeCode.Parse(op.Substring(1), inTc.Fps);
                        inTc += tc;
                    }
                    else
                    {
                        inTc.AddFrames(nbFrames);
                    }
                }
                else if (op.StartsWith("-"))
                {
                    if (op.Contains(":"))
                    {
                        TimeCode tc = TimeCode.Parse(op.Substring(1), inTc.Fps);
                        inTc -= tc;
                    }
                    else
                    {
                        inTc.AddFrames(-nbFrames);
                    }
                }
                else if (op.StartsWith("="))
                {
                    inTc.Convert(float.Parse(op.Substring(1)));
                }
                else
                {
                    if (op.Contains(":"))
                    {
                        inTc = TimeCode.Parse(op, inTc.Fps);
                    }
                    else
                    {
                        inTc.TotalFrames = nbFrames;
                    }
                }
            }

            Console.WriteLine(inTc.ToLongString() + " " + inTc.ToString());

            return inTc;
        }
    }
}
