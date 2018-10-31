using Builder.Astralis.Generators;

namespace Builder.Astralis.Blocks
{
    public class ToolsetBlock : IMakefileBlock
    {
        string IMakefileBlock.BuilderName => "Toolset";

        void IMakefileBlock.Process(MakefileGenerator generator)
        {
            generator.ExternalBuilder.AppendLine();
            generator.ExternalBuilder.AppendLine($"# Toolset ({generator.Project.Toolset.Name})");
            generator.ExternalBuilder.AppendLine($"TOOLSETPATH={Program.Config.GetPath($"Toolset.{generator.Project.Toolset.Name}")}");
            foreach (var tool in generator.Project.Toolset.Tools)
            {
                generator.ExternalBuilder.AppendLine($"{tool.Name}=$(TOOLSETPATH)/{tool.Binary}");
            }
        }
    }
}
