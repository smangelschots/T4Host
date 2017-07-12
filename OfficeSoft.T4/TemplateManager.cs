using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;

namespace OfficeSoft.T4
{
    public class TemplateManager
    {

        private TemplateHost _host;
        private ITextTemplatingSession _session;
        public string FileExtention { get; set; }
        public Encoding FileEncoding { get; set; }
        public string Input { get; set; }
        public CompilerErrorCollection Errors { get; private set; }
        public List<string> AssemblyReferences { get; set; }


        public TemplateManager(string templateFile)
        {
            var host = new TemplateHost();
            _session = host.CreateSession();
            host.TemplateFile = templateFile;
            AssemblyReferences = new List<string>();
            Input = File.ReadAllText(templateFile);
            _host = host;
        }

        public void SetParameter(string name, object property)
        {
            if (_session.ContainsKey(name))
                _session.Remove(name);

            _session[name] = property;
        }

        public void SetParameters(Dictionary<string, object> values)
        {
            foreach (var value in values)
            {
                SetParameter(value.Key, value.Value);
            }
        }

        public RenderTempateResult RenderTemplateToFile(string path = "", bool overwrite = false,
            IList<string> standardAssemblyReferences = null)
        {
            var result = new RenderTempateResult();
            try
            {


                Engine engine = new Engine();
                _host.Session = _session;


                if (standardAssemblyReferences != null)
                    foreach (var standardAssemblyReference in standardAssemblyReferences)
                    {
                        if (!_host.StandardAssemblyReferences.Contains(standardAssemblyReference))
                            _host.StandardAssemblyReferences.Add(standardAssemblyReference);
                    }
                foreach (var assemblyReference in AssemblyReferences)
                {
                    if (!_host.StandardAssemblyReferences.Contains(assemblyReference))
                        _host.StandardAssemblyReferences.Add(assemblyReference);
                }

                string output = engine.ProcessTemplate(Input, _host);
                Errors = _host.Errors;
                if (Errors.HasErrors)
                {

                    throw new Exception("error in processTemplate");
                }

                result.IsRendered = true;
                string outputFileName = path;


                if (string.IsNullOrEmpty(outputFileName))
                {
                    outputFileName = Path.GetFileNameWithoutExtension(_host.TemplateFile);
                    outputFileName = Path.Combine(Path.GetDirectoryName(_host.TemplateFile), outputFileName);
                    outputFileName = outputFileName + _host.FileExtension;
                }

                if (!File.Exists(outputFileName) || overwrite)
                {
                    if (File.Exists(outputFileName))
                        File.SetAttributes(outputFileName, FileAttributes.Normal);
                    File.WriteAllText(outputFileName, output, _host.FileEncoding);

                }


                result.FileName = outputFileName;
                result.Text = output;
                result.IsSaved = true;




            }
            catch (Exception exception)
            {

                result.HasErrors = true;
                result.Exception = exception;
                result.CompilerErrors = Errors;
            }

            finally
            {


            }

            return result;
        }

        public string ProjectName { get; set; }


        public static TemplateManager Create(string file)
        {
            var manager = new TemplateManager(file);
            return manager;
        }

    }
}
