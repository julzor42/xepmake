using System;
using System.IO;
using System.Linq;
using System.Text;
using static Builder.Astralis.Descriptors.SourceDescriptor;

namespace Builder.Astralis.Generators
{
    public class HeaderGenerator
    {
        #region Properties
        StringBuilder Builder { get; } = new StringBuilder();
        public DataFile Data { get; }
        #endregion

        #region Members
        int NamePadding;
        int ValuePadding;
        #endregion

        #region Constructor
        public HeaderGenerator(DataFile file)
        {
            Data = file;
        }
        #endregion

        void AppendField(DataFileField field)
        {
            string FieldName = field.Name.PadRight(NamePadding);
            string FieldValue = field.Default.PadRight(ValuePadding);

            Builder.AppendLine($"#define {FieldName} {FieldValue} // {field.Comment}");
        }

        public void Export(string FileName)
        {
            var now = DateTime.Now.ToLocalTime();

            Builder.Clear();
            Builder.AppendLine($"// This file was generated automatically on {now.ToShortDateString()} {now.ToShortTimeString()}");
            Builder.AppendLine("#pragma once");

            Builder.AppendLine();
            NamePadding = Data.Fields.Max(x => x.Name.Length) + 4;
            ValuePadding = Data.Fields.Max(x => x.Default.Length) + 4;
            foreach (var field in Data.Fields)
            {
                AppendField(field);
            }

            File.WriteAllText(FileName, Builder.ToString(), Encoding.UTF8);
        }
    }
}
