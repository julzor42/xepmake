using BuildCommon.Execution;
using BuildCommon.Logging;
using System;
using System.IO;
using System.Linq;

namespace cc2
{
    class Program
    {
        static int Main(string[] args)
        {
            var p = new Preprocessor
            {
                FinalPath = @"C:\Program Files (x86)\Microchip\xc32\v1.42\bin\bin\gcc\pic32mx\4.8.3",
                FinalBinary = "cc1"
            };

            p.ParseArgs(args);

            Action<Functive> fx = (f) => { Console.WriteLine($"Directive: {f.Name} --> {f.Line}"); };

            if (p.Config.FirstStage)
            {
                p.RegisterDirective("test", (f) =>
                {
                    Console.WriteLine("Test");

            //# 1 "c:\\program files (x86)\\microchip\\xc32\\v1.42\\pic32mx\\include\\lega-c\\p32xxxx.h" 1 3
            f.Processor.WriteCurrentLine();
                    f.Output.WriteLine("#error This is a test");
                });

                p.RegisterDirective("using", fx);
                p.RegisterDirective("data", fx);
                p.RegisterDirective("driver", fx);
                p.RegisterDirective("global", fx);
                p.RegisterDirective("require", fx);
                p.RegisterDirective("xmlvalue", fx);
                p.RegisterDirective("if exists", fx);
                p.RegisterDirective("run", fx);
                p.RegisterDirective("filesystem", fx);
                p.RegisterDirective("increment", fx);
            }


            p.ProcessLine = (proc, line) =>
            {
                return false;
            };

            return p.Process();
        }
    }
}
