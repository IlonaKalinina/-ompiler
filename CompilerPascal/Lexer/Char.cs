using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    public partial class Lexer
    {
        public static void Char(string input_data)
        {
            if (type == LexemaType.NONE) type = LexemaType.CHAR;
            string ascii = null;
            int id_save = 0;
            if (temp != null)
            {
                flag += temp.Length;
                id_save = temp.Length;
            }
            for (int i = flag; i < input_data.Length; i++)
            {
                if (input_data[i] == '#' && i + 1 < input_data.Length && input_data[i + 1] != '#' && ascii == null)
                {
                    temp += input_data[i];
                }
                else if (input_data[i] == '#' && ascii != null)
                {
                    goto TryAscii;
                }
                else if (input_data[i] >= '0' && input_data[i] <= '9')
                {
                    temp += input_data[i];
                    ascii += input_data[i];
                }
                else if (((input_data[i] >= 'A') && (input_data[i] <= 'Z')) || ((input_data[i] >= 'a') && (input_data[i] <= 'z')))
                {
                    Error($"({line_number}, {symbol_number}) Illegal char constant");
                }
                else if (input_data[i] == '/' && input_data[i + 1] == '/' || input_data[i] == '\'' || input_data[i] == ' ')
                {
                    if (input_data[i] == '\'') strEnd = false;
                    goto TryAscii;
                }
                else
                {
                    Error($"({line_number}, {symbol_number}) Syntax error");
                }
            }
        TryAscii:
            try
            {
                flag -= id_save;
                int unicode = int.Parse(ascii);
                value += Convert.ToChar(unicode);
                if (strEnd)
                {
                    Result();
                    return;
                }
                else
                {
                    String(input_data);
                    return;
                }
            }
            catch
            {
                Error($"({line_number}, {symbol_number}) Illegal char constant");
            }
        }
    }
}
