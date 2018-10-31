using Builder.Astralis.Generators;

namespace Builder.Astralis.Blocks
{
    public class FrameworkBlock : IMakefileBlock
    {
        string IMakefileBlock.BuilderName => "Framework";

        void IMakefileBlock.Process(MakefileGenerator generator)
        {
            generator.AppendSourceFiles(generator.Project.Framework, true);
            generator.AppendIncludes(generator.Project.Framework, true);
        }
    }
}
