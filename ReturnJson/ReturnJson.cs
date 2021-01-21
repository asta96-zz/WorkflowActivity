using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using ReturnJson.Helper;
using ReturnJson.Model;
using ReturnJson.BLogic;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnJson
{
    public class ReturnJson : CodeActivity
    {
        public IOrganizationService _service;
        public IOrganizationServiceFactory _serviceFactory;
        public IWorkflowContext _context;

        [RequiredArgument]
        [Input("TargetId")]        
        public InArgument<string> TargetId { get; set; }
        [RequiredArgument]
        [Input("EntityLogicalName")]
        public InArgument<string> EntityLogicalName { get; set; }
        [RequiredArgument]
        [Output("JsonResponse")]
        public OutArgument<string> JsonResponse { get; set; }
        

        public ITracingService _tracing;
        protected override void Execute(CodeActivityContext executionContext)
        {
            _context = executionContext.GetExtension<IWorkflowContext>();
            _serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            _service = _serviceFactory.CreateOrganizationService(_context.UserId);
            _tracing = executionContext.GetExtension<ITracingService>();
            Logic logic = new Logic(_service);
            try
            {
                AuditChanges auditChanges = new AuditChanges() { AuditHistories = logic.RetrieveRecordChangeHistory(new EntityReference(EntityLogicalName.Get(executionContext).Trim(), Guid.Parse(TargetId.Get(executionContext).Trim())))};
                var Output = Serializer.SerializeToJson(auditChanges);
                JsonResponse.Set(executionContext, Output);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message+Environment.NewLine+ex.StackTrace, ex);
            }
        }
    }
}
