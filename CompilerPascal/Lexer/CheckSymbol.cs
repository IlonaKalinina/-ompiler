using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public partial class Lexer
    {
        public static void CheckSymbol(string input_data)
        {
            if (startComment)
            {
                Comments(input_data);
                return;
            }
            if (startString)
            {
                String(input_data);
                return;
            }
            
            while (input_data[flag] == ' ' || input_data[flag] == '\t')
            {
                flag++;
                if (flag == input_data.Length)
                {
                    foundlexem = null;
                    return;
                }
            }
            if (flag < input_data.Length)
            {
                if (input_data[flag] >= '0' && input_data[flag] <= '9')
                {
                    Integer(input_data);
                }
                else if (((input_data[flag] >= 'A') && (input_data[flag] <= 'Z')) || ((input_data[flag] >= 'a') && (input_data[flag] <= 'z')) || input_data[flag] == '_')
                {
                    Identifier(input_data);
                }
                else if (input_data[flag] == '%' || input_data[flag] == '&' || input_data[flag] == '$')
                {
                    Sistem(input_data);
                }
                else if (input_data[flag] == '\'')
                {
                    String(input_data);
                }
                else if (input_data[flag] == '/')
                {
                    if (flag + 1 < input_data.Length)
                    {
                        if (input_data[flag] == '/' && input_data[flag + 1] == '/')
                        {
                            OneLineComments(input_data);
                        }
                        else
                        {
                            Symbols(input_data);
                        }
                    }
                    else
                    {
                        Symbols(input_data);
                    }
                }
                else if (input_data[flag] == '{')
                {
                    startComment = true;
                    Comments(input_data);
                }
                else if (input_data[flag] == '#' && flag + 1 < input_data.Length && input_data[flag + 1] >= '0' && input_data[flag + 1] <= '9')
                {
                    Char(input_data);
                }
                else
                {
                    Symbols(input_data);
                }
            }
        }
    }
}
