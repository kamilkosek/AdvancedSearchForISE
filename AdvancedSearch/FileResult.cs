using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace AdvancedSearch
{
    public class FileResult
    {
        public FileResult()
        {
            this.SearchResults = new List<SearchResult>();
        }
        private string fullPath;

        public string FullPath
        {
            get { return fullPath; }
            set { fullPath = value; }
        }
        public string FileName
        {
            get { return Path.GetFileName(fullPath); }
        }

        public string Extension { get; set; }

        public List<SearchResult> SearchResults { get; set; }

        public void GoTo(SearchResult result)
        {
            try
            {
                ISEControls.PowerShellTab.Files.Add(this.fullPath);
                ISEControls.PowerShellTab.Files.SelectedFile.Editor.SetCaretPosition(result.StartLine, 1);
                ISEControls.PowerShellTab.Files.SelectedFile.Editor.SelectCaretLine();
            }
            catch { }
        }
      
        
    }
}
