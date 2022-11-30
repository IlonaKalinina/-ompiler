using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    class CheckSymbol
    {
        public static void CheckFirstSymbol(string input_data)
        {
            if (Lexer.startComment)
            {
                CommentsFind.Comments(input_data);
                return;
            }
            if (input_data[Lexer.indicator] == ' ') Lexer.indicator++;
            while (input_data[Lexer.indicator] == '\t')
            {
                Lexer.indicator++;
            }
            if (Lexer.indicator < input_data.Length)
            {
                if (input_data[Lexer.indicator] >= '0' && input_data[Lexer.indicator] <= '9')
                {
                    IntegerFindLex.Integer(input_data);
                }
                else if (((input_data[Lexer.indicator] >= 'A') && (input_data[Lexer.indicator] <= 'Z')) || ((input_data[Lexer.indicator] >= 'a') && (input_data[Lexer.indicator] <= 'z')) || input_data[Lexer.indicator] == '_')
                {
                    IdentifierFindLex.Identifier(input_data);
                }
                else if (input_data[Lexer.indicator] == '%' || input_data[Lexer.indicator] == '&' || input_data[Lexer.indicator] == '$')
                {
                    NumSistemsConv.Sistem(input_data);
                }
                else if (input_data[Lexer.indicator] == '\'')
                {
                    StringFindLex.String(input_data);
                }
                else if (input_data[Lexer.indicator] == '/')
                {
                    if (Lexer.indicator + 1 < input_data.Length)
                    {
                        if (input_data[Lexer.indicator] == '/' && input_data[Lexer.indicator + 1] == '/')
                        {
                            OneLineCommentsVoid.OneLineComments(input_data);
                        }
                        else
                        {
                            SymbolsFind.Symbols(input_data);
                        }
                    }
                    else
                    {
                        SymbolsFind.Symbols(input_data);
                    }
                }
                else if (input_data[Lexer.indicator] == '{')
                {
                    Lexer.startComment = true;
                    CommentsFind.Comments(input_data);
                }
                else if (input_data[Lexer.indicator] == '#' && Lexer.indicator + 1 < input_data.Length && input_data[Lexer.indicator + 1] >= '0' && input_data[Lexer.indicator + 1] <= '9')
                {
                    CharFindLex.Char(input_data);
                }
                else
                {
                    SymbolsFind.Symbols(input_data);
                }
            }
        }
    }
}
