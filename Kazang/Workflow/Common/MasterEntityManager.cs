using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Kazang.Workflow
{
    public class MasterEntityManager
    {
        public MasterEntityManager() { }

        public EntityCollection RetrieveProperties(Guid MasterEntityGUID, IOrganizationService CrmService)
        {
            String RetLog = String.Empty;
            EntityCollection entityCollection = null;
            try
            {
                String FetchXML = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                                <entity name='new_property'>
                                                <attribute name='new_name' />
                                                <attribute name='new_propertyid' />
                                                <attribute name='new_propertyvalue' />
                                                <order attribute='createdon' descending='true' />
                                                <filter type='and'>  
                                                   <condition attribute='new_masterid' operator='eq' value='" + MasterEntityGUID + @"' />                                              
                                                </filter>
                                                </entity>
                                            </fetch>";

                entityCollection = ExecuteFetch(CrmService, FetchXML);
                if (entityCollection != null)
                {
                    RetLog = RetLog + entityCollection.Entities.Count;
                }
                else
                {
                    RetLog = RetLog + "No Properties Found";
                }
            }
            catch (Exception ex)
            {
                entityCollection = null;
                RetLog = RetLog + ex.Message;
            }
            return entityCollection;
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
