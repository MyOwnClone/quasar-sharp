using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GruntXProductions.Quasar;
using GruntXProductions.Quasar.Assembler.Scanner;
using GruntXProductions.Quasar.Assembler.Parser;
using GruntXProductions.Quasar.Assembler.Assemblers;

namespace GruntXProductions.Quasar.Assembler
{
    class Program
    {

        static Program()
        {
            CodeGenerator.RegisterAssembler(new MovAssembler());
            CodeGenerator.RegisterAssembler(new SwpAssembler());
            CodeGenerator.RegisterAssembler(new JmpAssembler());
            CodeGenerator.RegisterAssembler(new BsrAssembler());
            CodeGenerator.RegisterAssembler(new SurfAssembler());
            CodeGenerator.RegisterAssembler(new SsrfAssembler());
            CodeGenerator.RegisterAssembler(new SivtAssembler());
            CodeGenerator.RegisterAssembler(new SpdrAssembler());
            CodeGenerator.RegisterAssembler(new SctlAssembler());
            CodeGenerator.RegisterAssembler(new LsrfAssembler());
            CodeGenerator.RegisterAssembler(new LurfAssembler());
            CodeGenerator.RegisterAssembler(new LivtAssembler());
            CodeGenerator.RegisterAssembler(new LpdrAssembler());
            CodeGenerator.RegisterAssembler(new LctlAssembler());
            CodeGenerator.RegisterAssembler(new CmpAssembler());
            CodeGenerator.RegisterAssembler(new TstAssembler());
            CodeGenerator.RegisterAssembler(new AddAssembler());
            CodeGenerator.RegisterAssembler(new SubAssembler());
            CodeGenerator.RegisterAssembler(new MulAssembler());
            CodeGenerator.RegisterAssembler(new IMulAssembler());
            CodeGenerator.RegisterAssembler(new DivAssembler());
            CodeGenerator.RegisterAssembler(new ModAssembler());
            CodeGenerator.RegisterAssembler(new IDivAssembler());
            CodeGenerator.RegisterAssembler(new NegAssembler());
            CodeGenerator.RegisterAssembler(new NotAssembler());
            CodeGenerator.RegisterAssembler(new AndAssembler());
            CodeGenerator.RegisterAssembler(new BorAssembler());
            CodeGenerator.RegisterAssembler(new XorAssembler());
            CodeGenerator.RegisterAssembler(new ShlAssembler());
            CodeGenerator.RegisterAssembler(new ShrAssembler());
            CodeGenerator.RegisterAssembler(new SalAssembler());
            CodeGenerator.RegisterAssembler(new SarAssembler());
            CodeGenerator.RegisterAssembler(new RtnAssembler());
            CodeGenerator.RegisterAssembler(new OutAssembler());
            CodeGenerator.RegisterAssembler(new IRtnAssembler());
            CodeGenerator.RegisterAssembler(new LfpAssembler());
            CodeGenerator.RegisterAssembler(new ScAssembler());
            CodeGenerator.RegisterAssembler(new WaitAssembler());
            CodeGenerator.RegisterAssembler(new StringAssembler());
            CodeGenerator.RegisterAssembler(new DataAssembler());
            CodeGenerator.RegisterAssembler(new BytesAssembler());
            CodeGenerator.RegisterAssembler(new WordsAssembler());
            CodeGenerator.RegisterAssembler(new DwordsAssembler());
            CodeGenerator.RegisterAssembler(new ReserveAssembler());
            CodeGenerator.RegisterAssembler(new OrgAssembler());
        }

        static void Main(string[] args)
        {
            string source = null;
            string output = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--output" && i + 1 < args.Length)
                {
                    output = args[++i];
                }
                else if (args[i].StartsWith("-"))
                {
                    Console.Error.WriteLine("Unrecognized argument '{0}'!", args[i]);
                    System.Environment.Exit(-1);
                }
                else if (source == null)
                    source = args[i];
                else
                {
                    Console.Error.WriteLine("Unexpected '{0}'!", args[i]);
                    System.Environment.Exit(-1);
                }
            }
            if (source == null)
            {
                Console.Error.WriteLine("No input specified!");
                System.Environment.Exit(-1);
            }
            else if (!File.Exists(source))
            {
                Console.Error.WriteLine("File '{0}' does not exist!", source);
                System.Environment.Exit(-1);
            }
            else if (output == null)
            {
                output = Path.GetFileNameWithoutExtension(source) + ".bin";
            }

            try
            {
                Lexer lexer = new Lexer(File.ReadAllText(source));
                lexer.Scan();
                Parser.Parser parser = new Parser.Parser();
                parser.ProcessTokens(lexer.TokenList);
                BinaryFile bin = new BinaryFile();
                CodeGenerator cgen = new CodeGenerator(bin);
                cgen.Assemble(parser.Output);
                using (FileStream fs = new FileStream(output, FileMode.Create))
                {
                    bin.FinalizeExecutable();
                    bin.Generate(fs);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Fatal exception {0}", ex.Message);
                Console.Error.WriteLine("Stack trace:\n{0}", ex.StackTrace);
                System.Environment.Exit(-1);
            }
        }
    }
}
