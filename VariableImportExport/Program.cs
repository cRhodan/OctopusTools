using System;
using System.IO;
using System.Linq;
using CommandLine;
using Newtonsoft.Json;
using Octopus.Client;
using Octopus.Client.Model;

namespace VariableImportExport
{
    class Program
    {
        private static readonly string[] ExportValues = {"export", "e"};
        private static readonly string[] ImportValues = {"import", "i"};
        
        private static string FullPathToFile { get; set; }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    FullPathToFile = $"{o.SuppliedFilePath}\\{o.SuppliedFileName}";
                    Console.WriteLine($"Using {FullPathToFile} as File Path.");
                    
                    var endpoint = new OctopusServerEndpoint(o.OctopusUrl, o.OctopusApiKey);
                    var repository = new OctopusRepository(endpoint);

                    var project = repository.Projects.FindByName(o.OctopusProjectName);
                    var set = repository.VariableSets.Get(project.VariableSetId);

                    var caseInsensitiveSwitch = o.ImportExportSwitch.ToLower();

                    if (ExportValues.Contains(caseInsensitiveSwitch))
                    {
                        HandleExport(set);
                    }

                    else if (ImportValues.Contains(caseInsensitiveSwitch))
                    {
                        HandleImport(set, repository);
                    }

                    else Console.WriteLine("Invalid switch option provided. Value must be either export (e) or import (i).");
                });
        }

        private static void HandleExport(VariableSetResource variableSet)
        {
            Console.WriteLine("Beginning Export");
            File.WriteAllText(FullPathToFile, JsonConvert.SerializeObject(variableSet, Formatting.Indented));
            Console.WriteLine("Export Complete");
        }

        private static void HandleImport(VariableSetResource variableSet, OctopusRepository repository)
        {
            Console.WriteLine("Beginning Import");
            
            var set = JsonConvert.DeserializeObject<VariableSetResource>(File.ReadAllText(FullPathToFile));

            set.Id = variableSet.Id; 
            set.OwnerId = variableSet.OwnerId;
            set.Version = variableSet.Version;
            set.Links = variableSet.Links;

            // Update the variable back into Octopus
            repository.VariableSets.Modify(set);
            
            Console.WriteLine("Import Complete");
        }
    }
}