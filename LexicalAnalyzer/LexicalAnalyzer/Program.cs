using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class Program
    {
        static string[] keyWords = {"absolute", "and", "array", "as", "asm", "begin", "break", "case",  "class", "const", "constructor", "destructor",
                                    "dispinterface", "div", "do", "downto", "else", "end", "except", "exports", "file", "for", "function", "finalization",
                                    "finally", "goto", "if", "implementation", "initialization", "inline", "is", "library", "on", "out",
                                    "in", "inherited", "inline", "interface", "label", "library", "nil", "not", "object", "of", "or",
                                    "packed", "property", "procedure", "program", "record", "raise", "resourcestring", "threadvar", "try",
                                    "repeat", "set", "shl", "shr", "string", "then", "to", "type", "unit", "until", "uses", "var", "while", "with", "xor" };
        static char[] separators = { ',', '.', '(', ')', '[', ']', ':', ';', '@', '{', '}', '_', '^', '#' };
        static char[] operators = { '+', '-', '*', '/', '<', '>', ':', '=' };
        static string temp = null;
        static string category = null;
        static string meaning = null;
        static int first_symbol = 1;
        static int line_number = 1;
        static int id = 0;
        static int value = 0;
        static bool comEnd = true;
        static bool strEnd = true;
        static bool error = false;
        static void DFA(string input_data)
        {
        Again:
            if (!comEnd)
            {
                temp = null;
                Comment(input_data);
                if (!comEnd)
                    return;
                else
                    goto Again;
            }
            if (id == input_data.Length) return;
            if (input_data[id] == ' ')
            {
                id++;
                if (id == input_data.Length) return;
                goto Again;
            }

            if (input_data[id] == '-' || input_data[id] == '+')
            {
                if (id + 1 < input_data.Length && input_data[id + 1] >= '0' && input_data[id + 1] <= '9')
                {
                    Integer(input_data);
                    goto Again;
                }
                if (id + 1 < input_data.Length && (input_data[id + 1] == '%' || input_data[id + 1] == '&' || input_data[id + 1] == '$'))
                {
                    Sistem(input_data);
                    goto Again;
                }
                else
                {
                    Symbols(input_data);
                    goto Again;
                }
            }
            else if (input_data[id] >= '0' && input_data[id] <= '9')
            {
                Integer(input_data);
                goto Again;
            }
            else if (((input_data[id] >= 'A') && (input_data[id] <= 'Z')) || ((input_data[id] >= 'a') && (input_data[id] <= 'z')))
            {
                Identifier(input_data);
                goto Again;
            }
            else if (input_data[id] == '%' || input_data[id] == '&' || input_data[id] == '$')
            {
                Sistem(input_data);
                goto Again;
            }
            else if (input_data[id] == '\'')
            {
                String(input_data);
                goto Again;
            }
            else if (input_data[id] == '/')
            {
                if (id + 1 < input_data.Length)
                {
                    if (input_data[id] == '/' && input_data[id + 1] == '/')
                    {
                        oneLineComment(input_data);
                        return;
                    }
                    else
                    {
                        Symbols(input_data);
                        goto Again;
                    }
                }
                else
                {
                    Symbols(input_data);
                    goto Again;
                }
            }
            else if (input_data[id] == '{')
            {
                Comment(input_data);
                if (!comEnd)
                    return;
                else
                    goto Again;
            }
            else if (input_data[id] == '#' && id + 1 < input_data.Length && input_data[id + 1] >= '0' && input_data[id + 1] <= '9')
            {
                Char(input_data);
                goto Again;
            }
            else
            {
                Symbols(input_data);
                goto Again;
            }
        }

        static void Integer(string input_data)
        {
            for (int i = id; i < input_data.Length; i++)
            {
                if (temp == null)
                    first_symbol = i + 1;
                if ((input_data[i] >= '0' && input_data[i] <= '9') || (input_data[i] == '.') || ((input_data[i] == '-') || (input_data[i] == '+')) && (temp == null))//интеджер
                {
                    if (input_data[i] == '.')
                    {
                        if (i + 1 == input_data.Length || ((i + 2 < input_data.Length) && !(input_data[i + 1] >= '0' && input_data[i + 1] <= '9')))
                        {
                            meaning = temp;
                            Result();
                            return;
                        }
                        temp += input_data[i];
                        category = "real";
                    }
                    else
                    {
                        if (category == null)
                            category = "integer";
                        temp += input_data[i];
                        if (i == input_data.Length - 1)
                        {
                            meaning = temp;
                            if (category == "integer")
                            {
                                bool success = int.TryParse(meaning, out value);
                                if (success)
                                {
                                    Result();
                                    return;
                                }
                                else
                                {
                                    Error(2);
                                    return;
                                }
                            }
                            if (category == "real")
                            {
                                Result();
                                return;
                            }
                        }
                    }
                }
                else if (input_data[i] == 'e' || input_data[i] == 'E')
                {
                    temp += input_data[i];
                    category = "real";
                    if (i + 1 < input_data.Length && (input_data[i+1] == '-' || input_data[i+1] == '+'))
                    {
                        temp += input_data[i + 1];
                        i++;
                    }
                    else if (i == input_data.Length - 1 || input_data[i] == ' ')
                    {
                        Error(1);
                        return;
                    }
                }
                else if (input_data[i] == ' ')
                {
                    meaning = temp;
                    if (error)
                    {
                        Error(1);
                        return;
                    }
                    bool success = int.TryParse(meaning, out value);
                    if (success)
                    {
                        Result();
                        return;
                    }
                    else
                    {
                        Error(2);
                        return;
                    }
                }
                else if (input_data[i] == '+' || input_data[i] == '+')
                {
                    meaning = temp;
                    if (error)
                    {
                        Error(1);
                        return;
                    }
                    Result();
                    return;
                }
                else if (((input_data[i] >= 'A') && (input_data[i] <= 'Z')) || ((input_data[i] >= 'a') && (input_data[i] <= 'z')) || input_data[i] == '_')
                {
                    temp += input_data[i];
                    error = true;
                    Identifier(input_data);
                    return;
                }
                else
                {
                    meaning = temp;
                    if (error)
                    {
                        Error(1);
                        return;
                    }
                    Result();
                    return;
                }
            }
        }

        static void Identifier(string input_data)
        {
            int id_save = 0;
            category = "identifier"; 
            if (temp != null)
            {
                id += temp.Length;
                id_save = temp.Length;
            }
            for (int i = id; i < input_data.Length; i++)
            {
                if (temp == null)
                    first_symbol = i + 1;
                if (((input_data[i] >= 'A') && (input_data[i] <= 'Z')) || ((input_data[i] >= 'a') && (input_data[i] <= 'z')) || (input_data[i] == '_') || (input_data[i] >= '0' && input_data[i] <= '9'))
                {
                    temp += input_data[i];
                    if (i == input_data.Length - 1)
                    {
                        meaning = temp;
                        id -= id_save;
                        if (error)
                        {
                            Error(1);
                            return;
                        }
                        KeyWord();
                        Result();
                        return;
                    }
                }
                else
                {
                    meaning = temp;
                    id -= id_save;
                    if (error)
                    {
                        Error(1);
                        return;
                    }
                    KeyWord();
                    Result();
                    return;
                }
            }
            id -= id_save;
            if (error)
            {
                Error(1);
                return;
            }
        }

        static void String(string input_data)
        {
            category = "string";
            int id_save = 0;
            if (temp != null)
            {
                id += temp.Length;
                id_save = temp.Length;
            }
            for (int i = id; i < input_data.Length; i++)
            {
                if (temp == null)
                    first_symbol = i + 1;
                if (i == id && i + 1 < input_data.Length && input_data[i] == '\'' && input_data[i + 1] == '\'')
                {
                    if (i + 2 < input_data.Length && input_data[i + 2] == '\'')
                    {
                        temp += input_data[i];
                        temp += input_data[i + 1];
                        temp += input_data[i + 2];
                        meaning += "\'";
                        i += 2;
                    }
                    else
                    {
                        temp += input_data[i];
                        temp += input_data[i + 1];
                        meaning = " ";
                        Result();
                        return;
                    }
                }
                else if (i + 1 < input_data.Length && input_data[i] == '\'' && input_data[i + 1] == '\'')
                {
                    temp += input_data[i];
                    temp += input_data[i + 1];
                    meaning += "\'";
                    i++;
                }
                else
                {
                    if (input_data[i] == '\'')
                    {
                        temp += input_data[i];
                        if (meaning != null)
                        {
                            if (i + 1 < input_data.Length - 1 && input_data[i + 1] == '#')
                            {
                                Char(input_data);
                                return;
                            }
                            else if (!strEnd)
                            {
                                strEnd = true;
                            }
                            else
                            {
                                id -= id_save;
                                Result();
                                return;
                            }
                        }
                    }
                    else
                    {
                        meaning += input_data[i];
                        temp += input_data[i];

                        if (i == input_data.Length - 1)
                        {
                            Error(3);
                            return;
                        }
                    }
                }
            }
        }

        static void KeyWord()
        {
            for (int j = 0; j < keyWords.Length; j++)
            {
                if (temp.ToLower() == keyWords[j])
                {
                    category = "keyword";
                }
            }
            if (temp.ToLower() == "false" || temp.ToLower() == "true")
            {
                category = "boolean";
            }
        }

        static void Sistem(string input_data)
        {
            long answer = 0; char top = '9'; int up = 10;
            bool negative = false;
            category = "integer";
            input_data = input_data.ToUpper();

            for (int i = id; i < input_data.Length; i++)
            {
                if (temp == null)
                    first_symbol = i + 1;

                if (input_data[i] == '+' || input_data[i] == '-')
                {
                    if (input_data[i] == '-') negative = true;
                    temp += input_data[i];
                    i++;
                }
                else if (input_data[i] == ' ')
                {
                    if (negative) answer = 0 - answer;
                    meaning = answer.ToString();

                    if (answer > 2147483647 || answer < -2147483648)
                    {
                        Error(2);
                        return;
                    }
                    if (error)
                    {
                        Error(1);
                        return;
                    }
                    Result();
                    return;
                }

                if (input_data[i] == '%' || input_data[i] == '&' || input_data[i] == '$')
                {
                    if (i + 1 == input_data.Length)
                    {
                        temp += input_data[i];
                        Error(1);
                        return;
                    }
                    else
                    {
                        if (input_data[i] == '%')
                        {
                            top = '1';
                            up = 2;
                        }
                        if (input_data[i] == '&')
                        {
                            top = '7';
                            up = 8;
                        }
                        if (input_data[i] == '$')
                        {
                            top = 'F';
                            up = 16;
                        }
                        temp += input_data[i];
                        i++;
                    }
                }
                else if (input_data[i] == 46)
                {
                    Error(1);
                    return;
                }

                int comp = input_data[i] - '0';
                int border = top - '0';

                if (comp > border)
                {
                    error = true;
                    temp += input_data[i];
                }
                else if (((input_data[i] >= '0') && (input_data[i] <= '9')) || ((input_data[i] >= 'A') && (input_data[i] <= 'F')))
                {
                    if ((input_data[i] >= 'A') && (input_data[i] <= 'F'))
                    {
                        comp = input_data[i] - 'A' + 10;
                    }
                    temp += input_data[i];
                    answer = (answer * up) + comp;
                    if ((!negative) && answer > 2147483647)
                    {
                        Error(2);
                        return;
                    }
                    meaning = answer.ToString();
                }
            }
            if (negative) answer = 0 - answer;
            meaning = answer.ToString();

            if (answer > 2147483647 || answer < -2147483648)
            {
                Error(2);
                return;
            }
            if (error)
            {
                Error(1);
                return;
            }
            Result();
            return;
        }

        static void Comment(string input_data)
        {
            for (int i = id; i < input_data.Length; i++)
            {
                temp += input_data[i];
                if (input_data[i] == '}')
                {
                    comEnd = true;
                    id += temp.Length;
                    temp = null;
                    return;
                }
                else if (i == input_data.Length - 1)
                {
                    comEnd = false;
                    id += temp.Length;
                    temp = null;
                    return;
                }
            }
        }

        static void oneLineComment(string input_data)
        {
            for (int i = id; i < input_data.Length; i++)
            {
                if (id == input_data.Length)
                {
                    return;
                }
            }
        }

        static void Symbols(string input_data)
        {
            for (int i = id; i < input_data.Length; i++)
            {
                if (temp == null)
                    first_symbol = i + 1;
                foreach (char c in operators)
                {
                    if (input_data[i] == c)
                    {
                        if (i + 1 < input_data.Length)
                        {
                            if (input_data[i] == ':' && input_data[i + 1] == '=')
                            {
                                temp = input_data[i].ToString() + input_data[i + 1];
                                meaning = temp;
                                category = "operators";
                                Result();
                                return;
                            }
                            if (input_data[i] == '<' && (input_data[i + 1] == '>' || input_data[i + 1] == '='))
                            {
                                temp = input_data[i].ToString() + input_data[i + 1];
                                meaning = temp;
                                category = "operators";
                                Result();
                                return;
                            }
                            else
                            if (input_data[i] == '>' && input_data[i + 1] == '=')
                            {
                                temp = input_data[i].ToString() + input_data[i + 1];
                                meaning = temp;
                                category = "operators";
                                Result();
                                return;
                            }
                        }
                        temp = input_data[i].ToString();
                        meaning = temp;
                        category = "operators";
                        Result();
                        return;
                    }
                }
                foreach (char c in separators)
                {
                    if (input_data[i] == c)
                    {
                        temp = input_data[i].ToString();
                        meaning = temp;
                        category = "separators";
                        Result();
                        return;
                    }
                }

            }
        }

        static void Char(string input_data)
        {
            if (category == null)
                category = "char";
            string ascii = null;
            int id_save = 0;
            if (temp != null)
            {
                id += temp.Length;
                id_save = temp.Length;
            }
                for (int i = id; i < input_data.Length; i++)
            {
                if (temp == null)
                    first_symbol = i + 1;
                if (input_data[i] == '#' && i + 1 < input_data.Length && input_data[i + 1] != '#' && ascii == null)
                {
                    temp += input_data[i];
                }
                else if (input_data[i] == '#' && ascii != null)
                {
                    goto TryAscii;
                }
                else if (input_data[i] >= '0' && input_data[i] <= '9')
                {
                    temp += input_data[i];
                    ascii += input_data[i];
                }
                else if (((input_data[i] >= 'A') && (input_data[i] <= 'Z')) || ((input_data[i] >= 'a') && (input_data[i] <= 'z')))
                {
                    Error(4);
                }
                else if (input_data[i] == '/' && input_data[i + 1] == '/' || input_data[i] == '\'' || input_data[i] == ' ')
                {
                    if (input_data[i] == '\'') strEnd = false;
                    goto TryAscii;
                }
                else
                {
                    Error(5);
                }
            }
        TryAscii:
            try
            {
                id -= id_save;
                int unicode = int.Parse(ascii);
                meaning += Convert.ToChar(unicode);
                if (strEnd)
                {
                    Result();
                    return;
                }
                else
                {
                    String(input_data);
                    return;
                }
            }
            catch
            {
                Error(4);
            }
        }

        static void Result()
        {
            string result = line_number + " " + first_symbol + " " + category + " " + meaning + " " + temp;
            Console.WriteLine(result);
            id += temp.Length;
            temp = null;
            meaning = null;
            category = null;
        }

        static void Error(int codeError)
        {
            switch (codeError)
            {
                case 1:
                    Console.WriteLine($"Invalid format on line {line_number}");
                    break;
                case 2:
                    Console.WriteLine($"Range check error while evaluating constants on line {line_number}");
                    break;
                case 3:
                    Console.WriteLine($"String exceeds line on {line_number} line");
                    break;
                case 4:
                    Console.WriteLine($"Illegal char constant on line {line_number}");
                    break;
                case 5:
                    Console.WriteLine($"Syntax error on line {line_number}");
                    break;
            }
            id += temp.Length;
            temp = null;
            meaning = null;
            category = null;
            error = false;
            //Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            string input_data;

            for (int i = 1; i <= 56; i++)
            {
                Console.WriteLine($"--------File number: {i}----------");
                string fileName = $@"..\..\Tests\input_tests\{i}_in.txt";
                using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
                {
                    while ((input_data = sr.ReadLine()) != null)
                    {
                        id = 0;
                        DFA(input_data);
                        line_number++;
                    }
                }
                line_number = 1;
                first_symbol = 1;
            }
        }
    }
}