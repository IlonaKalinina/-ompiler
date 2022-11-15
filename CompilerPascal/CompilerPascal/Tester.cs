using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CompilerPascal
{
    public class Tester
    {
        public static void StartTest()
        {
            Console.WriteLine("Нажмите 1, чтобы провести общее тестирование\n" +
                              "Нажмите 2, чтобы провести подробное тестироние\n" +
                              "Нажмите 3, чтобы провести синтаксический анализ простейших выражений");
            string? key = Console.ReadLine();
            if (key == "1")
            {
                GeneralTesting();
            }
            if (key == "2")
            {
                DetailTesting();
            }
            if (key == "3")
            {
                Parser();
            }
        }
        static void GeneralTesting()
        {
            Lexer l = new Lexer();
            string in_path;
            string ans_path;
            string out_path;

            for (int i = 1; i <= 55; i++)
            {
                in_path = $@"..\..\..\..\..\Tests\input_tests\{i}_in.txt";
                ans_path = $@"..\..\..\..\..\Tests\answer_tests\{i}_ans.txt";
                out_path = $@"..\..\..\..\..\Tests\output_tests\{i}_out.txt";

                l.ReadFileLexer(in_path);
                List<string> answer = new List<string>();
                string line_res = "";

                if (Lexer.lexems.Count > 0)
                {
                    foreach (Lexema element in Lexer.lexems)
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
        static void DetailTesting()
        {
            Lexer l = new Lexer();
            string in_path;
            string ans_path;
            string out_path;

            for (int i = 1; i <= 55; i++)
            {
                in_path = $@"..\..\..\..\..\Tests\input_tests\{i}_in.txt";
                ans_path = $@"..\..\..\..\..\Tests\answer_tests\{i}_ans.txt";
                out_path = $@"..\..\..\..\..\Tests\output_tests\{i}_out.txt";

                l.ReadFileLexer(in_path);
                List<string> answer = new List<string>();
                string line_res = "";

                if (Lexer.lexems.Count > 0)
                {
                    foreach (Lexema element in Lexer.lexems)
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
        static void Parser()
        {

        }
    }
}
