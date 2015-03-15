using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TextTemplating;

namespace OfficeSoft.T4
{
    [Serializable]
    public class TemplateHost : ITextTemplatingEngineHost, ITextTemplatingSessionHost
    {

        ITextTemplatingSession session = new TextTemplatingSession();
        readonly IList<string> _standardAssemblyReferences;

        private string _fileExtensionValue = ".txt";
        public string FileExtension
        {
            get { return _fileExtensionValue; }
        }

        private CompilerErrorCollection _errorsValue;
        public CompilerErrorCollection Errors
        {
            get { return _errorsValue; }
        }

        private Encoding fileEncodingValue = Encoding.UTF8;
        private string _templateFile;

        public TemplateHost()
        {
            _standardAssemblyReferences = new List<string>();
            _standardAssemblyReferences.Add(typeof (System.Uri).Assembly.Location);

        }

        public Encoding FileEncoding
        {
            get { return fileEncodingValue; }
        }

        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            content = System.String.Empty;
            location = System.String.Empty;

            if (File.Exists(requestFileName))
            {
                content = File.ReadAllText(requestFileName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ResolveAssemblyReference(string assemblyReference)
        {
            if (File.Exists(assemblyReference))
            {
                return assemblyReference;
            }
            if (TemplateFile != null)
            {
                string candidate = Path.Combine(Path.GetDirectoryName(path: TemplateFile), assemblyReference);
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }
            return string.Empty;
        }

        public Type ResolveDirectiveProcessor(string processorName)
        {
            if (string.Compare(processorName, "XYZ", StringComparison.OrdinalIgnoreCase) == 0)
            {
               // return typeof();
            }
            throw new Exception("Directive Processor not found");
        }

        public string ResolvePath(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("the file name cannot be null");
            }
            if (File.Exists(fileName))
            {
                return fileName;
            }
            string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), fileName);
            if (File.Exists(candidate))
            {
                return candidate;
            }
            return fileName;

        }

        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            if (directiveId == null)
            {
                throw new ArgumentNullException("the directiveId cannot be null");
            }
            if (processorName == null)
            {
                throw new ArgumentNullException("the processorName cannot be null");
            }
            if (parameterName == null)
            {
                throw new ArgumentNullException("the parameterName cannot be null");
            }
            return String.Empty;
        }

        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            return AppDomain.CreateDomain("Generation App Domain");
        }

        public void LogErrors(CompilerErrorCollection errors)
        {
            _errorsValue = errors;
        }

        public void SetFileExtension(string extension)
        {
            _fileExtensionValue = extension;
        }

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            fileEncodingValue = encoding;

        }

        public object GetHostOption(string optionName)
        {
            object returnObject;
            switch (optionName)
            {
                case "CacheAssemblies":
                    returnObject = true;
                    break;
                default:
                    returnObject = null;
                    break;
            }
            return returnObject;
        }

        public IList<string> StandardAssemblyReferences
        {
            get
            {
             
                return _standardAssemblyReferences;
            }
        }

        public IList<string> StandardImports
        {
            get
            {

                return new string[]
                {
                    "System",
                    "System.Runtime.Serialization"

                };
                
            }         
        }

        public string TemplateFile
        {
            get { return _templateFile; }
            set { _templateFile = value; }
        }

        public ITextTemplatingSession CreateSession()
        {
            return session;
        }

        public ITextTemplatingSession Session { get; set; }
    }
}
