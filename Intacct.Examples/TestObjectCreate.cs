using System;
using Intacct.SDK.Xml;

namespace Intacct.Examples
{
    public class TestObjectCreate : AbstractTestObject
    {
        public override void WriteXml(ref IaXmlWriter xml)
        {
            xml.WriteStartElement("function");
            xml.WriteAttribute("controlid", this.ControlId, true);
            xml.WriteStartElement("create");
            xml.WriteStartElement(IntegrationName); // Integration name in the system.

            if (string.IsNullOrEmpty(this.Name))
            {
                throw new ArgumentException("Name field is required for create");
            }
            
            xml.WriteElement("NAME", this.Name, true);
            
            xml.WriteEndElement(); // test_object
            xml.WriteEndElement(); // create
            xml.WriteEndElement(); // function
        }
    }
}