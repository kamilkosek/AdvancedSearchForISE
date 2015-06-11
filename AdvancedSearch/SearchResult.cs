using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace AdvancedSearch
{
    public class SearchResult
    {
        public SearchResult(string Result, int StartLine, int StartColumn)
        {
            this.result = Result;
            this.startLine = StartLine;
            this.startColumn = StartColumn;
            //this.endLine = EndLine;
            //this.endColumn = EndColumn;
        }
        private string result;

        public string Result
        {
            get { return result; }
        }
        private int startLine;

        public int StartLine
        {
            get { return startLine; }
        }
        private int startColumn;

        public int StartColumn
        {
            get { return startColumn; }
        }
        private PSTokenType tokenType;

        public PSTokenType TokenType
        {
            get { return tokenType; }
            set { tokenType = value; }
        }
        

    }
}
