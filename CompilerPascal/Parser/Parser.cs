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

            if (nowLexem.categoryLexeme != "End File" && !findError)
            {
                doParse = new Node()
                {
                    category = "error",
                    value = $"Syntax error on line {nowLexem.numberLine}, missing binary operation"
                };
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
                        leftСhild = AddChilde(leftСhild, input_data);
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
                    while (nowLexem.valueLexema.ToString() == "*" || nowLexem.valueLexema.ToString() == "/")
                    {
                        leftСhild = AddChilde(leftСhild, input_data);
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
                Node nowChilde = new Node()
                {
                    category = "number",
                    value = nowLexem.valueLexema
                };
                if (nowLexem.categoryLexeme != "End File") nowLexem = lexer.GetLexem();
                return nowChilde;
            }

            if (nowLexem.categoryLexeme == "identifier")
            {
                Node nowChilde = new Node()
                {
                    category = "identifier",
                    value = nowLexem.valueLexema
                };
                if (nowLexem.categoryLexeme != "End File") nowLexem = lexer.GetLexem();
                return nowChilde;
            }

            if (nowLexem.valueLexema.ToString() == "(")
            {
                nowLexem = lexer.GetLexem();
                Node nextExpression = Expression(input_data);

                if (nowLexem.categoryLexeme == "End File")
                {
                    findError = true;
                    Node nowChilde = new Node()
                    {
                        category = "error",
                        value = $"No right bracket on line {nowLexem.numberLine - 1}"
                    };
                    return nowChilde;
                }
                else
                {
                    nowLexem = lexer.GetLexem();
                    return nextExpression;
                }
            }
            return Exep();
        }

        public Node AddChilde(Node leftСhild, string input_data)
        {
            var BinOp = nowLexem.valueLexema;
            if (nowLexem.categoryLexeme != "End File")
            {
                nowLexem = lexer.GetLexem();
            }
            leftСhild = new Node()
            {
                category = "binOp",
                value = BinOp,
                children = new List<Node> { leftСhild, Term(input_data) }
            };
            if (leftСhild.children[1].category == "error")
            {
                return new Node()
                {
                    category = "error",
                    value = leftСhild.children[1].value
                };
            }
            return leftСhild;
        }

        public Node Exep()
        {
            if (nowLexem.valueLexema != null)
            {
                findError = true;
                return new Node()
                {
                    category = "error",
                    value = $"Syntax error on line {nowLexem.numberLine}, don't have factor"
                };
            }
            else
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