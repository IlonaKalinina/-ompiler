using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    class RealFindLex
    {
        public static void Real(string input_data)
        {
            Lexer.category = "real";

            int doteCount = 0;
            int eCount = 0;
            int countWholePart = 0;

            string mantisa = null;
            string exp = null;

            bool startExp = false;

            for (int i = Lexer.indicator; i < input_data.Length; i++)
            {
                if ((input_data[i] >= '0' && input_data[i] <= '9') || (input_data[i] == '.'))
                {
                    if (startExp) 
                        exp += input_data[i];

                    if (input_data[i] == '.')
                    {
                        doteCount++;
                        countWholePart += Lexer.temp.Length;
                    }

                    Lexer.temp += input_data[i];

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
                            Lexer.meaning = mantisa + 'E' + exp;
                        }
                        else
                        {
                            Lexer.meaning = Lexer.temp;
                        }

                        if (doteCount > 1 || eCount > 1)
                        {
                            ExepError.Error(5);
                            return;
                        }
                        ResultOut.Result();
                        return;
                    }
                }
                else if (input_data[i] == 'e' || input_data[i] == 'E')
                {
                    startExp = true;
                    mantisa = Lexer.temp;
                    Lexer.temp += input_data[i];
                    eCount++;

                    if (i + 1 < input_data.Length && (input_data[i + 1] == '-' || input_data[i + 1] == '+'))
                    {
                        exp += input_data[i + 1];
                        Lexer.temp += input_data[i + 1];
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
                        Lexer.meaning = mantisa + 'E' + exp;
                    }
                    else
                    {
                        Lexer.meaning = Lexer.temp;
                    }

                    if (doteCount > 1 || eCount > 1)
                    {
                        ExepError.Error(5);
                        return;
                    }
                    ResultOut.Result();
                    return;
                }
            }
        }
    }
}
