using BuildCommon.Execution;

namespace Builder.Astralis.Execution
{
    public class Make : Executor
    {
        #region Constructor
        public Make()
        {
            BinaryPath = Program.Config.GetPath("Make");
            FileName = "make";
            LogClass = "Make";
        }
        #endregion
    }
}
