using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    class OneLineCommentsVoid
    {
        public static void OneLineComments(string input_data)
        {
            Lexer.startOneLineComment = true;
            Lexer.category = "comments";
            for (int i = Lexer.indicator; i < input_data.Length; i++)
            {
                if (i == input_data.Length - 1)
                {
                    Lexer.temp += input_data[i];
                    ResultOut.Result();
                    return;
                }
                else
                {
                    Lexer.temp += input_data[i];
                }
            }
        }
    }
}
