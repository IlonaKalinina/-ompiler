using System.Collections.Generic;

namespace CompilerPascal
{
    public partial class Parser
    {
        public NodeExpression ParseExpression(bool inDef = false)
        {
            NodeExpression left = ParseSimpleExpression();
            while (Expect(Operator.Less, Operator.LessOrEqual, Operator.Greater, Operator.GreaterOrEqual, Operator.Equal, Operator.NotEqual))
            {
                Operator operation = (Operator)currentLex.Value;
                currentLex = lexer.GetLexem();
                NodeExpression right = ParseSimpleExpression();
                left = new NodeBinOp(operation, left, right);
            }
            return left;
        }

        public NodeExpression ParseSimpleExpression(bool inDef = false)
        {
            NodeExpression left = ParseTerm();
            while (Expect(Operator.Plus, Operator.Minus, KeyWord.OR, KeyWord.XOR))
            {
                object operation = currentLex.Value;
                currentLex = lexer.GetLexem();
                NodeExpression right = ParseTerm();
                left = new NodeBinOp(operation, left, right);
            }
            return left;
        }
        public NodeExpression ParseTerm(bool inDef = false)
        {
            NodeExpression left = ParseFactor();
            while (Expect(Operator.Multiply, Operator.Divide, KeyWord.AND))
            {
                object operation = currentLex.Value;
                currentLex = lexer.GetLexem();
                NodeExpression right = ParseFactor();
                left = new NodeBinOp(operation, left, right);
            }
            return left;
        }
        public NodeExpression ParseFactor(bool inDef = false)
        {
            if (Expect(Separator.OpenBracket))
            {
                NodeExpression e;
                currentLex = lexer.GetLexem();
                e = ParseExpression();
                Require(Separator.CloseBracket);
                return e;
            }
            if (ExpectType(LexemaType.INT))
            {
                Lexema factor = currentLex;
                currentLex = lexer.GetLexem();
                return new NodeInt((int)factor.Value);
            }
            if (ExpectType(LexemaType.STRING))
            {
                Lexema factor = currentLex;
                currentLex = lexer.GetLexem();
                return new NodeString((string)factor.Value);
            }
            if (ExpectType(LexemaType.REAL))
            {
                Lexema factor = currentLex;
                currentLex = lexer.GetLexem();
                return new NodeReal((double)factor.Value);
            }
            if (ExpectType(LexemaType.IDENTIFIER))
            {
                NodeExpression ans;
                Lexema factor = currentLex;
                currentLex = lexer.GetLexem();
                SymbolVar symVar;
                try
                {
                    symVar = (SymbolVar)symTableStack.Get((string)factor.Value);
                }
                catch
                {
                    throw new Except(currentLex.Line_number, currentLex.Symbol_number, $"Identifier not found \"{factor.Value}\"");
                }
                ans = new NodeVar(symVar);
                while (Expect(Separator.OpenBracket, Separator.Dot))
                {
                    Separator separator = (Separator)currentLex.Value;
                    currentLex = lexer.GetLexem();
                    switch (separator)
                    {
                        case Separator.OpenBracket:
                            ans = ParsePositionArray(ans, ref symVar);
                            break;
                        case Separator.Dot:
                            ans = ParseRecordField(ans, ref symVar);
                            break;
                    }
                }
                return ans;
            }
            if (Expect(Operator.Plus, Operator.Minus, KeyWord.NOT))
            {
                object unOp = currentLex.Value;
                currentLex = lexer.GetLexem();
                NodeExpression factor = ParseFactor();
                return new NodeUnOp(unOp, factor);
            }
            throw new Except(currentLex.Line_number, currentLex.Symbol_number, "expected factor");
        }
    }
}
