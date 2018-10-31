using Builder.Astralis.Blocks;
using Builder.Astralis.Descriptors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Builder.Astralis.Generators
{
    public class MakefileGenerator
    {
        #region Properties
        public Project Project { get; }
        public StringBuilder Builder { get; private set; }
        public StringBuilder ExternalBuilder { get; private set; }
        public bool UseExternalMakefile { get; set; } = false;
        public List<IMakefileBlock> Blocks { get; } = new List<IMakefileBlock>();
        #endregion

        #region Constructor
        public MakefileGenerator(Project project)
        {
            Project = project;
        }
        #endregion

        #region Builders
        void AddBuilder<T>() where T : IMakefileBlock, new() => Blocks.Add(new T());
        T GetBuilder<T>() where T : class, IMakefileBlock => Blocks.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
        T GetBuilder<T>(string name) where T : class, IMakefileBlock => Blocks.FirstOrDefault(x => x.BuilderName == name) as T;
        #endregion

        #region Methods
        public void Generate()
        {
            if (Blocks.Count == 0)
            {
                AddBuilder<ToolsetBlock>();
                AddBuilder<FrameworkBlock>();
                AddBuilder<DriverBlock>();
                AddBuilder<ExternalSourcesBlock>();
            }

            Builder = new StringBuilder();
            ExternalBuilder = UseExternalMakefile ? new StringBuilder() : Builder;

            AppendHeaders();

            if (UseExternalMakefile)
            {
                Builder.AppendLine();
                Builder.AppendLine($"include {Project.Binary}.Tools.Makefile");
            }

            AppendVariables();
            AppendSourceFiles(Project);
            Blocks.ForEach(block => block.Process(this));
            AppendIncludes(Project);

            AppendDefinitions();

            ExternalBuilder.AppendLine();
            ExternalBuilder.AppendLine("# Automatically generated files");
            AppendDataFiles(Project);
            AppendDataFiles(Project.Framework);

            AppendLibraries();
            AppendRules();
        }

        #region Generation
        void AppendHeaders()
        {
            var now = DateTime.Now.ToLocalTime();

            if (UseExternalMakefile)
            {
                ExternalBuilder.AppendLine($"# This file was generated automatically on {now.ToShortDateString()} {now.ToShortTimeString()}");
                ExternalBuilder.AppendLine($"# Tools configuration for project {Project.Name}");
            }

            // Header
            Builder.AppendLine($"# This file was generated automatically on {now.ToShortDateString()} {now.ToShortTimeString()}");
            Builder.AppendLine($"# Builder: {Program.AppVersion}");
            Builder.AppendLine($"# Project: {Project.Name}");
            Builder.AppendLine($"# Target board: {Project.Board.Name}");
            Builder.AppendLine($"# Processor: {Project.Board.Processor.Name}");
            Builder.AppendLine($"# Framework: {Project.Framework.Name}");
        }

        void AppendVariables()
        {
            Builder.AppendLine();
            Builder.AppendLine("# Variables");
            if (!string.IsNullOrEmpty(Project.OutputDirectory))
                Builder.AppendLine($"OUTDIR={Catalog.ParsePath(Project.OutputDirectory)}");
            else
                Builder.AppendLine("OUTDIR=.");
            if (!string.IsNullOrEmpty(Project.ObjectDirectory)) Builder.AppendLine($"OBJDIR={Catalog.ParsePath(Project.ObjectDirectory)}"); else Builder.AppendLine(Catalog.ParsePath("OBJDIR=$(WORK)/obj"));
            Builder.AppendLine($"NAME={Project.Binary}");
        }

        public void AppendSourceFiles(SourceDescriptor desc, bool Ext = false)
        {
            StringBuilder builder = Ext ? ExternalBuilder : Builder;

            if (desc.SourceFiles != null)
            {
                builder.AppendLine();
                builder.AppendLine($"# Source files for {desc.Name}");
                foreach (var src in desc.SourceFiles)
                {
                    builder.AppendLine($"SRC += {src.FullPath}");

                    src.PreProcess();
                }
            }
        }

        public void AppendIncludes(SourceDescriptor desc, bool Ext = false)
        {
            StringBuilder builder = Ext ? ExternalBuilder : Builder;

            builder.AppendLine();
            builder.AppendLine($"# Include directories for {desc.Name}");
            foreach (var inc in desc.IncludeDirectories)
            {
                builder.AppendLine($"CFLAGS += -I{inc.FullPath}");
            }
        }

        void AppendLibraries()
        {
            // TODO
            Builder.AppendLine();
            Builder.AppendLine("# Library flags");
            Builder.AppendLine("LDFLAGS=");
        }

        void AppendRules()
        {
            Builder.AppendLine();
            Builder.AppendLine("OBJ=$(addprefix $(OBJDIR)/,$(patsubst %.c,%.o,$(SRC)))");
            Builder.AppendLine();
            foreach (var rule in Project.Toolset.Rules)
            {
                Builder.AppendLine($"{rule.Name}: {rule.Depends}");
                foreach (var line in rule.Lines)
                {
                    if (!string.IsNullOrEmpty(line))
                        Builder.AppendLine($"\t{Catalog.ParsePath(line)}");
                }
                Builder.AppendLine();
            }
        }

        void AppendDataFiles(SourceDescriptor desc)
        {
            var d = desc.DataFiles;
            if (d != null)
            {
                var DirName = Catalog.ResolvePath(Path.Combine("gen", desc.Name));
                ExternalBuilder.AppendLine($"CFLAGS += -I{DirName}");
                if (!Directory.Exists(DirName))
                    Directory.CreateDirectory(DirName);

                foreach (var df in d)
                {
                    string FilePath = Path.Combine(DirName, df.Name);
                    var hg = new HeaderGenerator(df);
                    hg.Export(FilePath);
                }
            }
        }

        void AppendDefinitionsBlock(Descriptor desc, bool Ext = false)
        {
            StringBuilder builder = Ext ? ExternalBuilder : Builder;

            if (desc.Definitions != null)
            {
                builder.AppendLine();
                builder.AppendLine($"# Definitions for {desc.Name}");
                foreach (var def in desc.Definitions)
                {
                    if (!string.IsNullOrEmpty(def.Value)) builder.AppendLine($"CFLAGS += -D{def.Name}={def.Value}");
                    else builder.AppendLine($"CFLAGS += -D{def.Name}");
                }
            }
        }

        void AppendDefinitions()
        {
            AppendDefinitionsBlock(Project);
            AppendDefinitionsBlock(Project.Framework, true);
            AppendDefinitionsBlock(Project.Board, true);
            AppendDefinitionsBlock(Project.Toolset, true);
            AppendDefinitionsBlock(Project.Board.Processor, true);

            foreach (var drv in Project.Drivers)
            {
                AppendDefinitionsBlock(drv.Source, true);
            }
        }
        #endregion

        public void Export(string FileName)
        {
            if (Builder == null)
                Generate();

            File.WriteAllText(FileName, Builder.ToString(), Encoding.UTF8);

            if (UseExternalMakefile)
            {
                File.WriteAllText($"{Project.Binary}.Tools.Makefile", ExternalBuilder.ToString());
            }
        }
        #endregion
    }
}
