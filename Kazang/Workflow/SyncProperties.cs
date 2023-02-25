using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;

namespace Kazang.Workflow
{

    public sealed class SyncProperties : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> Text { get; set; }

        [Input("MasterEntityID")]
        [ReferenceTarget("new_masterentity")]
        public InArgument<EntityReference> MasterEntityID { get; set; }

        [Input("SubEntityID")]
        [ReferenceTarget("new_subentity")]
        public InArgument<EntityReference> SubEntityID { get; set; }

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

            IWorkflowContext workflowContext = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService _service = serviceFactory.CreateOrganizationService(workflowContext.UserId);

            var tracingService = executionContext.GetExtension<ITracingService>();
            tracingService.Trace("SyncProperties Initiated.");

            MasterEntityManager mem = new MasterEntityManager();
            PropertiesManager pm = new PropertiesManager();

            try
            {
                EntityCollection entProperties = mem.RetrieveProperties(memID.Id, _service);
                if (entProperties != null)
                {
                    RetLog = RetLog + "Properties Count: " + entProperties.Entities.Count;
                }
                else
                {
                    RetLog = RetLog + "No properties found. ";
                }

                foreach (Entity entProperty in entProperties.Entities)
                {
                    Entity newSubProperty = pm.MapPropertiesParameters(entProperty, subID.Id);
                    _service.Create(newSubProperty);
                    RetLog = RetLog + "Property Created: " + entProperty["new_name"];
                }
            }
            catch (Exception ex)
            { RetLog = RetLog + "Critical Error" + ex.Message; }

            this.Result.Set(executionContext, RetLog);
        }
    }
}
