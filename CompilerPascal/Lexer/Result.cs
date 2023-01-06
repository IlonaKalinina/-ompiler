using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    public partial class Lexer
    {
        public static void Result()
        {
            symbol_number = flag + 1;

            object newValueLexema = null;

            switch (type)
            {
                case (LexemaType.INT):
                    newValueLexema = int.Parse(value);
                    break;
                case (LexemaType.REAL):
                    value = value.Replace(".", ",");
                    newValueLexema = float.Parse(value);
                    break;
                default:
                    newValueLexema = value;
                    break;
            }
            foundlexem = new Lexema(line_number, symbol_number, type, newValueLexema, temp);

            if (temp != null) flag += temp.Length;
            temp = null;
            value = null;
            type = LexemaType.NONE;
        }
    }
}
