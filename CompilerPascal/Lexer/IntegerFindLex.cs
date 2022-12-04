using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    class IntegerFindLex
    {
        public static void Integer(string input_data)
        {
            Lexer.category = "integer";
            bool insignZeros = false;
            for (int i = Lexer.indicator; i < input_data.Length; i++)
            {
                if (input_data[i] == '.' && i + 1 < input_data.Length && input_data[i + 1] == '.')
                {
                    ResultOut.Result();
                    return;
                }
                else if (input_data[i] == '.')
                {
                    Lexer.temp = null;
                    RealFindLex.Real(input_data);
                    return;
                }

                if ((input_data[i] >= '0' && input_data[i] <= '9') || (input_data[i] == '.'))
                {
                    if (Lexer.temp == null && input_data[i] == '0')
                    {
                        Lexer.temp += input_data[i];
                        insignZeros = true;
                        continue;
                    } 
                    if (input_data[i] > '0' )
                    {
                        insignZeros = false;
                    }
                    if (insignZeros)
                    {
                        Lexer.temp += input_data[i];

                        if (i + 1 == input_data.Length && Lexer.meaning == null)
                        {
                            Lexer.meaning = "0";
                            ResultOut.Result();
                            return;
                        }
                        continue;
                    }

                    Lexer.temp += input_data[i];
                    Lexer.meaning += input_data[i];

                    if (i == input_data.Length - 1 || (i + 1 < input_data.Length && input_data[i + 1] == ' '))
                    {
                        if (Lexer.error)
                        {
                            ExepError.Error(1);
                            return;
                        }
                        bool success = int.TryParse(Lexer.meaning, out Lexer.value);
                        if (success)
                        {
                            ResultOut.Result();
                            return;
                        }
                        else
                        {
                            ExepError.Error(2);
                            return;
                        }
                    }
                }
                else
                {
                    Lexer.meaning = Lexer.temp;

                    bool success = int.TryParse(Lexer.meaning, out Lexer.value);
                    if (success)
                    {
                        ResultOut.Result();
                        return;
                    }
                    else
                    {
                        ExepError.Error(2);
                        return;
                    }
                }
            }
        }
    }
}
