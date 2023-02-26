using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Kazang.Plugins
{
    public class SubEntityManager
    {
        private Utils UtilManager = new Utils();

        public SubEntityManager() { }

        public EntityCollection RetrieveSubEntities(Guid EntityGUID, IOrganizationService CrmService)
        {
            String RetLog = String.Empty;
            EntityCollection entityCollection = null;
            try
            {
                String FetchXML = String.Empty;
                FetchXML = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                                <entity name='new_subentity'>
                                                <attribute name='new_name' />
                                                <attribute name='new_subentityid' />
                                                <attribute name='new_authorize' />
                                                <order attribute='createdon' descending='true' />
                                                <filter type='and'>  
                                                   <condition attribute='new_master' operator='eq' value='" + EntityGUID + @"' />     
                                                   <condition attribute='statecode' operator='eq' value='0' />                                         
                                                </filter>
                                                </entity>
                                            </fetch>";

                entityCollection = UtilManager.ExecuteFetch(CrmService, FetchXML);
                if (entityCollection != null)
                {
                    RetLog = RetLog + entityCollection.Entities.Count;
                }
                else
                {
                    RetLog = RetLog + "No SubEntities Found";
                }
            }
            catch (Exception ex)
            {
                entityCollection = null;
                RetLog = RetLog + ex.Message;
            }
            return entityCollection;
        }

    }
}
