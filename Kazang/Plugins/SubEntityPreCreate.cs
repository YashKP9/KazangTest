// <copyright file="SubEntityPreCreate.cs" company="">
// </copyright>
// <author>YASH PILLAY</author>
// <date>27/2/2023 1:22:00 AM</date>
// <summary>Implements the SubEntityPreCreate Plugin.</summary>
namespace Kazang.Plugins
{
    using System;
    using System.ServiceModel;
    using Microsoft.Xrm.Sdk;
    using System.Linq;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Data;
    using System.Configuration;

    /// <summary>
    /// SubEntityPreCreate Plugin.
    /// </summary>    
    public class SubEntityPreCreate : Plugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubEntityPreCreate"/> class.
        /// </summary>
        public SubEntityPreCreate()
            : base(typeof(SubEntityPreCreate))
        {
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(20, "Create", "new_subentity", new Action<LocalPluginContext>(ExecuteSubEntityPreCreate)));

            // Note : you can register for more events here if this plugin is not specific to an individual entity and message combination.
            // You may also need to update your RegisterFile.crmregister plug-in registration file to reflect any change.
        }

        /// <summary>
        /// Executes the plug-in. IN ORDER TO MAINTAIN DATA INTEGRITY, PREVENT COMMIT TO DB ON PRE-EVENT, 
        /// SO THAT ONLY 1 UNAUTHORIZED RECORD IS ATTACHED TO MASTER
        /// </summary>
        /// <param name="localContext">The <see cref="LocalPluginContext"/> which contains the
        /// <see cref="IPluginExecutionContext"/>,
        /// <see cref="IOrganizationService"/>
        /// and <see cref="ITracingService"/>
        /// </param>
        protected void ExecuteSubEntityPreCreate(LocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new ArgumentNullException("localContext");
            }
           

            IPluginExecutionContext context = localContext.PluginExecutionContext;
            IOrganizationService _service = localContext.OrganizationService;

            Entity entity = (Entity)context.InputParameters["Target"];

            String RetLog = String.Empty;
            // The InputParameters collection contains all the data passed in the message request.
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                if (entity.LogicalName == "new_subentity")
                {
                    SubEntityManager sem = new SubEntityManager();
                    bool currUnAuth = entity.Attributes.Contains("new_authorize") && (entity["new_authorize"] == null || (bool)entity["new_authorize"] == false) ? true : (!entity.Attributes.Contains("new_authorize") ? true: false);
                    if (entity.Attributes.Contains("new_master") && currUnAuth)
                    {
                        Guid guidMaster = ((EntityReference)entity["new_master"]).Id;
                        EntityCollection ecSubEnts = sem.RetrieveSubEntities(guidMaster, _service);
                        foreach (Entity sub in ecSubEnts.Entities)
                        {
                            if (sub.Attributes.Contains("new_authorize"))
                            {
                                if (sub["new_authorize"] == null || (bool)sub["new_authorize"] == false)
                                {
                                    throw new InvalidPluginExecutionException("Not Permitted: Only 1 Unauthorized Sub Entity may exist at a given time.");
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
