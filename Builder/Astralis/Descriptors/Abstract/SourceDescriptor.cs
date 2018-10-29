using Builder.Astralis.XUtils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Builder.Astralis.Descriptors
{
    public class SourceDescriptor : Descriptor
    {
        #region Properties
        public IEnumerable<IncludeDirectory> IncludeDirectories => Elements("Include", "Directory")?.Select(x => new IncludeDirectory(x));
        public IEnumerable<SourceFile> SourceFiles => Elements("Source", "File")?.Select(x => new SourceFile(x));
        //public IEnumerable<SourceFile> SourcePatterns => Elements("Source", "Files")?.Select(x => new SourceFile(x));
        public IEnumerable<DataFile> DataFiles => Elements("DataFiles", "DataFile")?.Select(x => new DataFile(x));
        #endregion

        #region Source files
        public class SourceFile : ElementBased
        {
            #region Properties
            public string Path => Attribute("Path");
            public string Base => Element.Parent.Attribute("BaseDirectory")?.Value ?? string.Empty;
            public string FullPath => Catalog.ParsePath(System.IO.Path.Combine(Base, Path)).Replace('\\', '/');
            public string Preprocessor => Attribute("Preprocessor");
            #endregion

            #region Constructor
            public SourceFile(XElement element) : base(element) { }
            #endregion

            #region Preprocessing
            bool ProcessBlock(string Data, List<string> Output)
            {
                Output.Add($"// This section was automatically generated, do not modify it manually");
                Output.Add($"const char g_szBlock[] = \"{Data},{Preprocessor}\";");
                return true;
            }


            string ArgsAt(string Line, int Pos) => Line.Substring(Pos, Line.Length - Pos - 1);

            public void PreProcess()
            {
                if (string.IsNullOrEmpty(Preprocessor))
                    return;

                var lines = File.ReadAllLines(FullPath);
                List<string> output = new List<string>();

                bool Skipping = false;

                foreach (var line in lines)
                {
                    string Trimmed = line.Trim();
                    if (!Skipping)
                    {
                        if (Trimmed.StartsWith("#pragma driver("))
                        {
                            string DriverName = ArgsAt(Trimmed, 15);
                            output.Add(line);
                            var Driver = Catalog.FindDriver(DriverName);

                            if (Driver != null)
                            {
                                Driver.Used = true;
                            }
                        }
                        else if (Trimmed.StartsWith("#pragma BeginBlock("))
                        {
                            output.Add(line);
                            Skipping = ProcessBlock(ArgsAt(Trimmed, 20), output);
                        }
                        else
                            output.Add(line);
                    }
                    {
                        if (Trimmed.StartsWith("#pragma EndBlock()"))
                        {
                            output.Add(line);
                            Skipping = false;
                        }
                    }
                }

                File.WriteAllLines(FullPath, output);
            }
            #endregion

        }
        #endregion

        #region Include directories
        public class IncludeDirectory : ElementBased
        {
            #region Properties
            public string Path => Attribute("Path");
            public string Base => Element.Parent.Attribute("BaseDirectory")?.Value ?? string.Empty;
            public string FullPath => Catalog.ParsePath(System.IO.Path.Combine(Base, Path).Replace('\\', '/'));
            #endregion

            #region Constructor
            public IncludeDirectory(XElement element) : base(element) { }
            #endregion
        }
        #endregion

        #region DataFiles
        public class DataFileField : ElementBased
        {
            #region Properties
            public string Default => Attribute("Default");
            public string Comment => Attribute("Comment");
            #endregion

            #region Constructor
            public DataFileField(XElement element) : base(element) { }
            #endregion
        }

        public class DataFile : ElementBased
        {
            #region Properties
            public IEnumerable<DataFileField> Fields => Children("Field").Select(x => new DataFileField(x));
            #endregion

            #region Constructor
            public DataFile(XElement element) : base(element) { }
            #endregion
        }
        #endregion

    }
}
