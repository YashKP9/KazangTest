using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Kazang.Workflow
{
    public class Utils
    {
        public Utils() { }

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
