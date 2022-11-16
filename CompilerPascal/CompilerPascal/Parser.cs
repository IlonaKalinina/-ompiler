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
        int openBracketCount = 0;
        public Node resParser = new Node();

        public void ReadFileParser(string path)
        {
            resParser = new Node();
            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] textFromFile = new byte[fstream.Length];
                fstream.Read(textFromFile);

                List<byte> text = new List<byte>();
                for (int i = 0; i < textFromFile.Length; i++)
                {
                    text.Add(textFromFile[i]);
                }
                nowLexem = lexer.GetLexem(ref text);
                resParser = Expression(ref text);
            }
            return;
        }
        public Node Expression(ref List<byte> text)
        {
            Node leftСhild = Term(ref text);
            while (nowLexem.valueLexema == "+" || nowLexem.valueLexema == "-")
            {
                var operation = nowLexem.valueLexema;
                if (text.Count > 0)
                {
                    nowLexem = lexer.GetLexem(ref text);
                }
                Node rightСhild = Term(ref text);
                leftСhild = new Node()
                {
                    type = "BinOp",
                    value = operation,
                    children = new List<Node?> { leftСhild, rightСhild }
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

        public Node Term(ref List<byte> text)
        {
            Node leftСhild = Factor(ref text);
            if (leftСhild.type != "Error")
            {
                nowLexem = lexer.GetLexem(ref text);
                while (nowLexem.valueLexema == "*" || nowLexem.valueLexema == "/")
                {
                    var operation = nowLexem.valueLexema;
                    if (text.Count > 0)
                    {
                        nowLexem = lexer.GetLexem(ref text);
                    }
                    Node rightСhild = Factor(ref text);
                    if (text.Count > 0)
                    {
                        nowLexem = lexer.GetLexem(ref text);
                    }
                    leftСhild = new Node()
                    {
                        type = "BinOp",
                        value = operation,
                        children = new List<Node?> { leftСhild, rightСhild }
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

        public Node Factor(ref List<byte> text)
        {
            if (nowLexem.categoryLexeme == "integer" || nowLexem.categoryLexeme == "real")
            {
                Lexema factor = nowLexem;
                return new Node()
                {
                    type = "Number",
                    value = factor.valueLexema
                };
            }
            if (nowLexem.categoryLexeme == "integer")
            {
                Lexema factor = nowLexem;
                return new Node()
                {
                    type = "Identifier",
                    value = factor.valueLexema
                };
            }
            if (nowLexem.valueLexema == "(")
            {
                Node newExp = new Node();
                if (text.Count > 0)
                {
                    nowLexem = lexer.GetLexem(ref text);
                    newExp = Expression(ref text);
                }
                else
                {
                    newExp = new Node()
                    {
                        type = "Error",
                        value = $"Syntax error on line {nowLexem.numberLine}, \")\" expected"
                    };
                }
                if (nowLexem.valueLexema != ")")
                {
                    if (text.Count != 0)
                    {
                        openBracketCount += 1;
                    }
                    else
                    {
                        newExp = new Node()
                        {
                            type = "Error",
                            value = $"Syntax error on line {nowLexem.numberLine}, \")\" expected"
                        };
                    }
                }
                return newExp;
            }
            return new Node()
            {
                type = "Error",
                value = $"Syntax error on line {nowLexem.numberLine}, don't have factor"
            };
        }
    }
}