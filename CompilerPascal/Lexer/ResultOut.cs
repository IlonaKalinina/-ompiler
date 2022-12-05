using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    class ResultOut
    {
        public static void Result()
        {
            Lexer.first_symbol = Lexer.indicator + 1;

            object newValueLexema = null;

            switch (Lexer.category)
            {
                case ("integer"):
                    newValueLexema = int.Parse(Lexer.meaning);
                    break;
                case ("real"):
                    Lexer.meaning = Lexer.meaning.Replace(".", ",");
                    newValueLexema = float.Parse(Lexer.meaning);
                    break;
                default:
                    newValueLexema = Lexer.meaning;
                    break;
            }

            Lexer.foundlexem = new Lexema() { 
                numberLine      = Lexer.line_number, 
                numberSymbol    = Lexer.first_symbol, 
                categoryLexeme  = Lexer.category, 
                valueLexema     = newValueLexema, 
                initialLexema   = Lexer.temp 
            };

            if (Lexer.temp != null) Lexer.indicator += Lexer.temp.Length;
            Lexer.temp = null;
            Lexer.meaning = null;
            Lexer.category = null;
        }
    }
}
