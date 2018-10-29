using Builder.Astralis.Descriptors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Builder.Astralis
{
    public static class Catalog
    {
        #region Members
        public static string DataDirectory { get; set; }
        public static string WorkDirectory { get; set; }
        public static List<CPU> Processors { get; private set; }
        public static List<Board> Boards { get; private set; }
        public static List<Connector> Connectors { get; private set; }
        public static List<Driver> Drivers { get; private set; }
        public static List<Framework> Frameworks { get; private set; }
        public static List<Port> Ports { get; private set; }
        public static List<Toolset> Toolsets { get; private set; }
        #endregion

        #region Search methods
        public static CPU FindProcessor(string Name) => Processors.Find(x => x.Name.ToLower() == Name.ToLower());
        public static Board FindBoard(string Name) => Boards.Find(x => x.Name.ToLower() == Name.ToLower());
        public static Connector FindConnector(string Name) => Connectors.Find(x => x.Name.ToLower() == Name.ToLower());
        public static Driver FindDriver(string Name) => Drivers.Find(x => x.Name.ToLower() == Name.ToLower());
        public static Framework FindFramework(string Name) => Frameworks.Find(x => x.Name.ToLower() == Name.ToLower());
        public static Port FindPort(string Name) => Ports.Find(x => x.Name.ToLower() == Name.ToLower());
        public static Toolset FindToolset(string Name) => Toolsets.Find(x => x.Name.ToLower() == Name.ToLower());
        #endregion

        #region Methods
        public static void Initialize()
        {
            Processors = Descriptor.LoadType<CPU>(DataDirectory, "cpu");
            Boards = Descriptor.LoadType<Board>(DataDirectory, "board");
            Connectors = Descriptor.LoadType<Connector>(DataDirectory, "conn");
            Drivers = Descriptor.LoadType<Driver>(DataDirectory, "drivers");
            Frameworks = Descriptor.LoadType<Framework>(DataDirectory, "fw");
            Ports = Descriptor.LoadType<Port>(DataDirectory, "port");
            Toolsets = Descriptor.LoadType<Toolset>(DataDirectory, "toolset");
        }

        public static string ResolvePath(string PathName)
        {
            // TODO: Absolute path returns PathName

            if (!string.IsNullOrEmpty(WorkDirectory))
            {
                if (!Directory.Exists(WorkDirectory))
                {
                    Directory.CreateDirectory(WorkDirectory);
                }
                return Path.Combine(WorkDirectory, PathName);
            }

            return PathName;
        }

        public static string ParsePath(string PathName)
        {
            PathName = PathName.Replace("$(WORK)", WorkDirectory);
            PathName = PathName.Replace("$(ROOT)", Program.AppPath);

            return PathName;
        }

        #region Configurable elements
        public static Descriptor.ConfigurableObject<Driver> GetDriverInfo(XElement Element) => new Descriptor.ConfigurableObject<Driver>(Element, FindDriver);
        #endregion

        public static Project LoadProject(string FileName)
        {
            return Descriptor.Load<Project>(FileName);
        }
        #endregion
    }
}
