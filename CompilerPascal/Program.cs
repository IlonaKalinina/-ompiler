using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CompilerPascal
{
    class Program
    {
        public static bool eof = false;
        static void Main(string[] args)
        {
            try
            {
                if (args.Contains("-lex"))
                {
                //"../../../../CompilerPascal.Test/Tests/LexerTests/Files/1_1.in"
                var lexer = new Lexer.Lexer(args[0]);
                //var lexer = new Lexer.Lexer("../../../../CompilerPascal.Test/Tests/LexerTests/Files/03_2.in");

                eof = false;
                    while (!eof)
                    {
                        var lexema = lexer.GetLexem();

                        if (lexema != null)
                        {
                            if (lexema.Type != Lexer.LexemaType.ERROR && lexema.Type != Lexer.LexemaType.NONE)
                            {
                                if (lexema.Type == Lexer.LexemaType.EOF)
                                {
                                    eof = true;
                                }
                                else
                                {
                                    Console.WriteLine($"{lexema.Line_number} {lexema.Symbol_number} {lexema.Type} {lexema.Value} {lexema.Source}");
                                }
                            }
                            else if (lexema.Type == Lexer.LexemaType.ERROR)
                            {
                                Console.WriteLine($"{lexema.Value}");
                            }
                        }
                        else if (lexema != null && lexema.Type == Lexer.LexemaType.EOF)
                        {
                            eof = true;
                        }
                    }
                }
                if (args.Contains("-pars"))
                {
                   // Parser.Parser P = new Parser.Parser(args[0]);
                    List<string> answer = new List<string>();
                    //RunTree(P.doParse);
                }
            }
            catch
            {
                Console.WriteLine("File not found");
            }
        }

        static List<string> answerList = new List<string>();
        static List<bool> openBranch = new List<bool>();

        static string lineAnswer = null;
        static string lineBuf = null;

        static int numNode = 0;
        static int numChildren = -1;
        static void RunTree(Parser.Node doParse)
        {
            lineBuf = null;
            for (int i = 0; i < numNode; i++)
            {
                if (i == numNode - 1)
                {
                    if (numChildren == 0)
                    {
                        lineBuf += "├───";
                        openBranch.Add(true);
                    }
                    else if (numChildren == 1)
                    {
                        lineBuf += "└───";
                        openBranch[i] = false;
                    }
                }
                else
                {
                   if (openBranch[i])
                   {
                       lineBuf += "│   ";
                   }
                   else
                   {
                        lineBuf += "    ";
                   }
                }
            }
            lineAnswer = lineBuf + doParse.value;
            Console.WriteLine(lineAnswer);
            answerList.Add(lineAnswer);

            if (doParse.children != null)
            {
                numNode++;
                numChildren = 0;

                RunTree(doParse.children[0]);
                numChildren = -1;

                if (doParse.children.Count > 1)
                {
                    numChildren = 1;

                    RunTree(doParse.children[1]);
                    numChildren = -1;
                }
                numNode--;
            }
            return;
        }
    }
}
