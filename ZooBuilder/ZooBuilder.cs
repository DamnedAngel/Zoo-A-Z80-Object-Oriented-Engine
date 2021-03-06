using System;
using System.IO;
using System.Collections.Generic;
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

            [Option('i', "inputClasses", Required = false, HelpText = "Input class files to be processed.")]
            public IEnumerable<string> InputFiles { get; set; }

            [Option('o', "outputFile", Required = false, Default = "", HelpText = "Output Zoo library file.")]
            public string OutputFile { get; set; }

            [Option('d', "outputDir", Required = false, Default = ".", HelpText = "Output directory.")]
            public string OutputDir { get; set; }

            [Option('z', "zooDir", Required = false, Default = ".", HelpText = "Zoo framework directory.")]
            public string ZooDir { get; set; }

            [Option('r', "reflectionLevel", Required = false, Default = "class", HelpText = "Reflection Level (none, inheritance, namedInheritance, classStructure, namedMembers).")]
            public string ReflectionLevel { get; set; }
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
            ZBConsole.Debug(Resources.VerboseParams, String.Join(", ", options.InputFiles), options.ReflectionLevel, options.Verbose, options.Wait, options.OutputFile, options.OutputDir, options.ZooDir);

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
                ZooClass zooClass = ZooClass.getZooClass(inputFile, zoo);
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

                    var template = Template.Parse(global::ZooBuilder.Properties.Resources.zoo_class_s);
                    foreach (var zooClass in classes)
                    {
                        var result = template.Render(new { zooClass, zoo });
                        File.WriteAllText(Path.Combine(options.OutputDir, zooClass.Name + ".s"), result);
                    }

                    template = Template.Parse(global::ZooBuilder.Properties.Resources.zoo_package_asm);
                    {
                        var result = template.Render(new { classes, zoo });
                        File.WriteAllText(Path.Combine(options.OutputDir, options.OutputFile + ".asm"), result);
                    }

                    if (!ZBError.IsError)
                    {
                        ZBConsole.Print("\nPass 3 (Assembly) started.");

                        if (!ZBError.IsError)
                        {
                            ZBConsole.Print("\nPass 4 (Library building) started.");
                        }
                    }
                }
            }
        }
    }
}
