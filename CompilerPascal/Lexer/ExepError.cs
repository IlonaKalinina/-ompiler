using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    class ExepError
    {
        public static void Error(int codeError)
        {
            Lexer.category = "error";
            Lexer.first_symbol = Lexer.indicator + 1;

            string textError;
            switch (codeError)
            {
                case 1:
                    textError = $"Invalid format on line {Lexer.line_number}";
                    break;
                case 2:
                    textError = $"Range check error while evaluating constants on line {Lexer.line_number}";
                    break;
                case 3:
                    textError = $"String exceeds line on {Lexer.line_number} line";
                    break;
                case 4:
                    textError = $"Illegal char constant on line {Lexer.line_number}";
                    break;
                case 5:
                    textError = $"Syntax error on line {Lexer.line_number}";
                    break;
                default:
                    textError = $"Fatal Error {Lexer.line_number}";
                    break;
            }
            Lexer.foundlexem = new Lexema()
            {
                numberLine = Lexer.line_number,
                numberSymbol = Lexer.first_symbol,
                categoryLexeme = Lexer.category,
                valueLexema = textError,
                initialLexema = Lexer.temp
            };
            Lexer.indicator += Lexer.temp.Length;
            Lexer.temp = null;
            Lexer.meaning = null;
            Lexer.category = null;
        }
    }
}