using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    class SymbolsFind
    {
        public static char[] separators = { ',', '.', '(', ')', '[', ']', ':', ';', '@', '{', '}', '^', '#', '$', '&', '%' };
        public static char[] operators = { '+', '-', '*', '/', '<', '>', ':', '=' };
        public static void Symbols(string input_data)
        {
            for (int i = Lexer.indicator; i < input_data.Length; i++)
            {
                foreach (char c in operators)
                {
                    if (input_data[i] == c)
                    {
                        Lexer.category = "operators";
                        if (i + 1 < input_data.Length)
                        {
                            if (
                                (input_data[i] == '<' && (input_data[i + 1] == '>'  || input_data[i + 1] == '='   || input_data[i + 1] == '<')) ||
                                (input_data[i] == '>' && (input_data[i + 1] == '>'  || input_data[i + 1] == '='   || input_data[i + 1] == '<')) ||
                                (input_data[i] == '*' && (input_data[i + 1] == '*'  || input_data[i + 1] == '=')) ||
                                (input_data[i] == '+' &&  input_data[i + 1] == '=') ||
                                (input_data[i] == '-' &&  input_data[i + 1] == '=') ||
                                (input_data[i] == '/' &&  input_data[i + 1] == '=') ||
                                (input_data[i] == ':' &&  input_data[i + 1] == '=')
                                )
                            {
                                Lexer.temp = input_data[i].ToString() + input_data[i + 1];
                                Lexer.meaning = Lexer.temp;
                                ResultOut.Result();
                                return;
                            }
                        }
                        Lexer.temp = input_data[i].ToString();
                        Lexer.meaning = Lexer.temp;
                        ResultOut.Result();
                        return;
                    }
                }
                foreach (char c in separators)
                {
                    if (input_data[i] == c)
                    {
                        Lexer.category = "separators";
                        Lexer.temp = input_data[i].ToString();
                        Lexer.meaning = Lexer.temp;
                        ResultOut.Result();
                        return;
                    }
                }

            }
        }
    }
}
