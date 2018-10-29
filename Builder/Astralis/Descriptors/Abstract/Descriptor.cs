using Builder.Astralis.XUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Builder.Astralis.Descriptors
{
    public abstract class Descriptor
    {
        #region Constructor
        public Descriptor()
        {

        }
        #endregion

        #region Properties
        public XDocument Document { get; private set; }
        public string Name => DescAttribute("Name");
        public IEnumerable<Definition> Definitions => Elements("Defines", "Define")?.Select(x => new Definition(x)).Where(x => x.PlatformMatch);
        public IEnumerable<Repository> Repositories => Elements("Repositories", "Repository")?.Select(x => new Repository(x)).Where(x => x.PlatformMatch);
        #endregion

        #region XML methods
        public XElement Element(string ElementName) => Document.Root.Element(ElementName);
        public IEnumerable<XElement> Elements(string ElementName, string TypeName) => Element(ElementName)?.Elements(TypeName);
        public IEnumerable<XElement> Elements(string ElementName) => Element(ElementName)?.Elements();
        public string Attribute(string ElementName, string AttributeName) => Element(ElementName)?.Attribute(AttributeName)?.Value ?? string.Empty;
        public string DescAttribute(string AttributeName) => Attribute("Description", AttributeName);
        public string DescItem(string ItemName) => Element("Description")?.Element(ItemName)?.Value ?? string.Empty;
        public string DescItemAttr(string ItemName, string AttributeName) => Element("Description")?.Element(ItemName)?.Attribute(AttributeName)?.Value ?? string.Empty;
        #endregion

        #region Virtual methods
        public virtual void OnLoad()
        {

        }
        #endregion

        #region Static methods
        public static List<T> LoadType<T>(string DataDirectory, string SourcePath) where T : Descriptor, new()
        {
            var result = new List<T>();

            var FullPath = Path.Combine(DataDirectory, SourcePath);
            foreach (var FileName in Directory.GetFiles(FullPath, "*.xml"))
            {
                try
                {
                    result.Add(Load<T>(FileName));
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to load {FileName}: {e.Message}");
                }
            }

            return result;
        }

        public static T Load<T>(string FileName) where T : Descriptor, new()
        {
            var res = new T();

            res.Document = XDocument.Load(FileName);

            Debug.WriteLine($"Loaded {typeof(T).Name}: {res.Name}");

            res.OnLoad();

            return res;
        }
        #endregion

        #region Defines
        public class Definition : ElementBased
        {
            #region Constructor
            public Definition(XElement element) : base(element) { }
            #endregion
        }
        #endregion

        #region Repositories
        public class Repository : ElementBased
        {
            #region Properties
            public string Type => Attribute("Type");
            public string Address => Attribute("Address");
            public string Output => Attribute("Output");
            #endregion

            #region Constructor
            public Repository(XElement element) : base(element) { }
            #endregion
        }
        #endregion

        public class ConfigurableObject<T>
        {
            #region Properties
            public T Source { get; }
            public XElement Configuration { get; }
            #endregion

            #region Constructor
            public ConfigurableObject(XElement Elem, Func<string, T> DataResolver)
            {
                Configuration = Elem;
                Source = DataResolver(Elem.Attribute("Type")?.Value);
            }
            #endregion
        }
    }


}
