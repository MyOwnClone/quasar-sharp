using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GruntXProductions.Quasar.Assembler.Scanner;

namespace GruntXProductions.Quasar.Assembler.Parser
{
    public partial class Parser
    {
        private List<string> errors = new List<string>();
        private List<object> instructions = new List<object>();
        private IList<AbstractToken> tokens;
        private int position = 0;
        public IList<object> Output
        {
            get
            {
                return this.instructions;
            }
        }

        public void ProcessTokens(IList<AbstractToken> tokens)
        {
            position = 0;
            this.tokens = tokens;
            while (position < tokens.Count)
            {
                while (position < tokens.Count && (peekToken() is TokenEOL)) readToken();
                if (position < tokens.Count)
                {
                    if (peekToken(1) is TokenColon && peekToken(0) is TokenIdentifier)
                        parseLabel();
					else if (peekToken() is TokenHash)
					{
						readToken();
						parseSingleDirective();
					}
                    else
                        parseSingleInstruction();
                }
            }
			string globalScope = "";
			List<object> newInstructions = new List<object>();
			foreach(object o in this.instructions)
			{
				if(o is Instruction)	
				{
					Instruction ins = o as Instruction;
					foreach(AbstractToken token in ins.Operands)
					{
						if(token is TokenIdentifier)
						{
							TokenIdentifier ident = token as TokenIdentifier;
							if(ident.Value.StartsWith("."))
								ident.Value = globalScope + ident.Value;
						}
					}
				}
				else if (o is Label)
				{
					Label l = o as Label;
					if(l.Name.StartsWith("."))
						l.Name = globalScope + l.Name;
					else
						globalScope = l.Name;
				}
				if (o is PreprocessorDirective)
				{
					PreprocessorDirective dir = o as PreprocessorDirective;
					switch(dir.Name)
					{
					case "include":
						newInstructions.AddRange(parseInclude(dir));
						break;
					}
				}
				else
					newInstructions.Add(o);
			}
			
			this.instructions = newInstructions;
        }
		
		private List<object> parseInclude(PreprocessorDirective dir)
		{
			if(dir.Operands.Count != 1 && !(dir.Operands[0] is TokenStringLiteral))
				return new List<object>();
			else
			{
				TokenStringLiteral sl = dir.Operands[0] as TokenStringLiteral;
				string source = File.ReadAllText(sl.Value);
				Lexer lex = new Lexer(source);
				lex.Scan();
				Parser parser = new Parser();
				parser.ProcessTokens(lex.TokenList);
				return parser.instructions;
			}
		}
		
        private void parseLabel()
        {
            TokenIdentifier ident = readToken() as TokenIdentifier;
            readToken();
            this.instructions.Add(new Label(ident.Value));
        }

        private void parseSingleInstruction()
        {
            List<AbstractToken> list = new List<AbstractToken>();
            while (position < tokens.Count && !(peekToken() is TokenEOL))
                list.Add(readToken());
            if (list.Count == 0)
                return;
            if (list[0] is TokenIdentifier)
            {
                string name = list[0].ToString();
                List<AbstractToken> operands = new List<AbstractToken>();
                
                for (int pos = 1; pos < list.Count; pos++ )
                {
                    if (pos % 2 != 0)
                        operands.Add(list[pos]);
                    else if (!(list[pos] is TokenComma))
                        CreateError("Comma or EOL expected!");
                }
                this.instructions.Add(new Instruction(list[0].Line, name, operands));
            }
            else
                CreateError("Mnemonic expected!");
        }
		
		private void parseSingleDirective()
        {
            List<AbstractToken> list = new List<AbstractToken>();
            while (position < tokens.Count && !(peekToken() is TokenEOL))
                list.Add(readToken());
            if (list.Count == 0)
                return;
            if (list[0] is TokenIdentifier)
            {
                string name = list[0].ToString();
                List<AbstractToken> operands = new List<AbstractToken>();
                
                for (int pos = 1; pos < list.Count; pos++ )
                {
                    if (pos % 2 != 0)
                        operands.Add(list[pos]);
                    else if (!(list[pos] is TokenComma))
                        CreateError("Comma or EOL expected!");
                }
                this.instructions.Add(new PreprocessorDirective(list[0].Line, name, operands));
            }
            else
                CreateError("Mnemonic expected!");
        }
		
        public void CreateError(string format, params string[] args)
        {
            Console.Error.WriteLine(format, args);
            errors.Add(String.Format(format, args));
        }

        private AbstractToken readToken()
        {
            return tokens[position++];
        }

        private AbstractToken peekToken()
        {
            return tokens[position];
        }

        private AbstractToken peekToken(int pos)
        {
            if (position + pos < tokens.Count)
                return tokens[position + pos];
            else
                return new TokenEOL(0);
        }
    }
}
