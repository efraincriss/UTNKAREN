//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Web;

//namespace com.cpp.calypso.web.DbContext
//{

//    /// <summary>
//    /// Artic:
//    /// 
//    /// Entity Framework Core: History / Audit table
//    /// https://www.meziantou.net/2017/08/14/entity-framework-core-history-audit-table
//    /// </summary>
//    public class Audit
//    {
//        public int Id { get; set; }
//        public string TableName { get; set; }

//        public DateTime DateTime { get; set; }

//        public string KeyValues { get; set; }
//        public string OldValues { get; set; }
//        public string NewValues { get; set; }
//    }

//    public class AuditEntry
//    {
//        public AuditEntry(DbEntityEntry entry)
//        {
//            Entry = entry;
//        }

//        public DbEntityEntry Entry { get; }
//        public string TableName { get; set; }

//        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
//        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
//        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();

//        //public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

//        //public bool HasTemporaryProperties => TemporaryProperties.Any();

//        public Audit ToAudit()
//        {
//            var audit = new Audit();
//            audit.TableName = TableName;
//            audit.DateTime = DateTime.UtcNow;
//            audit.KeyValues = JsonConvert.SerializeObject(KeyValues);
//            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
//            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
//            return audit;
//        }
//    }

//    public class AuditoriaDBContext: DbContext
//    {

//        //var objectContext = ((IObjectContextAdapter)this).ObjectContext;
//        //objectContext.DetectChanges();
//        //var changes = objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added 
//        //    | EntityState.Modified | EntityState.Deleted);


//        var auditEntries = OnBeforeSaveChanges();

//        private List<com.cpp.calypso.web.DbContext.AuditEntry> OnBeforeSaveChanges()
//        {
//            ChangeTracker.DetectChanges();

//            var auditEntries = new List<com.cpp.calypso.web.DbContext.AuditEntry>();

//            foreach (var entry in ChangeTracker.Entries())
//            {
//                if (entry.Entity is Audit
//                    || entry.State == EntityState.Detached
//                    || entry.State == EntityState.Unchanged)
//                    continue;

//                var auditEntry = new com.cpp.calypso.web.DbContext.AuditEntry(entry);
//                //auditEntry.TableName = entry.Metadata.Relational().TableName;
//                auditEntry.TableName = entry.GetType().Name;
//                auditEntries.Add(auditEntry);

//                switch (entry.State)
//                {
//                    case EntityState.Added:

//                        foreach (var prop in entry.CurrentValues.PropertyNames)
//                        {
//                            var currentValue = entry.CurrentValues[prop];
//                            if (currentValue != null)
//                                auditEntry.NewValues[prop] = currentValue.ToString();

//                        }

//                        break;

//                    case EntityState.Deleted:

//                        foreach (var prop in entry.OriginalValues.PropertyNames)
//                        {
//                            var originalValue = entry.OriginalValues[prop];
//                            if (originalValue != null)
//                                auditEntry.OldValues[prop] = originalValue.ToString();
//                        }


//                        break;

//                    case EntityState.Modified:
//                        foreach (var prop in entry.OriginalValues.PropertyNames)
//                        {

//                            var originalValueModified = entry.OriginalValues[prop];
//                            var currentValueModified = entry.CurrentValues[prop];

//                            //if (property.IsModified)
//                            //{
//                            if (originalValueModified != null)
//                                auditEntry.OldValues[prop] = originalValueModified.ToString();

//                            if (currentValueModified != null)
//                                auditEntry.NewValues[prop] = currentValueModified.ToString();

//                            //}
//                        }

//                        break;
//                }



//            }

//            return auditEntries;
//        }

//    }
//}