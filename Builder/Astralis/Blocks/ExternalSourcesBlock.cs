using Builder.Astralis.Generators;

namespace Builder.Astralis.Blocks
{
    public class ExternalSourcesBlock : IMakefileBlock
    {
        string IMakefileBlock.BuilderName => "ExternalSources";

        void IMakefileBlock.Process(MakefileGenerator generator)
        {
            if (Program.Config.AdvSource.Count > 0)
            {
                generator.Builder.AppendLine();
                generator.Builder.AppendLine("# Source files");
                foreach (var src in Program.Config.AdvSource)
                {
                    generator.Builder.AppendLine($"SRC += {src}");
                }
            }
        }
    }
}
