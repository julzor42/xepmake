using BuildCommon.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System;

namespace Builder
{
  public class BuilderConfig : ArgumentParser
  {
    #region Properties
    public List<string> Rules { get; } = new List<string>();
    public XDocument Document { get; private set; }
    public bool Upgrade { get; private set; }
    public List<string> AdvSource { get; } = new List<string>();
    #endregion

    #region Constructor
    public BuilderConfig()
    {
      try
      {
        Document = XDocument.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Builder.xml"));
      }
      catch
      {
        Document = new XDocument();
        Program.Log("Configuration file not found!", "Error");
      }
    }
    #endregion

    public string GetPath(string For) => Document.Root?.Element("Paths")?.Elements()?.FirstOrDefault(x => x.Name.LocalName.ToLower() == For.ToLower())?.Value?.Replace("\n", string.Empty).Trim() ?? string.Empty;

    #region Arguments
    public override void ProcessArgument(string Argument)
    {
      if (Argument.StartsWith("-"))
        switch (Argument.ToLower())
        {
          case "-source": AdvSource.Add(NextArg); break;
          case "-upgrade": Upgrade = true; break;
          case "-rule": Rules.Add(NextArg); break;
          case "-build": Rules.Add("all"); break;
          case "-rebuild":
            {
              Rules.Add("clean");
              Rules.Add("all");
            }
            break;

        }
      else Result = Argument;
    }

    #endregion
  }

}
