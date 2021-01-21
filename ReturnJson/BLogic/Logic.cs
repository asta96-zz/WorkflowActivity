using System;
using System.Collections.Generic;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using ReturnJson.Model;

namespace ReturnJson.BLogic
{
   public class Logic
    {
        private static IOrganizationService crmseviceClient;
        public Logic(IOrganizationService service)
        {
            crmseviceClient = service;
        }
      
        public List<AuditHistory> RetrieveRecordChangeHistory(EntityReference target)
        {
            AuditHistoryManager manager = new AuditHistoryManager(crmseviceClient);
            List<AuditHistory> auditHistoryForRecord = new List<AuditHistory>();
            RetrieveRecordChangeHistoryRequest changeRequest = new RetrieveRecordChangeHistoryRequest();
            changeRequest.Target = target;
                //new EntityReference("account", new Guid("b1f68180-4c5a-eb11-bb23-000d3a0a74cb"));
            RetrieveRecordChangeHistoryResponse changeResponse = (RetrieveRecordChangeHistoryResponse)crmseviceClient.Execute(changeRequest);
            AuditDetailCollection details = changeResponse.AuditDetailCollection;
            foreach (AuditDetail detail in details.AuditDetails)
            {
                List<AuditHistory> records = manager.GetRecordChanges(detail);
                auditHistoryForRecord.AddRange(records);
            }
            return auditHistoryForRecord;
        }
    }
}
