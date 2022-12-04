using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    class IdentifierFindLex
    {
        public static void Identifier(string input_data)
        {
            Lexer.category = "identifier";
            for (int i = Lexer.indicator; i < input_data.Length; i++)
            {
                if (((input_data[i] >= 'A') && (input_data[i] <= 'Z')) || ((input_data[i] >= 'a') && (input_data[i] <= 'z')) || (input_data[i] == '_') || (input_data[i] >= '0' && input_data[i] <= '9'))
                {
                    Lexer.temp += input_data[i];

                    if (i == input_data.Length - 1)
                    {
                        Lexer.meaning = Lexer.temp;

                        if (Lexer.temp.Length > 127)
                        {
                            ExepError.Error(6);
                            return;
                        }

                        KeyWordFind.KeyWord();
                        ResultOut.Result();
                        return;
                    }
                }
                else
                {
                    Lexer.meaning = Lexer.temp;
                    KeyWordFind.KeyWord();
                    ResultOut.Result();
                    return;
                }
            }
        }
    }
}
