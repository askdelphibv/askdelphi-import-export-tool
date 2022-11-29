using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI.EditingAPI
{
    /// <summary>
    /// Request options used when retrieving the topic list. Can be used to manipulate the returned set of content.
    /// </summary>
    public class PostTopicListRequest
    {
        /// <summary>
        /// If specified and non-empty the result will contain only objects that match the query.
        /// </summary>
        /// <remarks>
        /// <para>A topic matches when the query is part of the title or if the query matches (part of) the topic's unique identifier.</para>
        /// </remarks>
        public string query { get; set; }

        /// <summary>
        /// If specified and non-empty limits the topic types that can be returned.
        /// </summary>
        /// <remarks>
        /// <para>Only topics with a topic type id (in the content design) that's in the list will be returned, nothing else.</para>
        /// </remarks>
        public Guid[] topicTypes { get; set; }

        /// <summary>
        /// If specified and non-empty limits the namespaces of the returned topics.
        /// </summary>
        /// <remarks>
        /// <para>Only topics that have the specified namespace will be returned, regardless of their topic type.</para>
        /// </remarks>
        public string[] namespaces { get; set; }

        /// <summary>
        /// If specified and non-empty (where empty is also Guid.Empty) returns only topics that are part of that topic type group.
        /// </summary>
        /// <remarks>
        /// Only topics that are in the topic group
        /// </remarks>
        public Guid? topicTypeGroupId { get; set; }

        /// <summary>
        /// When specified and true, will return only topics editable under the specified ACL.
        /// </summary>
        public bool? onlyEditable { get; set; }

        /// <summary>
        /// Field to order by.
        /// </summary>
        /// <remarks>
        /// <para>One of: topictype, title, status, lastModificationDate or indexedVersion</para>
        /// </remarks>
        public string orderBy { get; set; }

        /// <summary>
        /// Filter results to only topics tagged with any one of these hierarchy nodes, identified by their unique ID in the hierarchy.
        /// </summary>
        public string[] hierarchyNodeIds { get; set; }

        /// <summary>
        /// Page number, where the first page is 1.
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// Number of items to preferably return per page.
        /// </summary>
        public int pageSize { get; set; }
    }
}
