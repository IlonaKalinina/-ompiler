using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CompilerPascal.Parser
{
    class Node
    {
        public string type = "";
        public string value = "";
        public List<Node> children;
    }
    class Parser
    {
        Lexer.Lexema nowLexem;
        int closedParenthesiCounter;
        public Node doParse = new Node();
        public readonly StreamReader readFile;
        private Lexer.Lexer lexer;

       public Parser(string fileName)
        {
            lexer = new Lexer.Lexer(fileName);
            nowLexem = lexer.GetLexem();

            closedParenthesiCounter = 0;
            doParse = Expression(nowLexem.valueLexema);
        }

        public Node Expression(string input_data)
        {
            Node leftСhild = Term(input_data);
            while (nowLexem.valueLexema == "+" || nowLexem.valueLexema == "-")
            {
                var operation = nowLexem.valueLexema;
                if (nowLexem.categoryLexeme != "EndOfFile")
                {
                    nowLexem = lexer.GetLexem();
                }
                Node rightСhild = Term(input_data);
                leftСhild = new Node()
                {
                    type = "BinOp",
                    value = operation,
                    children = new List<Node> { leftСhild, rightСhild }
                };
                if (rightСhild.type == "Error")
                {
                    return new Node()
                    {
                        type = "Error",
                        value = rightСhild.value
                    };
                }
            }
            return leftСhild;
        }

        public Node Term(string input_data)
        {
            Node leftСhild = Factor(input_data);
            if (leftСhild.type != "Error")
            {
                if (nowLexem.categoryLexeme != "EndOfFile")
                {
                    nowLexem = lexer.GetLexem();
                }
                while (nowLexem.valueLexema == "*" || nowLexem.valueLexema == "/")
                {
                    var BinOp = nowLexem.valueLexema;
                    if (nowLexem.categoryLexeme != "EndOfFile")
                    {
                        nowLexem = lexer.GetLexem();
                    }
                    Node rightСhild = Factor(input_data);
                    if (nowLexem.categoryLexeme != "EndOfFile")
                    {
                        nowLexem = lexer.GetLexem();
                    }
                    leftСhild = new Node()
                    {
                        type = "BinOp",
                        value = BinOp,
                        children = new List<Node> { leftСhild, rightСhild }
                    };
                    if (rightСhild.type == "Error")
                    {
                        return new Node()
                        {
                            type = "Error",
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
                    type = "Number",
                    value = nowLexem.valueLexema
                };
            }
            if (nowLexem.categoryLexeme == "identifier")
            {
                return new Node()
                {
                    type = "Identifier",
                    value = nowLexem.valueLexema
                };
            }
            if (nowLexem.valueLexema == "(")
            {
                Node nextExpression = new Node();
                if (nowLexem.categoryLexeme != "EndOfFile")
                {
                    nowLexem = lexer.GetLexem();
                    nextExpression = Expression(input_data);
                }
                else
                {
                    nextExpression = new Node()
                    {
                        type = "Error",
                        value = $"Syntax error on line {nowLexem.numberLine}, \")\" expected"
                    };
                }
                if (nowLexem.valueLexema != ")")
                {
                    if (nowLexem.categoryLexeme != "EndOfFile")
                    {
                        closedParenthesiCounter++;
                    }
                    else
                    {
                        nextExpression = new Node()
                        {
                            type = "Error",
                            value = $"Syntax error on line {nowLexem.numberLine}, \")\" expected"
                        };
                    }
                }
                return nextExpression;
            }
            return new Node()
            {
                type = "Error",
                value = $"Syntax error on line {nowLexem.numberLine}, don't have factor"
            };
        }
    }
}