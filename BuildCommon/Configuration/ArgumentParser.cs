namespace BuildCommon.Configuration
{
    public abstract class ArgumentParser
    {
        public abstract void ProcessArgument(string Argument);

        public string[] Arguments { get; protected set; }

        public int CurrentPosition { get; protected set; }

        public string Result { get; protected set; }
        public string NextArg => Arguments[++CurrentPosition];
        public void SkipArg() => CurrentPosition++;

        public void Parse(string[] args)
        {
            Arguments = args;
            string Result = string.Empty;

            for (CurrentPosition = 0; CurrentPosition < Arguments.Length; CurrentPosition++)
            {
                ProcessArgument(Arguments[CurrentPosition]);
            }
        }
    }
}
