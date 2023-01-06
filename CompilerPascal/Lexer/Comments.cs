using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    public partial class Lexer
    {
        public static void Comments(string input_data)
        {
            startComment = true;
            for (int i = flag; i < input_data.Length; i++)
            {
                temp += input_data[i];
                if (input_data[i] == '}')
                {
                    startComment = false;
                    Result();
                    return;
                }
                else if (i == input_data.Length - 1)
                {
                    flag += temp.Length;
                    temp = null;
                }
            }
            return;
        }
    }
}
