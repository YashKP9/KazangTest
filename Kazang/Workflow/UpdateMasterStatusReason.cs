using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;

namespace Kazang.Workflow
{

    public sealed class UpdateMasterStatusReason : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> Text { get; set; }

        [Output("Result")]
        public OutArgument<String> Result { get; set; }

        [Input("MasterEntityID")]
        [ReferenceTarget("new_masterentity")]
        public InArgument<EntityReference> MasterEntityID { get; set; }

        [Input("SubEntityID")]
        [ReferenceTarget("new_subentity")]
        public InArgument<EntityReference> SubEntityID { get; set; }

        // This Code Activity Runs on Trigger of Authorize and (Post) onCreate of SubEntity
        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext executionContext)
        {
            String RetLog = String.Empty;
            // Obtain the runtime value of the Text input argument
            string text = executionContext.GetValue(this.Text);
            EntityReference memID = executionContext.GetValue(this.MasterEntityID);
            EntityReference subID = executionContext.GetValue(this.SubEntityID);

            IWorkflowContext workflowContext = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService _service = serviceFactory.CreateOrganizationService(workflowContext.UserId);

            MasterEntityManager mem = new MasterEntityManager();
            PropertiesManager pm = new PropertiesManager();
            SubEntityManager sm =  new SubEntityManager();

            try
            {
                bool unAuth = false; 
                EntityCollection ecSubEntities = sm.RetrieveSubEntities(memID.Id, _service);
                if (ecSubEntities != null)
                {
                    foreach (Entity sub in ecSubEntities.Entities)
                    {
                        if (sub.Attributes.Contains("new_authorize"))
                        {
                            if (sub["new_authorize"] == null || (bool)sub["new_authorize"] == false)
                            {
                                unAuth = true;
                                RetLog = RetLog + "new_authorize==null || new_authorize==false";
                            }
                            else
                            {
                                RetLog = RetLog + "new_authorize==true";
                            }

                        }
                        else { unAuth = true; }
                    }
                    if (unAuth)
                    {
                        Entity master = mem.RetrieveMasterEntity(memID.Id, _service);
                        if (master != null)
                        {
                            String Log = mem.UpdateMasterEntity(_service, master, String.Empty, String.Empty, "StatusReason");
                            RetLog = RetLog + "\nStatus Reason Updated Successful on Master: " + Log;
                        }
                    }
                }
                else { RetLog = RetLog + "\nNo Sub Entities Found."; }

            }
            catch (Exception ex)
            { RetLog = RetLog + "Critical Error: " + ex.Message; }


            this.Result.Set(executionContext, RetLog);

        }
    }
}
