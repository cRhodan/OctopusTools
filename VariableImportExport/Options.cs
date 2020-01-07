using CommandLine;

namespace VariableImportExport
{
    public class Options
    {
        [Option('u', "url", Required = true, HelpText = "Url for your Octopus Instance")]
        public string OctopusUrl { get; set; }

        [Option('k', "apiKey", Required = true, HelpText = "Api Key for your Octopus Instance")]
        public string OctopusApiKey { get; set; }

        [Option('p', "projectName", Required = true, HelpText = "The Name of the Octopus Project")]
        public string OctopusProjectName { get; set; }

        [Option('s', "switch", Required = true, HelpText = "Set to export (e) to export variables, set to import (i) to import variables")]
        public string ImportExportSwitch { get; set; }

        [Option('f', "filePath", Required = false, HelpText = "File path for the folder where the variable json file will be. Defaults to c:\\temp")]
        public string SuppliedFilePath { get; set; } = @"C:\temp";
            
        [Option('n', "fileName", Required = false, HelpText = "File name for the variable json file, including file extension. Defaults to OctopusVariables.json")]
        public string SuppliedFileName { get; set; } = "OctopusVariables.json";
    }
}