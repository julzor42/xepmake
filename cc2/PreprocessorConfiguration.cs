using BuildCommon.Configuration;
using BuildCommon.Logging;
using System.Collections.Generic;

namespace cc2
{
    public class PreprocessorConfiguration : ArgumentParser
    {
        #region Properties
        public bool FirstStage { get; private set; }
        public string OutputFile { get; private set; }
        public string InputFile { get; private set; }
        public List<string> IncludeDirectories { get; } = new List<string>();
        public List<string> Definitions { get; } = new List<string>();
        public Dictionary<string, string> Vars { get; } = new Dictionary<string, string>();
        public List<string> UpdatedArgs = new List<string>();
        public string NewOutput { get; set; }
        public string NewInput { get; set; }
        #endregion

        public void Update()
        {
            foreach (var arg in Arguments)
            {
                if (arg == InputFile && !string.IsNullOrEmpty(NewInput))
                    UpdatedArgs.Add(NewInput);
                else
                  if (arg == OutputFile && !string.IsNullOrEmpty(NewOutput))
                    UpdatedArgs.Add(NewOutput);
                else
                    UpdatedArgs.Add(arg);
            }
        }

        #region Parser
        public override void ProcessArgument(string Argument)
        {
            if (!Argument.StartsWith("-") && FirstStage)
            {
                InputFile = Argument;
                return;
            }

            switch (Argument)
            {
                case "-E": FirstStage = true; break;
                case "-o": OutputFile = NextArg; break;
                case "-c": InputFile = NextArg; break;
                case "-I": IncludeDirectories.Add(NextArg); break;
                case "-D":
                    {
                        var Data = NextArg;
                        if (Data.StartsWith("_cc2_"))
                        {
                            Data = Data.Substring(5);
                            int iPos = Data.IndexOf('=');
                            var Var = Data.Substring(0, iPos);
                            var Value = Data.Substring(iPos + 1);
                            Vars.Add(Var, Value);
                            Log.Write($"{Var} = {Value}");
                        }
                        Definitions.Add(Data);
                    }
                    break;
                case "-iprefix": SkipArg(); break;
                case "-auxbase-strip": SkipArg(); break;
                case "-fpreprocessed": InputFile = NextArg; break;
            }

        }
        #endregion
    }
}
