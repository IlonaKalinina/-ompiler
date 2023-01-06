using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    public partial class Lexer
    {
        public static void Error(string textError)
        {
            symbol_number = flag + 1;

            foundlexem = new Lexema(line_number, symbol_number, LexemaType.ERROR, textError, temp);

            if (temp != null) 
                flag += temp.Length;

            temp = null;
            value = null;
            type = LexemaType.NONE;
        }
    }
}