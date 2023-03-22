using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public partial class Lexer
    {
        public static void Sistem(string input_data)
        {
            long answer = 0; char top = '9'; int up = 10;
            input_data = input_data.ToUpper();

            type = LexemaType.INT;
            for (int i = flag; i < input_data.Length; i++)
            {
                if (input_data[i] == '%' || input_data[i] == '&' || input_data[i] == '$')
                {
                    if (i + 1 == input_data.Length)
                    {
                        Symbols(input_data);
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
                        temp += input_data[i];
                        i++;
                    }
                }
                else if (input_data[i] == 46)
                {
                    goto ResultOut;
                }

                int comp = input_data[i] - '0';
                int border = top - '0';
                if (comp > border)
                {
                    error = true;
                    temp += input_data[i];
                }
                else if (((input_data[i] >= '0') && (input_data[i] <= '9')) || ((input_data[i] >= 'A') && (input_data[i] <= 'F')))
                {
                    if ((input_data[i] >= 'A') && (input_data[i] <= 'F'))
                    {
                        comp = input_data[i] - 'A' + 10;
                    }
                    temp += input_data[i];
                    answer = (answer * up) + comp;
                    value = answer.ToString();
                }
            }
            ResultOut:
            value = answer.ToString();
            if (answer > 2147483647)
            {
                throw new Except(line_number, symbol_number, "Range check error while evaluating constants");
            }
            if (error)
            {
                throw new Except(line_number, symbol_number, "Invalid format");
            }
            Result();
            return;
        }
    }
}
