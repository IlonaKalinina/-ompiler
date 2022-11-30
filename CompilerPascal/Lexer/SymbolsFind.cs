using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    class SymbolsFind
    {
        public static char[] separators = { ',', '.', '(', ')', '[', ']', ':', ';', '@', '{', '}', '_', '^', '#' };
        public static char[] operators = { '+', '-', '*', '/', '<', '>', ':', '=' };
        public static void Symbols(string input_data)
        {
            for (int i = Lexer.indicator; i < input_data.Length; i++)
            {
                foreach (char c in operators)
                {
                    if (input_data[i] == c)
                    {
                        if (i + 1 < input_data.Length)
                        {
                            if (input_data[i] == ':' && input_data[i + 1] == '=')
                            {
                                Lexer.temp = input_data[i].ToString() + input_data[i + 1];
                                Lexer.meaning = Lexer.temp;
                                Lexer.category = "operators";
                                ResultOut.Result();
                                return;
                            }
                            if (input_data[i] == '<' && (input_data[i + 1] == '>' || input_data[i + 1] == '='))
                            {
                                Lexer.temp = input_data[i].ToString() + input_data[i + 1];
                                Lexer.meaning = Lexer.temp;
                                Lexer.category = "operators";
                                ResultOut.Result();
                                return;
                            }
                            else
                            if (input_data[i] == '>' && input_data[i + 1] == '=')
                            {
                                Lexer.temp = input_data[i].ToString() + input_data[i + 1];
                                Lexer.meaning = Lexer.temp;
                                Lexer.category = "operators";
                                ResultOut.Result();
                                return;
                            }
                        }
                        Lexer.temp = input_data[i].ToString();
                        Lexer.meaning = Lexer.temp;
                        Lexer.category = "operators";
                        ResultOut.Result();
                        return;
                    }
                }
                foreach (char c in separators)
                {
                    if (input_data[i] == c)
                    {
                        Lexer.temp = input_data[i].ToString();
                        Lexer.meaning = Lexer.temp;
                        Lexer.category = "separators";
                        ResultOut.Result();
                        return;
                    }
                }

            }
        }
    }
}
