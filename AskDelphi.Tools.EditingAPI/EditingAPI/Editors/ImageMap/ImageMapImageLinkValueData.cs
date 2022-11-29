using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI.EditingAPI.Editors.ImageMap
{
    /// <summary>
    /// Contains information
    /// </summary>
    public class ImageMapImageLinkValueData : SingleTopicChooserValueData
    {
        /// <summary>
        /// Indicates whether image link is existing. Legacy support
        /// </summary>
        public bool IsExisting { get; set; }
    }
}
