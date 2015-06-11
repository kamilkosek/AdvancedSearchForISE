using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.PowerShell.Host.ISE;

namespace AdvancedSearch
{
    public class SearchOptions
    {
        public SearchOptions(ISEFileCollection f)
        {

            this.tokenTypes = new List<PSTokenType>();
            this.regexMode = new List<RegexOptions>();
            if (f.Count > 0)
            {
                this.files = new List<string>();
                this.selectedFile = f.SelectedFile.FullPath;
                foreach (ISEFile file in f)
                {
                    this.files.Add(file.FullPath);
                }
            }

        }
        private string searchPatter;

        public string SearchPattern
        {
            get { return searchPatter; }
            set { searchPatter = value; }
        }
        private bool searchInSelectedScriptFolder;

        public bool SearchInSelectedScriptFolder
        {
            get { return searchInSelectedScriptFolder; }
            set { searchInSelectedScriptFolder = value; }
        }

        private string fileFilter;

        public string FileFilter
        {
            get { return fileFilter; }
            set { fileFilter = value; }
        }
        private bool searchInAllOpenedScripts;

        public bool SearchInAllOpenedScripts
        {
            get { return searchInAllOpenedScripts; }
            set { searchInAllOpenedScripts = value; }
        }
        private bool searchInAllOpenedScriptsAndFolders;

        public bool SearchInAllOpenedScriptsAndFolders
        {
            get { return searchInAllOpenedScriptsAndFolders; }
            set { searchInAllOpenedScriptsAndFolders = value; }
        }
        private bool searchInCurrentFolder;

        public bool SearchInCurrentFolder
        {
            get { return searchInCurrentFolder; }
            set { searchInCurrentFolder = value; }
        }


        private SearchOption searchInSubFolders;

        public SearchOption SearchInSubFolders
        {
            get { return searchInSubFolders; }
            set { searchInSubFolders = value; }
        }
        public enum SearchMode
        {
            Default,
            Regex,
            TokenType
        }
        private SearchMode mode;

        public SearchMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        private List<PSTokenType> tokenTypes;

        public List<PSTokenType> TokenTypes
        {
            get { return tokenTypes; }
            set { tokenTypes = value; }
        }

        private List<RegexOptions> regexMode;

        public List<RegexOptions> RegexMode
        {
            get { return regexMode; }
            set { regexMode = value; }
        }
        private List<string> files;
        public List<string> Files
        {
            get { return files; }
        }
        private string selectedFile;
        public string SelectedFile
        {
            get { return selectedFile; }
        }
    }
}
