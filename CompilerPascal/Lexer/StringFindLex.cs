using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    class StringFindLex
    {
        public static void String(string input_data)
        {
            Lexer.category = "string";
            int id_save = 0;
            if (Lexer.temp != null)
            {
                Lexer.indicator += Lexer.temp.Length;
                id_save = Lexer.temp.Length;
            }
            for (int i = Lexer.indicator; i < input_data.Length; i++)
            {
                if (i == Lexer.indicator && i + 1 < input_data.Length && input_data[i] == '\'' && input_data[i + 1] == '\'')
                {
                    if (i + 2 < input_data.Length && input_data[i + 2] == '\'')
                    {
                        Lexer.temp += input_data[i];
                        Lexer.temp += input_data[i + 1];
                        Lexer.temp += input_data[i + 2];
                        Lexer.meaning += "\'";
                        i += 2;
                    }
                    else
                    {
                        Lexer.temp += input_data[i];
                        Lexer.temp += input_data[i + 1];
                        Lexer.meaning = " ";
                        ResultOut.Result();
                        return;
                    }
                }
                else if (i + 1 < input_data.Length && input_data[i] == '\'' && input_data[i + 1] == '\'')
                {
                    Lexer.temp += input_data[i];
                    Lexer.temp += input_data[i + 1];
                    Lexer.meaning += "\'";
                    i++;
                }
                else
                {
                    if (input_data[i] == '\'')
                    {
                        Lexer.temp += input_data[i];
                        if (Lexer.meaning != null)
                        {
                            if (i + 1 < input_data.Length - 1 && input_data[i + 1] == '#')
                            {
                                CharFindLex.Char(input_data);
                                return;
                            }
                            else if (!Lexer.strEnd)
                            {
                                Lexer.strEnd = true;
                            }
                            else
                            {
                                Lexer.indicator -= id_save;
                                ResultOut.Result();
                                return;
                            }
                        }
                    }
                    else
                    {
                        Lexer.meaning += input_data[i];
                        Lexer.temp += input_data[i];

                        if (i == input_data.Length - 1)
                        {
                            ExepError.Error(3);
                            return;
                        }
                    }
                }
            }
        }
    }
}
