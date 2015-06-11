using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedSearch
{
        
    public class PsModule
    {
        [Cmdlet(VerbsCommon.Show, "AdvancedSearchAddon")]
        public class ShowISEMarkdownExtension : PSCmdlet
        {
            protected override void BeginProcessing()
            {
                try
                {
                    base.BeginProcessing();
                    if (ISEControls.PowerShellTab.VerticalAddOnTools.Single(x => x.Name == Strings.addonName) != null)
                    {
                        ISEControls.PowerShellTab.VerticalAddOnTools.Single(x => x.Name == Strings.addonName).IsVisible = true;
                       
                    }
                }
                catch
                {
                    ISEControls.PowerShellTab.VerticalAddOnTools.Add(Strings.addonName, typeof(AdvancedSearch.AdvancedSearchControl), true);
                }

        }
        [Cmdlet(VerbsData.Import, "AdvancedSearchAddon")]
        public class ImportISEMarkdownExtension : PSCmdlet
        {
            protected override void BeginProcessing()
            {
               try{
                    ISEControls.PowerShellTab.VerticalAddOnTools.Add(Strings.addonName, typeof(AdvancedSearch.AdvancedSearchControl), false);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
