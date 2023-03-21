using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public partial class Lexer
    {
        public static void Identifier(string input_data)
        {
            type = LexemaType.IDENTIFIER;
            for (int i = flag; i < input_data.Length; i++)
            {
                if (((input_data[i] >= 'A') && (input_data[i] <= 'Z')) || ((input_data[i] >= 'a') && (input_data[i] <= 'z')) || (input_data[i] == '_') || (input_data[i] >= '0' && input_data[i] <= '9'))
                {
                    temp += input_data[i];
                    if (i == input_data.Length - 1)
                    {
                        value = temp;
                        if (temp.Length > 127)
                        {
                            Error($"({line_number}, {symbol_number}) Identifier exceeds length");
                            return;
                        }
                        KeyWord();
                        Result();
                        return;
                    }
                }
                else
                {
                    value = temp;
                    KeyWord();
                    Result();
                    return;
                }
            }
        }
    }
}
