using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    public partial class Lexer
    {
        public static char[] separators = { ',', '.', '(', ')', '[', ']', ':', ';', '@', '{', '}', '^', '#', '$', '&', '%' };
        public static char[] operators = { '+', '-', '*', '/', '<', '>', ':', '='};
        public static void Symbols(string input_data)
        {
            for (int i = flag; i < input_data.Length; i++)
            {
                foreach (char c in operators)
                {
                    if (input_data[i] == c)
                    {
                        type = LexemaType.OPERATOR;
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
                                temp = input_data[i].ToString() + input_data[i + 1];
                                value = temp;
                                Result();
                                return;
                            }
                        }
                        temp = input_data[i].ToString();
                        value = temp;
                        Result();
                        return;
                    }
                }
                foreach (char c in separators)
                {
                    if (input_data[i] == c)
                    {
                        type = LexemaType.SEPARATOR;
                        temp = input_data[i].ToString();
                        value = temp;
                        Result();
                        return;
                    }
                }

            }
        }
    }
}
