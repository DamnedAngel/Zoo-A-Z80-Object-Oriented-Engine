using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scriban;
using CommandLine;
using global::ZooBuilder.Properties;
using System.Xml.Schema;

namespace ZooBuilder
{
    class ZooBuilder
    {
        public class Options
        {
            [Option('w', "wait", Required = false, HelpText = "Wait for key before ending the program.")]
            public bool Wait { get; set; }

            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool Verbose { get; set; }

            [Option('i', "inputFile", Required = false, HelpText = "Input class files to be processed.")]
            public IEnumerable<string> InputFiles { get; set; }

            [Option('o', "outputFile", Required = false, Default = "ZooPackage", HelpText = "Output Zoo library file.")]
            public string OutputFile { get; set; }

            [Option('d', "outputDir", Required = false, Default = ".", HelpText = "Output directory.")]
            public string OutputDir { get; set; }

            [Option('z', "zooDir", Required = false, Default = ".", HelpText = "Zoo framework directory.")]
            public string ZooDir { get; set; }

            [Option('r', "reflectionLevel", Required = false, Default = "inheritance", HelpText = "Reflection Level (none, inheritance, namedInheritance, classStructure, namedMembers).")]
            public string ReflectionLevel { get; set; }

            [Option('a', "includeAncestors", Required = false, Default = false, HelpText = "Include ancestor classes in the package.")]
            public bool IncludeAncestors { get; set; }

            [Option('s', "symbolFile", Required = false, HelpText = "Additional symbol files to be .included.")]
            public IEnumerable<string> SymbolFiles { get; set; }
        }

        static Zoo zoo = new Zoo();

        static int Main(string[] args)
        {
            ZBConsole.Verbose = false;
            var result = Parser.Default.ParseArguments<Options>(args)
                .WithParsed((Options opts) => { FinalParseAndRun(opts); })
                .WithNotParsed((errs) => { errs.ToString(); })
                ;

            return (int) ZBError.Error;
        }

        static void FinalParseAndRun (Options options)
        {
            zoo.ReflectionLevelAsString = options.ReflectionLevel;

            if (!ZBError.IsError)
            {
                RunCommand(options);
            }

            if (ZBError.IsError)
            {
                Console.WriteLine(Resources.ZooBuildFail, (int)ZBError.Error, ZBError.Error.ToString());
            }

            if (options.Wait)
            {
                ZBConsole.Print("Press any key...");
                while (!Console.KeyAvailable) { }
                Console.ReadKey(true);
            }
        }

        static void RunCommand(Options options)
        {
            ZBConsole.Verbose = options.Verbose;
            zoo.Path = options.ZooDir;
            zoo.OutputPath = options.OutputDir;
            zoo.OutputFile = options.OutputFile;
            zoo.IncludeAncestors = options.IncludeAncestors;
            ZBConsole.Debug(Resources.VerboseParams, String.Join(", ", options.InputFiles), options.ReflectionLevel, options.Verbose, options.Wait, options.OutputFile, options.OutputDir, options.ZooDir, options.IncludeAncestors, String.Join(", ", options.SymbolFiles));

            /*
            Console.WriteLine("only change access time:        {0}", IfTrue(TouchOptions.OnlyAccessTime));
            Console.WriteLine("only change modification time:  {0}", IfTrue(TouchOptions.OnlyModificationTime));
            Console.WriteLine("do not create any files:        {0}", IfTrue(TouchOptions.DontCreate));

            Console.WriteLine("files:");
            Console.WriteLine("=>" + String.Join(",", TouchOptions.Filenames.Select(_ => "\"" + _ + "\"")));
            */

            ZBConsole.Print("Pass 1 (Parsing) started.");

            foreach (var inputFile in options.InputFiles)
            {
                ZooClass.NewZooClassFromFile (inputFile, zoo);
                if (ZBError.IsError)
                { break; }
            }

            if (!ZBError.IsError)
            {
                ZBConsole.Print("\nPass 2 (Code Generation) started.");

                if (!ZBError.IsError)
                {
                    // Parse a scriban template
                    /*
                    foreach (var zooClass in ZooClass.zooClasses.Values)
                    {
                        var result = template.Render(new { zooClass, zoo });
                        File.WriteAllText(Path.Combine(options.OutputDir, zooClass.Name + ".s"), result);
                        if (ZBError.IsError)
                        { break; }
                    }
                    */
                    ICollection<ZooClass> classes = ZooClass.zooClasses.Values;

                    ZBConsole.Print("Generating package...");
                    var template = Template.Parse(global::ZooBuilder.Properties.Resources.zoo_package_s);
                    var result = template.Render(new { classes, zoo });
                    File.WriteAllText(Path.Combine(options.OutputDir, options.OutputFile + ".s"), result);
                    ZBConsole.Print("  DONE!");

                    foreach (var zooClass in classes)
                    {
                        if (zooClass.IsInputFile || zoo.IncludeAncestors)
                        {
                            ZBConsole.Print("Generating public symbol listing...");
                            template = Template.Parse(global::ZooBuilder.Properties.Resources.zoo_class_public_s);
                            result = template.Render(new { zooClass, zoo });
                            File.WriteAllText(Path.Combine(options.OutputDir, zooClass.Name + ".public.txt"), result);
                            ZBConsole.Print("  DONE!");

                            ZBConsole.Print("Generating protected symbol listing...");
                            template = Template.Parse(global::ZooBuilder.Properties.Resources.zoo_class_protected_s);
                            result = template.Render(new { zooClass, zoo });
                            File.WriteAllText(Path.Combine(options.OutputDir, zooClass.Name + ".protected.txt"), result);
                            ZBConsole.Print("  DONE!");
                        }
                    }

                    ZBConsole.Print("Zoo package generated.\n");
                }
            }
            ZBConsole.Print("ZooBuilder ended.");
        }
    }
}
