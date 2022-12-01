using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CompilerPascal.Parser
{
    /*
    class Node
    {
    }
    class BinOP : Node
    {
        public string op = "";
        public List<Node> children;
    }
    class Num : Node
    {
        public string value = "";
    }
    class Variable: Node
    {
        public string value = "";
    }*/
    class Node
    {
        public string type = "";
        public string value = "";
        public List<Node> children;
    }
    class Parser
    {
        Lexer.Lexema nowLexem;
        public Node doParse = new Node();
        private Lexer.Lexer lexer;

       public Parser(string fileName)
        {
            lexer = new Lexer.Lexer(fileName);
            nowLexem = lexer.GetLexem();

            doParse = Expression(nowLexem.valueLexema);
        }

        public Node Expression(string input_data)
        {
            Node leftСhild = Term(input_data);
            while (nowLexem.valueLexema == "+" || nowLexem.valueLexema == "-")
            {
                var operation = nowLexem.valueLexema;
                if (nowLexem.categoryLexeme != "End File")
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
                if (nowLexem.categoryLexeme != "End File")
                {
                    nowLexem = lexer.GetLexem();
                    nextExpression = Expression(input_data);
                }
                else
                {
                    nextExpression = new Node()
                    {
                        type = "Error",
                        value = $"No right bracket on line {nowLexem.numberLine - 1}"
                    };
                }
                if (nowLexem.valueLexema != ")")
                {
                    nextExpression = new Node()
                    {
                        type = "Error",
                        value = $"No right bracket on line {nowLexem.numberLine - 1}"
                    };
                }
                return nextExpression;
            }
            return new Node()
            {
                type = "Error",
                value = $"Syntax error on line {nowLexem.numberLine - 1}, don't have factor"
            };
        }
    }
}