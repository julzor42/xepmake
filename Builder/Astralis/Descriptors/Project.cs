using System.Collections.Generic;
using System.Linq;

namespace Builder.Astralis.Descriptors
{
    public class Project : SourceDescriptor
    {
        #region Properties
        public string Binary => DescriptionAttribute("Binary");
        public string OutputDirectory => DescriptionAttribute("OutputDirectory");
        public string ObjectDirectory => DescriptionAttribute("ObjectDirectory");
        public Framework Framework => Catalog.FindFramework(DescriptionAttribute("Framework"));
        public Board Board => Catalog.FindBoard(DescriptionAttribute("Board"));
        public Toolset Toolset => Catalog.FindToolset(DescriptionAttribute("Toolset"));
        public IEnumerable<ConfigurableObject<Driver>> Drivers => Elements("Drivers", "Driver")?.Select(x => Catalog.GetDriverInfo(x));
        #endregion

        public override void OnLoad()
        {

        }
    }
}
