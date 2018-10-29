using System.Collections.Generic;
using System.Linq;

namespace Builder.Astralis.Descriptors
{
    public class Project : SourceDescriptor
    {
        #region Properties
        public string Binary => DescAttribute("Binary");
        public string OutputDirectory => DescAttribute("OutputDirectory");
        public string ObjectDirectory => DescAttribute("ObjectDirectory");
        public Framework Framework => Catalog.FindFramework(DescAttribute("Framework"));
        public Board Board => Catalog.FindBoard(DescAttribute("Board"));
        public Toolset Toolset => Catalog.FindToolset(DescAttribute("Toolset"));
        public IEnumerable<ConfigurableObject<Driver>> Drivers => Elements("Drivers", "Driver")?.Select(x => Catalog.GetDriverInfo(x));
        #endregion

        public override void OnLoad()
        {

        }
    }
}
