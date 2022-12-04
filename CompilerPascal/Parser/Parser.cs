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
        Lexer.Lexema nowLexem;
        public Node doParse = new Node();
        private readonly Lexer.Lexer lexer;

       public Parser(string fileName)
        {
            lexer = new Lexer.Lexer(fileName);
            nowLexem = lexer.GetLexem();

            doParse = Expression(nowLexem.valueLexema);
        }

        public Node Expression(string input_data)
        {
            Node leftСhild = Term(input_data);
            if (leftСhild.category != "error")
            {
                if (nowLexem.valueLexema == "+" || nowLexem.valueLexema == "-")
                {
                    while (nowLexem.valueLexema == "+" || nowLexem.valueLexema == "-")
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
                    }
                }
                else if (nowLexem.valueLexema != null && nowLexem.valueLexema != "*" && nowLexem.valueLexema != "/" && nowLexem.valueLexema != ")")
                {
                    return new Node()
                    {
                        category = "error",
                        value = $"Syntax error on line {nowLexem.numberLine}, missing binary operation"
                    };
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
                while (nowLexem.valueLexema == "*" || nowLexem.valueLexema == "/")
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

            if (nowLexem.valueLexema == "(")
            {
                Node nextExpression = new Node();
                if (nowLexem.categoryLexeme != "End File")
                {
                    nowLexem = lexer.GetLexem();
                    if (nowLexem.valueLexema == ")")
                    {
                        return new Node()
                        {
                            category = "error",
                            value = $"Syntax error on line {nowLexem.numberLine}, don't have factor"
                        };
                    }
                    nextExpression = Expression(input_data);
                }
                if (nowLexem.valueLexema != ")" && nextExpression.category != "error")
                {
                    if (nowLexem.valueLexema != null)
                    {
                        return new Node()
                        {
                            category = "error",
                            value = $"No right bracket on line {nowLexem.numberLine}"
                        };
                    }
                    else
                    {
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
                return new Node()
                {
                    category = "error",
                    value = $"Syntax error on line {nowLexem.numberLine}, don't have factor"
                };
            } else
            {
                return new Node()
                {
                    category = "error",
                    value = $"Syntax error on line {nowLexem.numberLine - 1}, don't have factor"
                };
            }
               
        }
    }
}