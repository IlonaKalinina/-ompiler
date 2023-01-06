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

            doParse = Expression(nowLexem.Value);

            if (nowLexem.Type == Lexer.LexemaType.EOF && !findError)
            {
                doParse = new Node()
                {
                    category = "error",
                    value = $"({nowLexem.Line_number}, {nowLexem.Symbol_number}) Syntax error, missing binary operation"
                };
            }
        }

        public Node Expression(object input_data)
        {
            Node leftСhild = Term(input_data);
            if (leftСhild.category != "error")
            {
                while (nowLexem.Type == Lexer.LexemaType.OPERATOR && nowLexem.Source == "+" || nowLexem.Source == "-")
                {
                    leftСhild = AddChilde(leftСhild, input_data);
                    if (nowLexem.type == "End File") break;
                }
            }
            return leftСhild;
        }

        public Node Term(object input_data)
        {
            Node leftСhild = Factor(input_data);
            if (leftСhild.category != "error")
            {
                if (nowLexem.type != "End File")
                {
                    while (nowLexem.value.ToString() == "*" || nowLexem.value.ToString() == "/")
                    {
                        leftСhild = AddChilde(leftСhild, input_data);
                        if (nowLexem.type == "End File") break;
                    }
                }
            }
            return leftСhild;
        }

        public Node Factor(object input_data)
        {
            if (nowLexem.type == "integer" || nowLexem.type == "real")
            {
                Node nowChilde = new Node()
                {
                    category = "number",
                    value = nowLexem.value
                };
                if (nowLexem.type != "End File") nowLexem = lexer.GetLexem();
                return nowChilde;
            }

            if (nowLexem.type == "identifier")
            {
                Node nowChilde = new Node()
                {
                    category = "identifier",
                    value = nowLexem.value
                };
                if (nowLexem.type != "End File") nowLexem = lexer.GetLexem();
                return nowChilde;
            }

            if (nowLexem.value.ToString() == "(")
            {
                nowLexem = lexer.GetLexem();
                Node nextExpression = Expression(input_data);

                if (nowLexem.type == "End File")
                {
                    findError = true;
                    Node nowChilde = new Node()
                    {
                        category = "error",
                        value = $"No right bracket on line {nowLexem.line - 1}"
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
            var BinOp = nowLexem.value;
            if (nowLexem.type != "End File")
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
            if (nowLexem.value != null)
            {
                findError = true;
                return new Node()
                {
                    category = "error",
                    value = $"Syntax error on line {nowLexem.line}, don't have factor"
                };
            }
            else
            {
                findError = true;
                return new Node()
                {
                    category = "error",
                    value = $"Syntax error on line {nowLexem.line - 1}, don't have factor"
                };
            }
        }
    }
}