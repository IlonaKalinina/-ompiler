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
            if (args.Length == 0)
            {
                Console.WriteLine("Options:");
                Console.WriteLine("  -help    Display help");
                return;
            }
            if (args.Contains("-help"))
            {
                Console.WriteLine("Use the command:");
                Console.WriteLine("  dotnet run ..\\CompilerPascal.Test\\Tests\\LexerTests\\Files\\{file name} -key");
                Console.WriteLine("Key:");
                Console.WriteLine("  -l     Lexical parser");
                Console.WriteLine("  -sp    Simple expression parser");
                Console.WriteLine("  -p     Syntax analyzer)");
                Console.WriteLine("  -sa    Semantic analysis");
                return;
            }
            //string filePath = "../../../../CompilerPascal.Test/Tests/ParserTests/Files/01_02_input.txt";
            try
            {
                Lexer lexer = new Lexer(args[0]);

                if (args[1] == "-l")
                {
                    while (!eof)
                    {
                        var lexema = lexer.GetLexem();

                        if (lexema != null)
                        {
                            if (lexema.Type != LexemaType.ERROR && lexema.Type != LexemaType.NONE)
                            {
                                if (lexema.Type == LexemaType.EOF)
                                {
                                    eof = true;
                                }
                                else
                                {
                                    Console.WriteLine($"{lexema.Line_number} {lexema.Symbol_number} {lexema.Type} {lexema.Value} {lexema.Source}");
                                }
                            }
                            else if (lexema.Type == LexemaType.ERROR)
                            {
                                Console.WriteLine($"{lexema.Value}");
                            }
                        }
                        else if (lexema != null && lexema.Type == LexemaType.EOF)
                        {
                            eof = true;
                        }
                    }
                }
                if (args[1] == "-sp")
                {
                }
                if (args[1] == "-par")
                {
                    try
                    {
                        Parser parser = new Parser(lexer);
                        Node firstNode = parser.ParseMainProgram();
                        Console.WriteLine(firstNode.ToString(null));
                    }
                    catch (Except ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        throw new Except(lexer.GetLexem().Line_number, lexer.GetLexem().Symbol_number - 1, ex.Message);
                    }
                }
                if (args[1] == "-sa")
                {
                }
            }
            catch (Except e)
            {
                Console.Write($"{e}\r\n");
            }
        }
    }
}
