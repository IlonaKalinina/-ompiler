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

            Lexer.foundlexem = new Lexema() { 
                numberLine      = Lexer.line_number, 
                numberSymbol    = Lexer.first_symbol, 
                categoryLexeme  = Lexer.category, 
                valueLexema     = Lexer.meaning, 
                initialLexema   = Lexer.temp 
            };

            if (Lexer.temp != null) Lexer.indicator += Lexer.temp.Length;
            Lexer.temp = null;
            Lexer.meaning = null;
            Lexer.category = null;
        }
    }
}
