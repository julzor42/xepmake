using System.Collections.Generic;
using System.Xml.Linq;

namespace Builder.Astralis.Descriptors
{
    public class CPU : Descriptor
    {
        #region Properties
        public IEnumerable<XElement> Ports => Elements("Ports", "Port");
        #endregion
    }
}
