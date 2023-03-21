using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public partial class Lexer
    {
        public static void OneLineComments(string input_data)
        {
            startOneLineComment = true;
            for (int i = flag; i < input_data.Length; i++)
            {
                if (i == input_data.Length - 1)
                {
                    temp += input_data[i];
                    Result();
                    return;
                }
                else
                {
                    temp += input_data[i];
                }
            }
        }
    }
}
