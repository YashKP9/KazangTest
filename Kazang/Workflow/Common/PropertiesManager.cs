using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
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

        public Entity MapMasterPropertiesParameters(Entity subProperty, Entity masterProperty)
        {
            //masterProperty["new_name"] = Property["new_name"];
            masterProperty["new_propertyvalue"] = subProperty["new_propertyvalue"];

            return masterProperty;
        }

        public Entity FindPropertyonMaster(Entity subProperty, Guid masterGUID, IOrganizationService _service)
        {
            EntityCollection entityCollection = null;
            Entity entity = null;
            string FetchXML = String.Empty;

            try
            {

                FetchXML = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                                <entity name='new_property'>
                                                <attribute name='new_name' />
                                                <attribute name='new_propertyid' />
                                                <attribute name='new_propertyvalue' />
                                                <order attribute='createdon' descending='true' />
                                                <filter type='and'>  
                                                   <condition attribute='new_masterid' operator='eq' value='" + masterGUID + @"' />    
                                                   <condition attribute='new_name' operator='eq' value='" + subProperty["new_name"] + @"' />                                             
                                                </filter>
                                                </entity>
                                            </fetch>";

                entityCollection = ExecuteFetch(_service, FetchXML);

                if (entityCollection.Entities.Count > 0)
                {
                    entity = entityCollection[0];
                }
            }
            catch (Exception ex)
            {
                entityCollection = null;
            }
            return entity;
        }

        public EntityCollection ExecuteFetch(IOrganizationService CrmService, string FetchXml)
        {
            try
            {
                FetchExpression fetch = new FetchExpression(FetchXml);
                EntityCollection entityCollection = CrmService.RetrieveMultiple(fetch);

                return entityCollection;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

    }
}
