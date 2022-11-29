using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI.EditingAPI
{
    public class TopicListEntryIdentifier
    {
        public Guid TopicGuid { get; set; }
        public bool IsEditable { get; set; }
        public bool IsLocked { get; set; }
        public DateTime LastModificationDate { get; set; }
        public string Title { get; set; }
        public Guid TopicTypeKey { get; set; }
        public string TopicTypeName { get; set; }
        public string TopicTypeNamespace { get; set; }
        public string LockedByUserName { get; set; }
        public string ThumbnailImageBase64 { get; set; }
    }
}
