using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CompilerPascal.Lexer
{
    public partial class Lexer
    {
        public static List<Lexema> lexems = new List<Lexema>();

        public static string input_data;
        public static string temp;
        public static LexemaType type;
        public static string value;

        public static int symbol_number;
        public static int line_number;
        public static int flag;
        public static int valueInt;

        public static bool startComment;
        public static bool startString;
        public static bool startOneLineComment;
        public static bool strEnd;
        public static bool error;

        public readonly StreamReader readFile;

        public static Lexema foundlexem = new Lexema(line_number, symbol_number, type, value, input_data);

        public Lexer(string fileName)
        {
            readFile = new StreamReader(fileName);
            input_data = readFile.ReadLine();

            foundlexem = null;
            temp       = null;
            type       = LexemaType.NONE;
            value      = null;

            symbol_number = 1;
            line_number  = 1;

            flag = 0;
            valueInt     = 0;

            startComment        = false;
            startString         = false;
            startOneLineComment = false;

            strEnd = true;
            error  = false;
        }

        public Lexema GetLexem()
        {
            CommentOrStringNotEnd:
            if (input_data != null && flag + 1 > input_data.Length)
            {
                input_data = readFile.ReadLine();
                flag = 0;
                symbol_number = 1;
                line_number++;

                while (input_data == "")
                {
                    input_data = readFile.ReadLine();
                    line_number++;
                }
                if (input_data == null)
                {
                    if (startComment || startString)
                    {
                        if (startComment) 
                            Error($"({line_number}, {symbol_number}) Unexpected end of file");
                        if (startString)  
                            Error($"({line_number}, {symbol_number}) String exceeds line");

                        startComment = false;
                        return foundlexem;
                    }
                    type = LexemaType.EOF;
                    Result();
                    return foundlexem;
                }
            }else if (input_data == null)
            {
                type = LexemaType.EOF;
                Result();
                return foundlexem;
            }
            CheckSymbol(input_data);

            if (startComment || startString)
            {
                while (startComment || startString)
                {
                    goto CommentOrStringNotEnd;
                }
            }

            if (foundlexem == null)
            {
                type = LexemaType.EOF;
                Result();
                return foundlexem;
            }
            return foundlexem;
        }
    }
}
