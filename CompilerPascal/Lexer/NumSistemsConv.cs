using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    class NumSistemsConv
    {
        public static void Sistem(string input_data)
        {
            long answer = 0; char top = '9'; int up = 10;
            input_data = input_data.ToUpper();

            Lexer.category = "integer";
            for (int i = Lexer.indicator; i < input_data.Length; i++)
            {
                if (input_data[i] == '%' || input_data[i] == '&' || input_data[i] == '$')
                {
                    if (i + 1 == input_data.Length)
                    {
                        Lexer.temp += input_data[i];
                        ExepError.Error(1);
                        return;
                    }
                    else
                    {
                        if (input_data[i] == '%')
                        {
                            top = '1';
                            up = 2;
                        }
                        if (input_data[i] == '&')
                        {
                            top = '7';
                            up = 8;
                        }
                        if (input_data[i] == '$')
                        {
                            top = 'F';
                            up = 16;
                        }
                        Lexer.temp += input_data[i];
                        i++;
                    }
                }
                else if (input_data[i] == 46)
                {
                    ExepError.Error(1);
                    return;
                }

                int comp = input_data[i] - '0';
                int border = top - '0';
                if (comp > border)
                {
                    Lexer.error = true;
                    Lexer.temp += input_data[i];
                }
                else if (((input_data[i] >= '0') && (input_data[i] <= '9')) || ((input_data[i] >= 'A') && (input_data[i] <= 'F')))
                {
                    if ((input_data[i] >= 'A') && (input_data[i] <= 'F'))
                    {
                        comp = input_data[i] - 'A' + 10;
                    }
                    Lexer.temp += input_data[i];
                    answer = (answer * up) + comp;
                    Lexer.meaning = answer.ToString();
                }
            }
            Lexer.meaning = answer.ToString();
            if (answer > 2147483647)
            {
                ExepError.Error(2);
                return;
            }
            if (Lexer.error)
            {
                ExepError.Error(1);
                return;
            }
            ResultOut.Result();
            return;
        }
    }
}
