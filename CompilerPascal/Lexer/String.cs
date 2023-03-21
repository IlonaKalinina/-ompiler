using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public partial class Lexer
    {
        public static void String(string input_data)
        {
            type = LexemaType.STRING;
            int id_save = 0;
            if (temp != null)
            {
                flag += temp.Length;
                id_save = temp.Length;
            }
            for (int i = flag; i < input_data.Length; i++)
            {
                if (i == flag && i + 1 < input_data.Length && input_data[i] == '\'' && input_data[i + 1] == '\'')
                {
                    if (i + 2 < input_data.Length && input_data[i + 2] == '\'')
                    {
                        temp += input_data[i];
                        temp += input_data[i + 1];
                        temp += input_data[i + 2];
                        if (value == null || (value != null && value.Length < 255))
                        {
                            value += "\'";
                        }
                        i += 2;
                    }
                    else
                    {
                        temp += input_data[i];
                        temp += input_data[i + 1];
                        if (value == null || (value != null && value.Length < 255))
                        {
                            value = " ";
                        }
                        if (startString)
                        {
                            startString = false;
                            Error($"({line_number}, {symbol_number}) String exceeds line");
                            return;
                        }
                        flag -= id_save;
                        Result();
                        return;
                    }
                }
                else if (i + 1 < input_data.Length && input_data[i] == '\'' && input_data[i + 1] == '\'')
                {
                   temp += input_data[i];
                   temp += input_data[i + 1];
                    if (value == null || (value != null && value.Length < 255))
                    {
                        value += "\'";
                    }
                    i++;
                }
                else
                {
                    if (input_data[i] == '\'')
                    {
                        temp += input_data[i];
                        if (value != null)
                        {
                            if (i + 1 < input_data.Length - 1 && input_data[i + 1] == '#')
                            {
                                Char(input_data);
                                return;
                            }
                            else if (!strEnd)
                            {
                                strEnd = true;
                                if (i == input_data.Length - 1)
                                {
                                    flag -= id_save;
                                    Result();
                                    return;
                                }
                            }
                            else
                            {
                                flag -= id_save;
                                if (startString)
                                {
                                    startString = false;
                                    Error($"({line_number}, {symbol_number}) String exceeds line");
                                    return;
                                }
                                Result();
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (value == null || (value != null && value.Length < 255))
                        {
                            value += input_data[i];
                        }
                        temp += input_data[i];

                        if (i == input_data.Length - 1)
                        {
                            startString = true;
                            flag += temp.Length;
                            temp = null;
                            return;
                        }
                    }
                }
            }
        }
    }
}
