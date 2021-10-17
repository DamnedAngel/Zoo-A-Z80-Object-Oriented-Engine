using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace ZooBuilder
{
    class XMLValidator
    {
        public static void Validate(Action<object, ValidationEventArgs> callback,  String xmlFilename, String  xsdFilename)
        {
            //Load the XmlSchemaSet.
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(null, xsdFilename);
            XmlSchema compiledSchema = null;
            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                compiledSchema = schema;
            }
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(compiledSchema);
            settings.ValidationEventHandler += new ValidationEventHandler(callback);

            settings.ValidationType = ValidationType.Schema;

            //Create the schema validating reader.
            XmlReader vreader = XmlReader.Create(xmlFilename, settings);

            while (vreader.Read()) { }

            //Close the reader.
            vreader.Close();
        }
    }
}
