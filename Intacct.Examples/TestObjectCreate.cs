/*
 * Copyright 2018 Sage Intacct, Inc.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"). You may not
 * use this file except in compliance with the License. You may obtain a copy 
 * of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * or in the "LICENSE" file accompanying this file. This file is distributed on 
 * an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either 
 * express or implied. See the License for the specific language governing 
 * permissions and limitations under the License.
 */

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