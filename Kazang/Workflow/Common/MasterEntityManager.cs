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

        public EntityCollection RetrieveProperties(Guid EntityGUID, IOrganizationService CrmService, String entityName)
        {
            String RetLog = String.Empty;
            EntityCollection entityCollection = null;
            try
            {
                String FetchXML = String.Empty;

                switch (entityName)
                {
                    case "new_masterentity":
                        FetchXML = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                                <entity name='new_property'>
                                                <attribute name='new_name' />
                                                <attribute name='new_propertyid' />
                                                <attribute name='new_propertyvalue' />
                                                <order attribute='createdon' descending='true' />
                                                <filter type='and'>  
                                                   <condition attribute='new_masterid' operator='eq' value='" + EntityGUID + @"' />                                              
                                                </filter>
                                                </entity>
                                            </fetch>";
                        break;
                    case "new_subentity":
                        FetchXML = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                            <entity name='new_property'>
                            <attribute name='new_name' />
                            <attribute name='new_propertyid' />
                            <attribute name='new_propertyvalue' />
                            <order attribute='createdon' descending='true' />
                            <filter type='and'>  
                                <condition attribute='new_subid' operator='eq' value='" + EntityGUID + @"' />                                              
                            </filter>
                            </entity>
                        </fetch>";
                        break;
                    default:
                        break;
                }

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

        public Entity RetrieveMasterEntity(Guid masterGUID, IOrganizationService _service)
        {
            EntityCollection entityCollection = null;
            Entity entity = null;
            string FetchXML = String.Empty;

            try
            {

                FetchXML = String.Format(@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='new_masterentity'>
                                <attribute name='new_masterentityid' />
                                <filter type='and'>
                                    <condition attribute='new_masterentityid' operator='eq' value='{0}' />
                                </filter>
                                </entity>
                            </fetch>", masterGUID);

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

        public String UpdateMasterEntity(IOrganizationService CrmService, Entity entity, String authData1, String authData2)
        {
            String RetCode = String.Empty;
            try
            {
                entity["new_authorizedsubdata1"] = authData1;
                entity["new_authorizedsubdata2"] = authData2;
                CrmService.Update(entity);
                RetCode = "Successful";
            }
            catch (Exception ex)
            {
                RetCode = "FAILED TO UPDATE MASTER ENTITY";
            }
            return RetCode;
        }

    }
}
