using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CompilerPascal.Lexer
{
    public class Lexer
    {
        public static List<Lexema> lexems = new List<Lexema>();

        public static string input_data;
        public static string temp;
        public static string category;
        public static string meaning;

        public static int first_symbol;
        public static int line_number;
        public static int indicator;
        public static int value;

        public static bool startComment;
        public static bool startOneLineComment;
        public static bool strEnd;
        public static bool error;

        public readonly StreamReader readFile;

        public static Lexema foundlexem = new Lexema();

        public Lexer(string fileName)
        {
            readFile = new StreamReader(fileName);
            input_data = readFile.ReadLine();

            foundlexem = null;
            temp       = null;
            category   = null;
            meaning    = null;

            first_symbol = 1;
            line_number  = 1;

            indicator = 0;
            value     = 0;

            startComment        = false;
            startOneLineComment = false;

            strEnd = true;
            error  = false;
        }

        public Lexema GetLexem()
        {
            CommentNotEnd:
            if (indicator + 1 > input_data.Length)
            {
                input_data = readFile.ReadLine();
                indicator = 0;
                first_symbol = 1;
                line_number++;

                if (input_data == null)
                {
                    category = "End File";
                    ResultOut.Result();
                    return foundlexem;
                }
            }
            CheckSymbol.CheckFirstSymbol(input_data);

            if (startComment)
            {
                while (startComment)
                {
                    goto CommentNotEnd;
                }
            }
            return foundlexem;
        }
    }
}
