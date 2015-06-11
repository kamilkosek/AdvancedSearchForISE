using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.PowerShell.Host.ISE;
using System.Reflection;
using System.Windows;
namespace AdvancedSearch
{
    public static class Search
    {
        public static List<FileResult> PerformSearch(SearchOptions searchOptions)
        {
            
            List<FileResult> results = new List<FileResult>(); 
            List<string> fileList = new List<string>();
            if(searchOptions.SearchInAllOpenedScripts)
            {
                try
                {
                    fileList.AddRange(ISEControls.Files.Select(p => p.FullPath));
                    
                }
                catch
                {

                }
                
            }
            else if(searchOptions.SearchInCurrentFolder)
            {
                PowerShellTab pt = ISEControls.PowerShellTab;
                Collection<PSObject>  i = pt.InvokeSynchronous("get-location");
                PSObject pi = i[0];
                var pii = pi.BaseObject as PathInfo;
                if(pii.Provider.Name == "FileSystem")
                {
                    fileList.AddRange(Directory.EnumerateFiles(pii.Path, searchOptions.FileFilter, searchOptions.SearchInSubFolders));
                }
                else
                {
                    MessageBox.Show(String.Format("The current provider is {0} but must be FileSystem",(pii.Provider.Name)));
                }

            }
            else if(searchOptions.SearchInAllOpenedScriptsAndFolders)
            {
                foreach (string file in ISEControls.Files.Select(p => p.FullPath))
                {
                    try
                    {
                        string path = Path.GetDirectoryName(file);
                        fileList.AddRange(Directory.EnumerateFiles(path, searchOptions.FileFilter, searchOptions.SearchInSubFolders));
                    }
                    catch { }
                }
                
            }
            else if(searchOptions.SearchInSelectedScriptFolder)
            {
                try
                {
                    fileList.AddRange(Directory.EnumerateFiles(Path.GetDirectoryName(searchOptions.SelectedFile), searchOptions.FileFilter, searchOptions.SearchInSubFolders));
                }
                catch
                { }
            }

            if (searchOptions.Mode == SearchOptions.SearchMode.Regex)
            {
                
                RegexOptions mode = searchOptions.RegexMode.Aggregate(RegexOptions.None, (current, b) => current | b);
                foreach (string file in fileList)
                {
                    int lineNumber = 1;
                    FileResult result = new FileResult();
                    result.FullPath = file;
                    result.Extension = Path.GetExtension(file);

                    foreach (string line in File.ReadLines(file))
                    {
                        Match m = Regex.Match(line, searchOptions.SearchPattern,mode);
                        if (m.Success)
                        {
                            result.SearchResults.Add(new SearchResult(m.Value, lineNumber, 0));
                        }
                        lineNumber++;
                    }
                    if (result.SearchResults.Count > 0)
                        results.Add(result);
                }
            }
            if(searchOptions.Mode == SearchOptions.SearchMode.Default)
            {
                foreach (string file in fileList)
                {
                    int lineNumber = 1;
                    FileResult result = new FileResult();
                    result.FullPath = file;
                    result.Extension = Path.GetExtension(file);
                    try
                    {
                        foreach (string line in File.ReadLines(file))
                        {
                            if (StringExtensions.Contains(line, searchOptions.SearchPattern, StringComparison.OrdinalIgnoreCase))
                            {
                                result.SearchResults.Add(new SearchResult(searchOptions.SearchPattern, lineNumber, 0));
                            }
                            lineNumber++;
                        }
                    }
                    catch
                    { }
                    if (result.SearchResults.Count > 0)
                        results.Add(result);
                }
            }
            if(searchOptions.Mode == SearchOptions.SearchMode.TokenType)
            {
                foreach(string file in fileList)
                {
                    FileResult result = new FileResult();
                    result.FullPath = file;
                    result.Extension = Path.GetExtension(file);
                    
                    if (Regex.Match(Path.GetExtension(file),"ps(s|m|)1").Success)
                    {
                        string fileContent = File.ReadAllText(file);
                        Collection<PSParseError> parseErrors;
                        try
                        {
                            Collection<PSToken> tokenCollection = PSParser.Tokenize(fileContent, out parseErrors);
                            foreach (PSToken token in tokenCollection)
                            {
                                if (searchOptions.TokenTypes.Contains(token.Type) && StringExtensions.Contains(token.Content, searchOptions.SearchPattern, StringComparison.OrdinalIgnoreCase))
                                {
                                    result.SearchResults.Add(new SearchResult(token.Content,token.StartLine,token.StartColumn));
                                }
                            }
                        }
                        catch
                        {
                        }
                        if (result.SearchResults.Count > 0)
                            results.Add(result);
                            
                    }
                }
            }
            return results;
        }


    }
}
