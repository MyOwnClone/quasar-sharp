using System;
using System.Collections.Generic;

namespace GruntXProductions.Quasar.Assembler.Scanner
{
    public partial class Lexer
    {
        private int lineNumber;
        private string sourceRef;
        private int position;
        private List<AbstractToken> tokenList;

        public IList<AbstractToken> TokenList
        {
            get
            {
                return this.tokenList;
            }
        }

        public Lexer(string sourceref)
        {
            this.sourceRef = sourceref;
            this.position = 0;
            this.lineNumber = 0;
            this.tokenList = new List<AbstractToken>();
        }

        public void Scan()
        {
            while (position < sourceRef.Length)
            {
                while ((char)peekChar() != '\n' && char.IsWhiteSpace((char)peekChar())) readChar();

                if ((char)peekChar() == '\n')
                {
                    while ((char)peekChar() == '\n')
                    {
                        readChar();
                        lineNumber++;
                    }
                    tokenList.Add(new TokenEOL(lineNumber));
                }
                else if (char.IsDigit((char)peekChar()) )
                {
                    tokenList.Add(scanIntLiteral());
                }
                else if ((char)peekChar() == '\"')
                {
                    tokenList.Add(scanStringLiteral());
                }
                else if (char.IsLetter((char)peekChar()) || (char)peekChar() == '.')
                {
                    tokenList.Add(scanIdentifier());
                }
                else 
                {
                    switch ((char)peekChar())
                    {
                       
                        case '%':
                            tokenList.Add(scanRegister());
                            break;
                        case '@':
                            tokenList.Add(scanIndirectRegister());
                            break;
                        case ',':
                            readChar();
                            tokenList.Add(new TokenComma(lineNumber));
                            break;
                        case '#':
                            readChar();
                            tokenList.Add(new TokenHash(lineNumber));
                            break;
                        case ':':
                            readChar();
                            tokenList.Add(new TokenColon(lineNumber));
                            break;
                        default:
                            throw new ScanningException(String.Format("Unexpected token '{0}' at line {1}", (char)peekChar(), lineNumber));

                    }
                }

            }
        }

        private int peekChar()
        {
            return peekChar(0);
        }

        private int peekChar(int pos)
        {
            if (pos + position < sourceRef.Length)
                return (int)sourceRef[position + pos];
            else
                return -1;
        }

        private int readChar()
        {
            if (position < sourceRef.Length)
            {
                return (int)sourceRef[position++];
            }
            else
                return -1;
        }
    }
}