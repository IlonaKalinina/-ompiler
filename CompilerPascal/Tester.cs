/*using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CompilerPascal
{
    public class Tester
    {
        public static void Test()
        {
            Console.WriteLine("1 - лексический анализатор\n" +
                              "2 - синтаксический анализ простейших выражений");
            string? key = Console.ReadLine();
            if (key == "1")
            {
                Console.WriteLine("3 - общее тестирование\n" +
                                  "4 - подробное тестирование");
                string? keyTesting = Console.ReadLine();
                if (keyTesting == "3")
                {
                    GeneralTestingLexer();
                }
                if (keyTesting == "4")
                {
                    DetailTestingLexer();
                }
            }
            if (key == "2")
            {
                GeneralTestingParser();
            }
        }
        static void GeneralTestingLexer()
        {
            string in_path;
            string ans_path;
            string out_path;

            for (int i = 1; i <= 55; i++)
            {
                in_path = $@"..\..\..\..\..\Tests\input_tests\{i}_in.txt";
                ans_path = $@"..\..\..\..\..\Tests\answer_tests\{i}_ans.txt";
                out_path = $@"..\..\..\..\..\Tests\output_tests\{i}_out.txt";

               // Lexer.Lexer.ReadFileLexer(in_path);
                List<string> answer = new List<string>();
                string line_res = "";

                if (Lexer.Lexer.lexems.Count > 0)
                {
                    foreach (Lexer.Lexema element in Lexer.Lexer.lexems)
                    {
                        if (element.categoryLexeme != "error")
                        {
                            line_res = element.numberLine + " " + element.numberSymbol + " " + element.categoryLexeme + " " + element.valueLexema + " " + element.initialLexema;
                            answer.Add(line_res);
                        }
                        else
                        {
                            line_res = element.valueLexema;
                            answer.Add(line_res);
                        }
                    }
                }

                using (StreamWriter writer = new StreamWriter(out_path))
                {
                    foreach (var line_out in answer)
                    {
                        writer.WriteLine(line_out);
                    }
                }

                int j = 0;
                bool wrong = false;
                using (StreamReader sr = new StreamReader(ans_path, Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line == answer[j])
                        {
                            wrong = false;
                        }
                        else
                        {
                            wrong = true;
                        }
                        j++;
                    }
                    if (j != answer.Count)
                    {
                        wrong = true;
                    }
                    if (line == null && line_res == "")
                    {
                        wrong = false;
                    }
                }

                if (wrong)
                {
                    Console.WriteLine($"{i} | WA");
                }
                else
                {
                    Console.WriteLine($"{i} | OK");
                }
            }
        }
        static void DetailTestingLexer()
        {
            string in_path;
            string ans_path;
            string out_path;

            for (int i = 53; i <= 55; i++)
            {
                in_path = $@"..\..\..\..\..\Tests\input_tests\{i}_in.txt";
                ans_path = $@"..\..\..\..\..\Tests\answer_tests\{i}_ans.txt";
                out_path = $@"..\..\..\..\..\Tests\output_tests\{i}_out.txt";

               // Lexer.Lexer.ReadFileLexer(in_path);
                List<string> answer = new List<string>();
                string line_res = "";

                if (Lexer.Lexer.lexems.Count > 0)
                {
                    foreach (Lexer.Lexema element in Lexer.Lexer.lexems)
                    {
                        
                        if (element.categoryLexeme != "error")
                        {
                            if (element.valueLexema != null)
                            {
                                line_res = element.numberLine + " " + element.numberSymbol + " " + element.categoryLexeme + " " + element.valueLexema + " " + element.initialLexema;
                                answer.Add(line_res);
                            }
                        }
                        else
                        {
                            line_res = element.valueLexema;
                            answer.Add(line_res);
                        }
                    }
                }

                using (StreamWriter writer = new StreamWriter(out_path))
                {
                    foreach (var line_out in answer)
                    {
                        writer.WriteLine(line_out);
                    }
                }

                int j = 0;
                bool wrong = false;
                string line;
                using (StreamReader sr = new StreamReader(ans_path, Encoding.Default))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line == answer[j])
                        {
                            wrong = false;
                        }
                        else
                        {
                            wrong = true;
                        }
                        j++;
                    }
                    if (j != answer.Count)
                    {
                        wrong = true;
                    }
                    if (line == null && line_res == "")
                    {
                        wrong = false;
                    }
                }
                Console.WriteLine($"\tTest №{i}");
                Console.WriteLine("\tInput:");
                using (StreamReader sr = new StreamReader(in_path, Encoding.Default))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
                Console.WriteLine("\tOutput:");
                using (StreamReader sr = new StreamReader(out_path, Encoding.Default))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
                Console.WriteLine("\tAnswer:");
                using (StreamReader sr = new StreamReader(ans_path, Encoding.Default))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
                Console.WriteLine("\tResult:");

                if (wrong)
                {
                    Console.WriteLine($"{i} | WA");
                }
                else
                {
                    Console.WriteLine($"{i} | OK");
                    Console.WriteLine();
                }
                var exit = Console.ReadKey();
                if (exit.Key == ConsoleKey.Escape)
                {
                    return;
                }
                else
                {
                    continue;
                }
            }
        }
        static void GeneralTestingParser()
        {
            Parser P = new Parser();
            
            string in_path;
            //string ans_path;
            //string out_path;

            for (int i = 500; i <= 500; i++)
            {
                in_path = $@"..\..\..\..\..\Tests\input_tests\{i}_in.txt";
               // ans_path = $@"..\..\..\..\..\Tests\answer_tests\{i}_ans.txt";
               // out_path = $@"..\..\..\..\..\Tests\output_tests\{i}_out.txt";

                P.ReadFileParser(in_path);
                List<string> answer = new List<string>();

                RunTree(P.doParse);
            }
        }

        static List<string> answerList = new List<string>();
        static string lineAnswer = null;
        static int numNode = 0;
        static int numChildren = -1;
        static string lineBuf = null;
        static void RunTree(Node doParse)
        {
            lineBuf = null;
            for(int i = 0; i < numNode; i++)
            {
                if (i == numNode - 1)
                {
                    if (numChildren == 0)
                    {
                        lineBuf += "├───";
                    }
                    else if (numChildren == 1)
                    {
                        lineBuf += "└───";
                    }
                }
                else
                {
                    lineBuf += "    ";
                }
            }
            lineAnswer = lineBuf + doParse.value;
            Console.WriteLine(lineAnswer);
            answerList.Add(lineAnswer);

            if (doParse.children != null)
            {
                numNode++;
                numChildren = 0;
                RunTree(doParse.children[0]);
                numChildren = -1;

                if (doParse.children.Count > 1)
                {
                    numChildren = 1;
                    RunTree(doParse.children[1]);
                    numChildren = -1;
                }
                numNode--;
            }
            return;
        }
    }
}
*/