using Builder.Astralis.Generators;
using System.Linq;

namespace Builder.Astralis.Blocks
{
    public class DriverBlock : IMakefileBlock
    {
        string IMakefileBlock.BuilderName => "Driver";

        void IMakefileBlock.Process(MakefileGenerator generator)
        {
            foreach (var driver in generator.Project.Drivers)
            {
                driver.Source.Used = true;
                // TODO: load driver config?
                //        AppendSourceFiles(drv.Source, true);
                //       AppendIncludes(drv.Source, true);
            }

            foreach (var driver in Catalog.Drivers.Where(x => x.Used))
            {
                generator.AppendSourceFiles(driver, true);
                generator.AppendIncludes(driver, true);
            }
        }
    }
}
