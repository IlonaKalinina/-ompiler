using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    class CharFindLex
    {
        public static void Char(string input_data)
        {
            if (Lexer.category == null)
                Lexer.category = "char";
            string ascii = null;
            int id_save = 0;
            if (Lexer.temp != null)
            {
                Lexer.indicator += Lexer.temp.Length;
                id_save = Lexer.temp.Length;
            }
            for (int i = Lexer.indicator; i < input_data.Length; i++)
            {
                if (input_data[i] == '#' && i + 1 < input_data.Length && input_data[i + 1] != '#' && ascii == null)
                {
                    Lexer.temp += input_data[i];
                }
                else if (input_data[i] == '#' && ascii != null)
                {
                    goto TryAscii;
                }
                else if (input_data[i] >= '0' && input_data[i] <= '9')
                {
                    Lexer.temp += input_data[i];
                    ascii += input_data[i];
                }
                else if (((input_data[i] >= 'A') && (input_data[i] <= 'Z')) || ((input_data[i] >= 'a') && (input_data[i] <= 'z')))
                {
                    ExepError.Error(4);
                }
                else if (input_data[i] == '/' && input_data[i + 1] == '/' || input_data[i] == '\'' || input_data[i] == ' ')
                {
                    if (input_data[i] == '\'') Lexer.strEnd = false;
                    goto TryAscii;
                }
                else
                {
                    ExepError.Error(5);
                }
            }
        TryAscii:
            try
            {
                Lexer.indicator -= id_save;
                int unicode = int.Parse(ascii);
                Lexer.meaning += Convert.ToChar(unicode);
                if (Lexer.strEnd)
                {
                    ResultOut.Result();
                    return;
                }
                else
                {
                    StringFindLex.String(input_data);
                    return;
                }
            }
            catch
            {
                ExepError.Error(4);
            }
        }
    }
}
