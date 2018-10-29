using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Builder.Astralis.XUtils
{
    public class ElementBased
    {
        #region Properties
        public XElement Element { get; }
        public string Name => Attribute("Name");
        public string Value => Attribute("Value");
        public string Platform => Attribute("Platform");
        public bool PlatformMatch => string.IsNullOrEmpty(Platform) || (Platform.ToLower() == Environment.OSVersion.Platform.ToString().ToLower());
        #endregion

        #region Constructor
        public ElementBased(XElement element)
        {
            Element = element;
        }
        #endregion

        #region Methods
        protected string Attribute(string AttributeName) => Element.Attribute(AttributeName)?.Value ?? string.Empty;
        public XElement Child(string ElementName) => Element.Element(ElementName);
        public IEnumerable<XElement> Children() => Element.Elements();
        public IEnumerable<XElement> Children(string TypeName) => Element.Elements(TypeName);
        #endregion
    }
}
