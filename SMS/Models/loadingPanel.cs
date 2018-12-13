using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class loadingPanel
    {
        public bool PanelLoading { get; set; }
        public string PanelMainMessage { get; set; }
        public string PanelSubMessage { get; set; }
        public bool PanelCloseCommand { get; set; }
    }
}
