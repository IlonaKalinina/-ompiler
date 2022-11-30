using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    internal class CommentsFind
    {
        public static void Comments(string input_data)
        {
            Lexer.startComment = true;
            Lexer.category = "comments";
            for (int i = Lexer.indicator; i < input_data.Length; i++)
            {
                Lexer.temp += input_data[i];
                if (input_data[i] == '}')
                {
                    Lexer.startComment = false;
                    ResultOut.Result();
                    return;
                }
                else if (i == input_data.Length - 1)
                {
                    Lexer.indicator += Lexer.temp.Length;
                    Lexer.temp = null;
                }
            }
            return;
        }
    }
}
