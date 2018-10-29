using BuildCommon.Execution;

namespace Builder.Astralis.Execution
{
  public class Git : Executor
  {
    #region Properties
    public string Address { get; set; }
    public string Output { get; set; }
    #endregion

    #region Constructor
    public Git()
    {
      BinaryPath = Program.Config.GetPath("Git");
      FileName = "git";
      LogClass = "Git";
    }
    #endregion

    public void Clone() => Run($"clone http://github.com/{Address} {Output}");
    public void Pull()
    {
      Program.Log($"Trying to upgrade {Address}", LogClass);
      WorkingDirectory = Output;
      Run($"pull");
    }
  }
}
