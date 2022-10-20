using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class Program
    {
        static string[] keyWords = {"absolute", "and", "array", "asm", "begin", "boolean", "break", "case", "char", "const", "continue",
                                        "div", "do", "downto", "else", "end", "for", "function", "goto", "if", "implementation", "in", "interrupt", "is",
                                        "label", "mod", "not", "or", "org", "otherwise", "print", "procedure", "program", "read", "real", "record",
                                        "repeat", "shl", "shr", "step", "string", "then", "to", "type", "unit", "until", "uses", "var", "while", "with", "xor" };
        static char[] separators = { ',', '.', '(', ')', '[', ']', ':', ';', '@', '{', '}' };
        static char[] math = { '+', '-', '*', '/', '<', '>', ':', '=' };
        static string temp = null;
        static string state = null;
        static int SS = 10;
        static void Print(string state, string temp)
        {
            if (state == "comments") return;
            Console.Write(state);
            Console.Write(" ");
            Console.Write(temp);
            Console.Write(" ");
            Console.Write(temp);
            Console.WriteLine();

            string path_out = @"01_out.txt";
            string text = state + " " + temp + " " + temp;

            using (FileStream file = File.Create(path_out))
            {
                byte[] bytes = Encoding.ASCII.GetBytes(text);
                file.Write(bytes, 0, bytes.Length);
            }
        }
        static void Check(char nextSymbol, int i, string input_data)
        {
            if (state == "error") return;
            if ((nextSymbol == ' ' || nextSymbol == '\n') && (temp != null))
            {
                Result(temp);
                state = null;
                temp = null;
                return;
            }
            if (((nextSymbol >= 'A') && (nextSymbol <= 'Z')) || ((nextSymbol >= 'a') && (nextSymbol <= 'z')) || (nextSymbol == '_'))
            {
                if (temp == null)
                    state = "identifier";
                temp += nextSymbol;
                if (i == input_data.Length - 1)
                {
                    Result(temp);
                    temp = null;
                }
                return;
            }
            if (((nextSymbol >= '0') && (nextSymbol <= '9')) || (nextSymbol == '.'))
            {
                switch (SS)
                {
                    case 10:
                        if (nextSymbol == '.')
                        {
                            temp += nextSymbol;
                            state = "real";
                            return;
                        }
                        else
                        {
                            if (temp == null)
                                state = "integer";
                            temp += nextSymbol;
                            if (i == input_data.Length - 1)
                            {
                                Result(temp);
                                temp = null;
                            }
                            return;
                        }
                    case 2:
                        break;
                    case 8:
                        break;
                    case 16:
                        break;
                }
            }
            if (nextSymbol == '%' || nextSymbol == '&' || nextSymbol == '$')
            {
                switch (nextSymbol)
                {
                    case '%':
                        if (input_data[i + 1] >= 0 && input_data[i + 1] <= 1)
                        {
                            SS = 2;
                        } else
                        {
                            state = "error";
                            Console.WriteLine(state);
                            return;
                        }
                        break;
                    case '&':
                        if (input_data[i + 1] >= 0 && input_data[i + 1] <= 7)
                        {

                        }
                        break;
                    case '$':
                        if (input_data[i + 1] >= 0 && input_data[i + 1] <= 1)
                        {

                        }
                        break;
                }
            }
            foreach (char c in separators)
            {
                if (nextSymbol == c)
                {
                    if (nextSymbol == ':' && input_data[i + 1] == '=')
                    {
                        temp = nextSymbol.ToString() + input_data[i + 1];
                        state = "math";
                        Result(temp);
                        temp = null;
                        return;
                    }
                    temp = nextSymbol.ToString();
                    state = "separators";
                    Result(temp);
                    temp = null;
                    return;
                }
            }
            foreach (char c in math)
            {
                if (nextSymbol == c)
                {
                    if (i + 1 < input_data.Length)
                        if (nextSymbol == '<' && (input_data[i + 1] == '>' || input_data[i + 1] == '='))
                        {
                            temp = nextSymbol.ToString() + input_data[i + 1];
                            state = "math";
                            Result(temp);
                            temp = null;
                            return;
                        }
                        else
                            if (nextSymbol == '>' && input_data[i + 1] == '=')
                            {
                                temp = nextSymbol.ToString() + input_data[i + 1];
                                state = "math";
                                Result(temp);
                                temp = null;
                                return;
                            }
                    if ((nextSymbol == '=' &&
                            (input_data[i - 1] == '>' || input_data[i - 1] == '<' || input_data[i - 1] == ':'))
                            || ((nextSymbol == '>') && (input_data[i - 1] == '<')))
                    {
                        return;
                    }
                    temp = nextSymbol.ToString();
                    state = "math";
                    Result(temp);
                    temp = null;
                    return;
                }
            }
        }
        static void Result(string temp)
        {
            for (int j = 0; j < keyWords.Length; j++)
            {
                if (temp == keyWords[j])
                {
                    state = "keyword";
                }
            }
            Print(state, temp);
        }
        static void Main(string[] args)
        {
            string path = @"01_in.txt";
            string input_data;
            string text;
            using (FileStream stream = File.OpenRead(path))
            {
                int totalBytes = (int)stream.Length;
                byte[] bytes = new byte[totalBytes];
                int bytesRead = 0;

                while (bytesRead < totalBytes)
                {
                    int len = stream.Read(bytes, bytesRead, totalBytes);
                    bytesRead += len;
                }

                text = Encoding.UTF8.GetString(bytes);

            }
            input_data = text;
            for (int i = 0; i < input_data.Length; i++)
            {
                if (input_data[i] == '/' && input_data[i+1] == '/')
                {
                    temp += "/";
                    i++;
                    while (i != input_data.Length)
                    {
                        temp += input_data[i];
                        i++;
                    }
                    state = "comments";
                    Print(state, temp);
                    temp = null;
                } else
                if (input_data[i] == '{')
                {
                    temp += "{";
                    i++;
                    while (input_data[i] != '}')
                    {
                        temp += input_data[i];
                        i++;
                    }
                    temp += "}";
                    state = "comments";
                } else
                if (input_data[i] == '}' && temp != null)
                {
                    temp = null;
                } else
                 Check(input_data[i], i, input_data);
            }
        }
    }
}
