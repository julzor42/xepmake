using BuildCommon.Execution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cc2
{
    public class Functive
    {
        #region Properties
        public Preprocessor Processor { get; set; }
        public string Name { get; set; }
        public string Line { get; set; }
        public string OriginalLine { get; set; }
        public StreamWriter Output => Processor.Output;
        public StreamReader Input => Processor.Input;
        #endregion

        #region Constructor
        #endregion
    }

    public class Preprocessor
    {
        #region Properties
        public PreprocessorConfiguration Config { get; } = new PreprocessorConfiguration();
        public string FinalPath { get; set; }
        public string FinalBinary { get; set; }
        public StreamReader Input { get; protected set; }
        public StreamWriter Output { get; protected set; }
        public int CurrentLine { get; protected set; }
        public string CurrentFile { get; protected set; }
        #endregion

        #region Members
        Dictionary<string, Action<Functive>> KnownDirectives { get; } = new Dictionary<string, Action<Functive>>();
        #endregion

        #region Constructor
        public Preprocessor()
        {

        }
        #endregion

        int RealCc1()
        {
            Executor e = new Executor
            {
                BinaryPath = FinalPath,
                FileName = FinalBinary,
                LogClass = "RealPreProcessor"
            };

            Config.Update();

            return e.Run(string.Join(" ", Config.UpdatedArgs.Select(x => $"\"{x}\"")));
        }

        #region Processing
        public void ParseArgs(string[] args)
        {
            Config.Parse(args);
            Config.NewInput = $"{Config.OutputFile}.stage{(Config.FirstStage ? 1 : 2)}";
        }

        public int Process()
        {
            int Result = ProcessFile(Config.FirstStage ? 1 : 2, Config.InputFile, Config.NewInput);

            if (Result == 0)
            {
                Result = RealCc1();
            }

            DeleteInput();

            return Result;
        }

        void DeleteInput()
        {
            if (File.Exists(Config.NewInput))
                File.Delete(Config.NewInput);
        }

        bool LocalLineProcessor(string Line, string Original)
        {
            if (Line.StartsWith("#") && Line.EndsWith(")"))
            {
                var Args = Line.Split('(');
                var DirectiveName = Args[0].ToLower().Substring(1).Trim();
                if (KnownDirectives.ContainsKey(DirectiveName))
                {
                    var f = new Functive
                    {
                        Processor = this,
                        Name = DirectiveName,
                        Line = Line,
                        OriginalLine = Original
                    };

                    KnownDirectives[DirectiveName](f);
                    return true;
                }

                return false;
            }
            else if (Line.StartsWith("#"))
            {

            }

            //if (Line.StartsWith("["))
            //{
            //  Output.WriteLine("#warning \"Hello world\"");
            //  return true;
            //}

            return false;
        }

        public void RegisterDirective(string Name, Action<Functive> Handler) => KnownDirectives.Add(Name.ToLower(), Handler);

        public void WriteCurrentLine()
        {
            Output.WriteLine($"# {CurrentLine} \"{CurrentFile}\"");
        }

        int ProcessFile(int Stage, string InputFile, string OutputFile)
        {
            Console.WriteLine($"STAGE {Stage} -----------------------------");

            CurrentFile = InputFile;
            int Result = 0;
            CurrentLine = 0;
            using (var InStream = File.OpenRead(InputFile))
            {
                using (var OutStream = File.Open(OutputFile, FileMode.Create, FileAccess.Write))
                {
                    var enc = Encoding.Default;
                    Input = new StreamReader(InStream, enc);
                    Output = new StreamWriter(OutStream, enc);

                    string Line;
                    while (true)
                    {
                        Line = Input.ReadLine();
                        if (Line == null)
                            break;

                        CurrentLine++;

                        var Trim = Line.Replace("\t", string.Empty).Trim();
                        if (!ProcessLine(this, Trim))
                        {
                            if (!LocalLineProcessor(Trim, Line))
                            {
                                // Simply copy the line
                                Output.WriteLine(Line);
                            }
                        }
                    }

                    Output.Dispose();
                    OutStream.Close();
                }

                Input.Dispose();
                InStream.Close();
            }


            return Result;
        }
        #endregion


        #region Events
        public Func<Preprocessor, string, bool> ProcessLine;
        #endregion
    }
}
