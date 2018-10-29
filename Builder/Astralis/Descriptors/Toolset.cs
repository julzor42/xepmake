using Builder.Astralis.XUtils;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Builder.Astralis.Descriptors
{
  public class Toolset : Descriptor
  {
    #region Members
    public IEnumerable<Tool> Tools => Elements("Tools", "Tool").Select(x => new Tool(x));
    public IEnumerable<Rule> Rules => Elements("Rules", "Rule").Select(x => new Rule(x)).Where(x => x.PlatformMatch);
    #endregion

    public class Rule : ElementBased
    {
      #region Properties
      public string Depends => Attribute("Depends");
      public string Content => (Element?.Value?.Replace("\t", string.Empty)) ?? string.Empty;
      public string[] Lines => Content.Split('\n');
      #endregion

      #region Constructor
      public Rule(XElement element) : base(element) { }
      #endregion
    }

    public class Tool : ElementBased
    {
      #region Properties
      public string Binary => Attribute("Binary");
      #endregion

      #region Constructor
      public Tool(XElement element) : base(element)
      {
      }
      #endregion
    }
  }
}
