using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReturnJson.Model
{
    [DataContract]
    public class AuditHistory
    {
        [DataMember]
        public Guid RecordId { get; set; }
        [DataMember]
        public Guid AuditId { get; set; }
        [DataMember]
        public int EntityId { get; set; }
        [DataMember]
        public int ActionId { get; set; }
        [DataMember]
        public int OperationId { get; set; }
        [DataMember]
        public string OldValue { get; set; }
        [DataMember]
        public string NewValue { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public string EntityName { get; set; }
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public string AttributeName { get; set; }
        [DataMember]
        public string RecordKeyValue { get; set; }
        [DataMember]
        public string Action { get; set; }
        [DataMember]
        public string Operation { get; set; }
        [DataMember]
        public string Username { get; set; }
    }
    [DataContract]
    public class AuditChanges
    {
        [DataMember]
        public List<AuditHistory> AuditHistories { get; set; }
    }
}
