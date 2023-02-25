using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kazang.Workflow
{
    public class PropertiesManager
    {
        public PropertiesManager() { }

        public Entity MapPropertiesParameters(Entity Property, Guid subID)
        {
            Entity newProperty = new Entity("new_property");
            newProperty["new_name"] = Property["new_name"];
            newProperty["new_propertyvalue"] = Property["new_propertyvalue"];
            newProperty["new_subid"] = new EntityReference("new_subentity", subID);

            return newProperty;
        }

    }
}
