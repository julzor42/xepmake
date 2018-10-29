using Builder.Astralis;
using Builder.Astralis.Descriptors;
using Builder.Astralis.Execution;
using Builder.Astralis.Generators;
using System;
using System.IO;
using System.Reflection;

namespace Builder
{
    public static class Program
    {
        #region Properties
        public static string AppPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string AppVersion => $"{Assembly.GetExecutingAssembly().GetName().Name} {Assembly.GetExecutingAssembly().GetName().Version}";
        public static BuilderConfig Config = new BuilderConfig();
        #endregion

        #region Utilities
        public static void Log(string Message, string Class = "Generation") => Console.WriteLine($"[{Class}] {Message}");
        #endregion

        static void Main(string[] args)
        {
            Config.Parse(args);
            string ProjectFile = Config.Result;
            if (string.IsNullOrEmpty(ProjectFile))
                ProjectFile = "project.xml";

            Log(AppVersion);
            try
            {
                Log("Loading data...");
                Catalog.DataDirectory = Path.Combine(AppPath, "data");
                Catalog.WorkDirectory = "build";
                Catalog.Initialize();

                Log("Loading project...");
                Project project;
                project = Catalog.LoadProject(ProjectFile);

                RepositoryResolver.ResolveForProject(project);

                Log("Generating files...");
                MakefileGenerator m = new MakefileGenerator(project);
                m.UseExternalMakefile = false;
                m.Export("Makefile");

                Make make = new Make();
                foreach (var rule in Config.Rules)
                {
                    Log($"Rule {rule}");
                    make.Run(rule);
                }

                Log("Finished");
            }
            catch (Exception e)
            {
                Log($"Unhandled: {e.Message}", "Error");
            }
        }
    }
}
