using Builder.Astralis;
using Builder.Astralis.Descriptors;
using Builder.Astralis.Execution;
using System.IO;

namespace Builder
{
    public static class RepositoryResolver
    {
        static void ResolveRepo(Descriptor desc)
        {
            if (desc.Repositories == null)
                return;

            foreach (var repo in desc.Repositories)
            {
                switch (repo.Type.ToLower())
                {
                    case "git":
                        Git git = new Git { Address = repo.Address, Output = Catalog.ParsePath(repo.Output) };

                        if (!Directory.Exists(git.Output)) git.Clone();
                        else if (Program.Config.Upgrade) git.Pull();
                        break;
                }

            }
        }

        public static void ResolveForProject(Project project)
        {
            ResolveRepo(project);
            ResolveRepo(project.Framework);
            ResolveRepo(project.Board);
            ResolveRepo(project.Board.Processor);
            ResolveRepo(project.Toolset);

            foreach (var drv in project.Drivers)
                ResolveRepo(drv.Source);
        }
    }
}
