using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI.EditingAPI.Editors
{
    public class ToggleConfiguration 
    {
        public string TrueLabel { get; set; }
        public string FalseLabel { get; set; }
        /// <summary>
        /// When value is true in the toggle value data, indicates editors with ids that should be hidden
        /// </summary>
        public string[] EditorIdsToHideOnTrue { get; set; }
        /// <summary>
        /// When value is false in the toggle value data, indicates editors with ids that should be hidden
        /// </summary>
        public string[] EditorIdsToHideOnFalse { get; set; }
    }
}
