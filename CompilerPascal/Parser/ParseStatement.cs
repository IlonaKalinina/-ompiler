using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public partial class Parser
    {
        public NodeStatement ParseStatement()
        {
            NodeStatement result = new NullStmt();

            if (ExpectType(LexemaType.IDENTIFIER))
            {
                result = ParseSimpleStatement();
                return result;
            }

            switch (currentLex.Value)
            {
                case KeyWord.BEGIN:
                    currentLex = lexer.GetLexem();
                    result = ParseBlock();
                    break;
                case KeyWord.IF:
                    result = ParseIf();
                    break;
                case KeyWord.FOR:
                    result = ParseFor();
                    break;
                case KeyWord.WHILE:
                    result = ParseWhile();
                    break;
                case KeyWord.REPEAT:
                    result = ParseRepeat();
                    break;
                case Separator.Semiсolon:
                    break;
                case KeyWord.EXIT:
                    currentLex = lexer.GetLexem();
                    break;
                default:
                    throw new Except(currentLex.Line_number, currentLex.Symbol_number, "expected statement");
            }
            return result;
        }

        public NodeStatement ParseSimpleStatement()
        {
            NodeExpression left;
            NodeExpression right;
            SymbolVar symVar;

            Symbol sym = symTableStack.Get((string)currentLex.Value);

            string value = (string)currentLex.Value;
            int line = currentLex.Line_number;
            int symbol = currentLex.Symbol_number;

            currentLex = lexer.GetLexem();
            if (sym.GetType() == typeof(SymProc))
            {
                return ParseProcedureStmt(value, line, symbol);
            }

            symVar = (SymbolVar)sym;
            left = new NodeVar(symVar);

            while (Expect(Separator.OpenSquareBracket, Separator.Dot))
            {
                Separator separator = (Separator)currentLex.Value;
                currentLex = lexer.GetLexem();
                switch (separator)
                {
                    case Separator.OpenSquareBracket:
                        left = ParsePositionArray(left, ref symVar);
                        break;
                    case Separator.Dot:
                        left = ParseRecordField(left, ref symVar);
                        break;
                }
            }
            if (!Expect(Operator.Assign, Operator.PlusEquality, Operator.MinusEquality, Operator.MultiplyEquality, Operator.DivideEquality))
            {
                throw new Except(currentLex.Line_number, currentLex.Symbol_number, $"Expected assigment sign");
            }
            Operator operation = (Operator)currentLex.Value;
            currentLex = lexer.GetLexem();
            right = ParseExpression();
            return new AssignmentStmt(operation, left, right);
        }
        public NodeExpression ParsePositionArray(NodeExpression node, ref SymbolVar var_)
        {
            SymArray array = new SymArray("", new List<OrdinalTypeNode>(), new SymInteger(""));
            List<NodeExpression> body = new List<NodeExpression>();
            bool bracketClose = false;
            bool end = false;

            body.Add(ParseSimpleExpression());
            while (Expect(Separator.Comma, Separator.CloseSquareBracket))
            {
                if (end)
                {
                    break;
                }
                switch (currentLex.Value)
                {
                    case Separator.CloseSquareBracket:
                        array = (SymArray)((NodeVar)node).GetSymbolVar().GetOriginalTypeVar();
                        var_ = new SymbolVar(var_.ToString(), array.GetTypeArray());
                        bracketClose = true;
                        currentLex = lexer.GetLexem();

                        if (Expect(Separator.OpenSquareBracket))
                        {
                            bracketClose = false;
                            currentLex = lexer.GetLexem();

                            body.Add(ParseSimpleExpression());
                        }
                        break;
                    case Separator.Comma:
                        if (!bracketClose)
                        {
                            currentLex = lexer.GetLexem();

                            body.Add(ParseSimpleExpression());
                        }
                        else
                        {
                            end = true;
                        }
                        break;
                }
            }
            if (!bracketClose)
            {
                throw new Except(currentLex.Line_number, currentLex.Symbol_number, "expected ']'");
            }
            return new NodeArrayPosition(var_.ToString(), array, body);
        }
        public NodeStatement ParseIf()
        {
            NodeExpression condition;
            NodeStatement  body = new NullStmt();
            NodeStatement  elseStatement = new NullStmt();

            currentLex = lexer.GetLexem();
            condition = ParseExpression();
            Require(KeyWord.THEN);

            if (!Expect(KeyWord.ELSE))
            {
                body = ParseStatement();
            }
            if (Expect(KeyWord.ELSE))
            {
                currentLex = lexer.GetLexem();
                elseStatement = ParseStatement();
            }
            return new IfStmt(condition, body, elseStatement);
        }
        public NodeStatement ParseFor()
        {
            KeyWord toOrDownto;
            NodeVar controlVar;
            NodeExpression initialValue;
            NodeExpression finalValue;
            NodeStatement body;

            currentLex = lexer.GetLexem();
            RequireType(LexemaType.IDENTIFIER);

            controlVar = new NodeVar((SymbolVar)symTableStack.Get((string)currentLex.Value));

            currentLex = lexer.GetLexem();
            Require(Operator.Assign); // :=
            initialValue = ParseSimpleExpression();

            if (!Expect(KeyWord.TO, KeyWord.DOWNTO))
            {
                throw new Except(currentLex.Line_number, currentLex.Symbol_number, "expected 'to' or 'downto'");
            }

            toOrDownto = (KeyWord)currentLex.Value;
            currentLex = lexer.GetLexem();
            finalValue = ParseSimpleExpression();

            Require(KeyWord.DO);

            body = ParseStatement();
            return new ForStmt(controlVar, initialValue, toOrDownto, finalValue, body);
        }
        public NodeStatement ParseWhile()
        {
            NodeExpression condition;
            NodeStatement body;

            currentLex = lexer.GetLexem();
            condition = ParseExpression();
            Require(KeyWord.DO);
            body = ParseStatement();
            return new WhileStmt(condition, body);
        }
        public NodeStatement ParseRepeat()
        {
            List<NodeStatement> body = new List<NodeStatement>();
            NodeExpression cond;

            do
            {
                currentLex = lexer.GetLexem();
                if (Expect(KeyWord.UNTIL))
                {
                    break;
                }
                body.Add(ParseStatement());
            } while (Expect(Separator.Semiсolon));

            Require(KeyWord.UNTIL);

            cond = ParseExpression();
            return new RepeatStmt(body, cond);
        }
        public NodeExpression ParseRecordField(NodeExpression node, ref SymbolVar var_)
        {
            if (!ExpectType(LexemaType.IDENTIFIER))
            {
                throw new Except(currentLex.Line_number, currentLex.Symbol_number, "expected Identifier");
            }
            SymRecord record = (SymRecord)var_.GetOriginalTypeVar();
            SymTable fields = record.GetFields();
            var_ = (SymbolVar)fields.Get((string)currentLex.Value);
            NodeExpression field = new NodeVar(var_);
            currentLex = lexer.GetLexem();
            return new NodeRecordAccess(Operator.DotRecord, node, field);
        }
        public NodeStatement ParseProcedureStmt(string name, int lineProc, int symProc)
        {
            List<NodeExpression?> parameter = new List<NodeExpression?>();
            SymProc proc;

            try
            {
                proc = (SymProc)symTableStack.Get(name);
            }
            catch
            {
                throw new Except(lineProc, symProc, $"Procedure not found \"{name}\"");
            }

            if (Expect(Separator.OpenBracket))
            {
                currentLex = lexer.GetLexem();
                while (!Expect(Separator.CloseBracket))
                {
                    NodeExpression param = ParseSimpleExpression();
                    parameter.Add(param);
                    if (Expect(Separator.Comma))
                    {
                        currentLex = lexer.GetLexem();
                    }
                    else
                    {
                        break;
                    }
                }
                Require(Separator.CloseBracket);
            }
            return new CallStmt(proc, parameter);
        }
    }
}
