using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    class RealFindLex
    {
        public static void Real(string input_data)
        {
            int doteCount = 0;
            int eCount = 0;

            string mantisa = null;
            string exp = null;

            Lexer.category = "real";
            for (int i = Lexer.indicator; i < input_data.Length; i++)
            {
                if ((input_data[i] >= '0' && input_data[i] <= '9') || (input_data[i] == '.'))
                {
                    if (mantisa != null) exp += input_data[i];
                    Lexer.temp += input_data[i];

                    if (input_data[i] == '.') 
                    {
                        doteCount++;
                    }

                    if (i == input_data.Length - 1 || (i + 1 < input_data.Length && input_data[i + 1] == ' '))
                    {
                        Lexer.meaning = Lexer.temp;
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
                    Lexer.meaning = Lexer.temp;
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
