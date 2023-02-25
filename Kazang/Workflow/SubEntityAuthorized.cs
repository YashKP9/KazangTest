using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;


namespace Kazang.Workflow
{

    public sealed class SubEntityAuthorized : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> Text { get; set; }

        [Input("MasterEntityID")]
        [ReferenceTarget("new_masterentity")]
        public InArgument<EntityReference> MasterEntityID { get; set; }

        [Input("SubEntityID")]
        [ReferenceTarget("new_subentity")]
        public InArgument<EntityReference> SubEntityID { get; set; }

        [RequiredArgument]
        [Input("Authorized")]
        public InArgument<bool> Authorized { get; set; }

        [Input("AuthorizedSubdata1")]
        public InArgument<string> AuthorizedSubdata1 { get; set; }

        [Input("AuthorizedSubdata2")]
        public InArgument<string> AuthorizedSubdata2 { get; set; }

        [Output("Result")]
        public OutArgument<String> Result { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext executionContext)
        {
            String RetLog = String.Empty;
            // Obtain the runtime value of the Text input argument
            string text = executionContext.GetValue(this.Text);
            EntityReference memID = executionContext.GetValue(this.MasterEntityID);
            EntityReference subID = executionContext.GetValue(this.SubEntityID);
            bool authorized = executionContext.GetValue(this.Authorized);

            string authData1 = executionContext.GetValue(this.AuthorizedSubdata1);
            string authData2 = executionContext.GetValue(this.AuthorizedSubdata2);

            IWorkflowContext workflowContext = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService _service = serviceFactory.CreateOrganizationService(workflowContext.UserId);

            MasterEntityManager mem = new MasterEntityManager();
            PropertiesManager pm = new PropertiesManager();

            try
            {
                if (authorized)
                {
                    Entity master = mem.RetrieveMasterEntity(memID.Id, _service);
                    if (master != null)
                    {
                        RetLog = mem.UpdateMasterEntity(_service, master, authData1, authData2);
                    }
                    else { RetLog = RetLog + "Master Not Found"; }


                    EntityCollection entProperties = mem.RetrieveProperties(subID.Id, _service, "new_subentity");
                    if (entProperties != null)
                    {
                        RetLog = RetLog + "Properties Count: " + entProperties.Entities.Count;
                    }
                    else
                    {
                        RetLog = RetLog + "No properties found. ";
                    }

                    foreach (Entity entSubProperty in entProperties.Entities)
                    {
                        Entity masterProperty = pm.FindPropertyonMaster(entSubProperty, memID.Id, _service);
                        if (masterProperty != null)
                        {
                            RetLog = RetLog + "Master Properties Found";
                            RetLog = RetLog + "\nProperty Value: " + masterProperty["new_propertyvalue"];

                            if (entSubProperty.Attributes.Contains("new_propertyvalue") && !String.IsNullOrEmpty(entSubProperty["new_propertyvalue"].ToString()))
                            {
                                masterProperty = pm.MapMasterPropertiesParameters(entSubProperty, masterProperty);
                                _service.Update(masterProperty);
                                RetLog = RetLog + "Master Property Updated";
                            }
                        }
                        else
                        {
                            RetLog = RetLog + "No Master properties Found. ";
                            //Question 7: Create New Property on Master:
                            Entity newMasterProperty = pm.MapPropertiesParameters(entSubProperty, memID.Id, "new_masterentity");
                            _service.Create(newMasterProperty);
                            RetLog = RetLog + "\nAdded Property from Sub to Master. ";
                        }

                    }
                }
            }
            catch (Exception ex)
            { RetLog = RetLog + "Critical Error" + ex.Message; }

            this.Result.Set(executionContext, RetLog);
        }
    }
}
