//-----------------------------------------------------------------------
// <copyright file="XmlObjetcSerializer.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Martin Trejo Pancardo/author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils
{
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public static class XmlObjetcSerializer
    {
        public static string ToXml<T>(this T value)
        {
            if (value == null)
                return null;

            string xml = string.Empty;

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings() { Encoding = Encoding.UTF8, Indent = true, OmitXmlDeclaration = true };

            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value, new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty, string.Empty) }));
                }

                xml = textWriter.ToString();
            }

            return xml;
        }

        public static T FromXml<T>(this string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return default(T);

            var obj = default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlReaderSettings settings = new XmlReaderSettings();

            using (StringReader textReader = new StringReader(xml))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                {
                    obj = (T)serializer.Deserialize(xmlReader);
                }
            }

            return obj;
        }
    }
}
