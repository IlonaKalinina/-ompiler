using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public partial class Lexer
    {
        public static void Integer(string input_data)
        {
            type = LexemaType.INT;
            for (int i = flag; i < input_data.Length; i++)
            {
                if (input_data[i] == '.' && i + 1 < input_data.Length && input_data[i + 1] == '.')
                {
                    Result();
                    return;
                }
                else if (input_data[i] == '.')
                {
                    temp = null;
                    Real(input_data);
                    return;
                }

                if ((input_data[i] >= '0' && input_data[i] <= '9') || (input_data[i] == '.'))
                {
                    temp += input_data[i];
                    value += input_data[i];

                    if (i == input_data.Length - 1 || (i + 1 < input_data.Length && input_data[i + 1] == ' '))
                    {
                        CheckRange();
                        return;
                    }
                }
                else
                {
                    CheckRange();
                    return;
                }
            }
        }

        public static void CheckRange()
        {
            bool success = int.TryParse(value, out valueInt);
            if (success)
            {
                Result();
                return;
            }
            else
            {
                throw new Except(line_number, symbol_number, "Range check error while evaluating constants");
            }
        }
    }
}
