using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI.EditingAPI.Editors.ImageMap
{
    /// <summary>
    /// Editor data containing all necessary information to render/store image map editor
    /// </summary>
    public class ImageMapValueData
    {
        /// <summary>
        /// Indicates which platform the image map is intended for e.g. desktop, mobile or etc
        /// </summary>
        public string IntendedFor { get; set; }
        /// <summary>
        /// Contains information about the image topic used for the image map
        /// </summary>
        public ImageMapImageLinkValueData ImageLink { get; set; }
        /// <summary>
        /// The areas/shapes defined/drawn for/on the image map, linked to a target topic/item
        /// </summary>
        public List<ImageMapAreaValueData> Areas { get; set; }
        /// <summary>
        /// Image map definitions for other platforms
        /// </summary>
        public List<ImageMapValueData> AlternativeMaps { get; set; }

    }
}
