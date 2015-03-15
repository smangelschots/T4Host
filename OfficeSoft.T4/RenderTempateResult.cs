using System;
using System.CodeDom.Compiler;

namespace OfficeSoft.T4
{
    public class RenderTempateResult
    {

        public CompilerErrorCollection CompilerErrors { get; set; }
        public Exception Exception { get; set; }
        public bool IsRendered { get; set; }
        public bool IsSaved { get; set; }
        public bool HasErrors { get; set; }
        public string FileName { get; set; }
        public string Text { get; set; }


    }
}