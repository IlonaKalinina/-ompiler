using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public partial class SimpleParser
    {
        readonly Lexer lexer;
        Lexema nowLexem;

        public SimpleParser(Lexer lexer)
        {
            this.lexer = lexer;
            nowLexem = lexer.GetLexem();
        }
        public SimpleNode Expression()
        {
            SimpleNode leftСhild = Term();
            if (nowLexem.Type != LexemaType.EOF)
            {
                while (nowLexem.Type == LexemaType.OPERATOR &&
               ((Operator)nowLexem.Value == Operator.Plus || (Operator)nowLexem.Value == Operator.Minus))
                {
                    leftСhild = AddChilde(leftСhild);
                    if (nowLexem.Type == LexemaType.EOF) break;
                }
            }
            return leftСhild;
        }

        public SimpleNode Term()
        {
            SimpleNode leftСhild = Factor();
            if (nowLexem.Type != LexemaType.EOF)
            {
                while (nowLexem.Type == LexemaType.OPERATOR &&
                ((Operator)nowLexem.Value == Operator.Multiply || (Operator)nowLexem.Value == Operator.Divide))
                {
                    leftСhild = AddChilde(leftСhild);
                    if (nowLexem.Type == LexemaType.EOF) break;
                }
            }
            return leftСhild;
        }

        public SimpleNode Factor()
        {
            if (nowLexem.Type == LexemaType.INT)
            {
                SimpleNode nowChilde = new SimpleNode()
                {
                    type = LexemaType.INT,
                    value = nowLexem.Value
                };
                if (nowLexem.Type != LexemaType.EOF) nowLexem = lexer.GetLexem();
                return nowChilde;
            }
            if (nowLexem.Type == LexemaType.REAL)
            {
                SimpleNode nowChilde = new SimpleNode()
                {
                    type = LexemaType.REAL,
                    value = nowLexem.Value
                };
                if (nowLexem.Type != LexemaType.EOF) nowLexem = lexer.GetLexem();
                return nowChilde;
            }
            if (nowLexem.Type == LexemaType.IDENTIFIER)
            {
                SimpleNode nowChilde = new SimpleNode()
                {
                    type = LexemaType.IDENTIFIER,
                    value = nowLexem.Value
                };
                if (nowLexem.Type != LexemaType.EOF) nowLexem = lexer.GetLexem();
                return nowChilde;
            }
            if (nowLexem.Type == LexemaType.SEPARATOR && (Separator)nowLexem.Value == Separator.OpenBracket)
            {
                nowLexem = lexer.GetLexem();
                SimpleNode nextExpression = Expression();

                if (nowLexem.Type == LexemaType.EOF)
                {
                    throw new Except(nowLexem.Line_number, nowLexem.Symbol_number, "expected right bracket");
                }
                else
                {
                    nowLexem = lexer.GetLexem();
                    return nextExpression;
                }
            }

            throw new Except(nowLexem.Line_number, nowLexem.Symbol_number, "expected factor");
        }

        public SimpleNode AddChilde(SimpleNode leftСhild)
        {
            var BinOp = nowLexem.Value;
            if (nowLexem.Type != LexemaType.EOF)
            {
                nowLexem = lexer.GetLexem();
            }
            leftСhild = new SimpleNode()
            {
                type = LexemaType.OPERATOR,
                value = BinOp,
                children = new List<SimpleNode> {leftСhild, Term()}
            };
            if (leftСhild.children[1].type == LexemaType.ERROR)
            {
                return new SimpleNode()
                {
                    type = LexemaType.ERROR,
                    value = leftСhild.children[1].value
                };
            }
            return leftСhild;
        }
    }
}
