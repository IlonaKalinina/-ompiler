using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public partial class Lexer
    {
        public static void Real(string input_data)
        {
            type = LexemaType.REAL;

            int doteCount = 0;
            int eCount = 0;
            int countWholePart = 0;

            string mantisa = null;
            string exp = null;

            bool startExp = false;

            for (int i = flag; i < input_data.Length; i++)
            {
                if ((input_data[i] >= '0' && input_data[i] <= '9') || (input_data[i] == '.'))
                {
                    if (startExp) 
                        exp += input_data[i];

                    if (input_data[i] == '.')
                    {
                        doteCount++;
                        if (i + 1 < input_data.Length && input_data[i + 1] == '.')
                        {
                            temp += input_data[i];
                            temp += input_data[i + 1];
                            value = temp;
                            type = LexemaType.SEPARATOR;
                            Result();
                            return;
                        }
                        countWholePart += temp.Length;
                    }

                    temp += input_data[i];

                    if (i == input_data.Length - 1 || (i + 1 < input_data.Length && input_data[i + 1] == ' '))
                    {
                        if (mantisa != null)
                        {
                            while (mantisa.Length < 17 + countWholePart)
                            {
                                mantisa += '0';
                            }
                        }

                        if (startExp)
                        {
                            value = mantisa + 'E' + exp;
                        }
                        else
                        {
                            value = temp;
                        }

                        if (doteCount > 1 || eCount > 1)
                        {
                            Error($"({line_number}, {symbol_number}) Syntax error");
                            return;
                        }
                        Result();
                        return;
                    }
                }
                else if (input_data[i] == 'e' || input_data[i] == 'E')
                {
                    startExp = true;
                    mantisa = temp;
                    temp += input_data[i];
                    eCount++;

                    if (i + 1 < input_data.Length && (input_data[i + 1] == '-' || input_data[i + 1] == '+'))
                    {
                        exp += input_data[i + 1];
                        temp += input_data[i + 1];
                        i++;
                    }
                }
                else
                {
                    while (mantisa.Length < 17 + countWholePart)
                    {
                        mantisa += '0';
                    }

                    if (startExp)
                    {
                        value = mantisa + 'E' + exp;
                    }
                    else
                    {
                        value = temp;
                    }

                    if (doteCount > 1 || eCount > 1)
                    {
                        Error($"({line_number}, {symbol_number}) Syntax error");
                        return;
                    }
                    Result();
                    return;
                }
            }
        }
    }
}
