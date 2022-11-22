using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CompilerPascal
{
    class Node
    {
        public string type = "";
        public string value = "";
        public List<Node> children;
    }
    class Parser
    {
        Lexema nowLexem = null;
        Lexer lexer = new Lexer();
        int closedParenthesiCounter = 0;
        public Node doParse = new Node();

        public void ReadFileParser(string path)
        {
            doParse = new Node();
            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] in_file = new byte[fstream.Length];
                fstream.Read(in_file);

                List<byte> input_data = new List<byte>();
                for (int i = 0; i < in_file.Length; i++)
                {
                    input_data.Add(in_file[i]);
                }
                nowLexem = lexer.GetLexem(ref input_data);
                doParse = Expression(ref input_data);

                /*if (nowLexem.valueLexema != null)
                {
                    doParse = new Node()
                    {
                        type = "Error",
                        value = $"Syntax error on line {nowLexem.numberLine}, \"\" "
                    };
                }*/
            }
            return;
        }
        public Node Expression(ref List<byte> input_data)
        {
            Node leftСhild = Term(ref input_data);
            while (nowLexem.valueLexema == "+" || nowLexem.valueLexema == "-")
            {
                var operation = nowLexem.valueLexema;
                if (input_data.Count > 0)
                {
                    nowLexem = lexer.GetLexem(ref input_data);
                }
                Node rightСhild = Term(ref input_data);
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

        public Node Term(ref List<byte> input_data)
        {
            Node leftСhild = Factor(ref input_data);
            if (leftСhild.type != "Error")
            {
                if (input_data.Count > 0)
                {
                    nowLexem = lexer.GetLexem(ref input_data);
                }
                while (nowLexem.valueLexema == "*" || nowLexem.valueLexema == "/")
                {
                    var BinOp = nowLexem.valueLexema;
                    if (input_data.Count > 0)
                    {
                        nowLexem = lexer.GetLexem(ref input_data);
                    }
                    Node rightСhild = Factor(ref input_data);
                    if (input_data.Count > 0)
                    {
                        nowLexem = lexer.GetLexem(ref input_data);
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

        public Node Factor(ref List<byte> input_data)
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
                if (input_data.Count > 0)
                {
                    nowLexem = lexer.GetLexem(ref input_data);
                    nextExpression = Expression(ref input_data);
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
                    if (input_data.Count != 0)
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