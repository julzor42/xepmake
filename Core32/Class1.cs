using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Core32
{
  public static class Test
  {
    public static List<string> RegisterPragmas()
    {
      var res = new List<string>();

      Console.WriteLine("Registering pragmas");
      res.Add("driver");

      return res;
    }

    public static bool OnPragma(string Pragma, StreamReader Input, StreamWriter Output, string Data)
    {
      switch (Pragma)
      {
        case "driver":
          //Output.WriteLine("func();");
          break;
      }
      return true;
    }
  }
}
