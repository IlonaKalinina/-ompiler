using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CompilerPascal.Parser
{
    class Parser
    {
        public Node doParse = new Node();

        private Lexer.Lexema nowLexem;
        private readonly Lexer.Lexer lexer;
        private bool findError = false;

       public Parser(string fileName)
        {
            lexer = new Lexer.Lexer(fileName);
            nowLexem = lexer.GetLexem();

            doParse = Expression(nowLexem.valueLexema.ToString());

            if(nowLexem.categoryLexeme != "End File" && !findError)
            {
                doParse = new Node()
                {
                    category = "error",
                    value = $"Syntax error on line {nowLexem.numberLine}, missing binary operation"
                };
            }
        }

        public void Programm(string input_data)
        {
            if (nowLexem.valueLexema.ToString() == "programm")
            {
                //добавить программ в корень
                //ищем следующую лексему, чтоб найти название программы
                nowLexem = lexer.GetLexem(); //добавить проверку что не конец файла
                if (nowLexem.categoryLexeme.ToString() == "identifier")
                {
                    nowLexem = lexer.GetLexem();
                    if (nowLexem.categoryLexeme == "separators" && nowLexem.valueLexema.ToString() == ";")
                    {
                    }
                    else
                    {
                        //ошибка, нет ;
                    }
                }
                else
                {
                    //ошибка, не может не быть названия
                }
            }
            //если нет программ, то нет ошибки, но нужно добавить в корень просто программ без названия, далее ищем описание программы var (ребенок)
            Var(input_data);
        }
        public void Var(string input_data)
        {
            //ищем ключевое слово
            nowLexem = lexer.GetLexem();
            if (nowLexem.valueLexema.ToString() == "var")
            {
                //записываем куда то вар
                //ищем идентификаторы
                Id(input_data);
            }
            else
            {
                //ошибка
            }
        }
        //a, b, c
        public void Id(string input_data)
        {
            nowLexem = lexer.GetLexem();
            while (nowLexem.valueLexema.ToString() != ":")
            {
                if (nowLexem.categoryLexeme.ToString() == "indetifier")
                {
                    //говорим что нашли одного ребенка вар, далее ищем либо еще одного, либо :
                    nowLexem = lexer.GetLexem();
                    if (nowLexem.valueLexema.ToString() == ",")
                    {
                        Id(input_data);
                    }
                }
            }
            //после нахождения : ищем тип данных
        }

        public void Type(string input_data)
        {
            nowLexem = lexer.GetLexem();
            if (nowLexem.valueLexema.ToString() == "identifier" ||
                nowLexem.valueLexema.ToString() == "string" ||
                nowLexem.valueLexema.ToString() == "real" ||
                nowLexem.valueLexema.ToString() == "keyword" ||
                nowLexem.valueLexema.ToString() == "char" ||
                nowLexem.valueLexema.ToString() == "integer")
            {
                //записываем тип данных
                nowLexem = lexer.GetLexem();
                if (nowLexem.categoryLexeme == "separators" && nowLexem.valueLexema.ToString() == ";")
                {
                }
            }
            else
            {
                //ошибка, не указан тип данных
            }
        }

        public Node Expression(string input_data)
        {
        Node leftСhild = Term(input_data);
        if (leftСhild.category != "error")
        {
            if (nowLexem.categoryLexeme != "End File")
            {
                while (nowLexem.valueLexema.ToString() == "+" || nowLexem.valueLexema.ToString() == "-")
                {
                    var BinOp = nowLexem.valueLexema;
                    if (nowLexem.categoryLexeme != "End File")
                    {
                        nowLexem = lexer.GetLexem();
                    }
                    Node rightСhild = Term(input_data);
                    leftСhild = new Node()
                    {
                        category = "binOp",
                        value = BinOp,
                        children = new List<Node> { leftСhild, rightСhild }
                    };
                    if (rightСhild.category == "error")
                    {
                        return new Node()
                        {
                            category = "error",
                            value = rightСhild.value
                        };
                    }

                    if (nowLexem.categoryLexeme == "End File") break;
                }
            }
        }
        return leftСhild;
        }

        public Node Term(string input_data)
        {
            Node leftСhild = Factor(input_data);
            if (leftСhild.category != "error")
            {
                if (nowLexem.categoryLexeme != "End File")
                {
                    nowLexem = lexer.GetLexem();
                }
                if (nowLexem.categoryLexeme != "End File")
                {
                    while (nowLexem.valueLexema.ToString() == "*" || nowLexem.valueLexema.ToString() == "/")
                    {
                        var BinOp = nowLexem.valueLexema;
                        if (nowLexem.categoryLexeme != "End File")
                        {
                            nowLexem = lexer.GetLexem();
                        }
                        Node rightСhild = Factor(input_data);
                        if (nowLexem.categoryLexeme != "End File")
                        {
                            nowLexem = lexer.GetLexem();
                        }
                        
                        leftСhild = new Node()
                        {
                            category = "binOp",
                            value = BinOp,
                            children = new List<Node> { leftСhild, rightСhild }
                        };
                        if (rightСhild.category == "error")
                        {
                            return new Node()
                            {
                                category = "error",
                                value = rightСhild.value
                            };
                        }
                        if (nowLexem.categoryLexeme == "End File") break;
                    }
                }
            }
            return leftСhild;
        }

        public Node Factor(string input_data)
        {
            if (nowLexem.categoryLexeme == "integer" || nowLexem.categoryLexeme == "real")
            {
                return new Node()
                {
                    category = "number",
                    value = nowLexem.valueLexema
                };
            }
            if (nowLexem.categoryLexeme == "identifier")
            {
                return new Node()
                {
                    category = "identifier",
                    value = nowLexem.valueLexema
                };
            }

            if (nowLexem.valueLexema.ToString() == "(")
            {
                Node nextExpression = new Node();
                if (nowLexem.categoryLexeme != "End File")
                {
                    nowLexem = lexer.GetLexem();

                    nextExpression = Expression(input_data);

                    if (nowLexem.categoryLexeme == "End File")
                    {
                        findError = true;
                        return new Node()
                        {
                            category = "error",
                            value = $"No right bracket on line {nowLexem.numberLine - 1}"
                        };
                    }

                }
                return nextExpression;
            }

            if (nowLexem.valueLexema != null)
            {
                findError = true;
                return new Node()
                {
                    category = "error",
                    value = $"Syntax error on line {nowLexem.numberLine}, don't have factor"
                };
            } else
            {
                findError = true;
                return new Node()
                {
                    category = "error",
                    value = $"Syntax error on line {nowLexem.numberLine - 1}, don't have factor"
                };
            }
               
        }
    }
}