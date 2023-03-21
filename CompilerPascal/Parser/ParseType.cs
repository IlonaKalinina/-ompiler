using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public partial class Parser
    {
        public SymbolType ParseType()
        {
            if (!(ExpectType(LexemaType.IDENTIFIER) || ExpectType(LexemaType.KEYWORD)))
            {
                throw new Except(currentLex.Line_number, currentLex.Symbol_number, "expected type variable");
            }
            Lexema type = currentLex;
            currentLex = lexer.GetLexem();
            switch (type.Value)
            {
                case KeyWord.ARRAY:
                    return ParseArrayType();
                case KeyWord.RECORD:
                    return ParseRecordType();
                case KeyWord.STRING:
                    return (SymbolType)symTableStack.Get("string");
                case "integer":
                    return (SymbolType)symTableStack.Get("integer");
                case "real":
                    return (SymbolType)symTableStack.Get("real");
                default:
                    SymbolType original;
                    try
                    {
                        Symbol sym = symTableStack.Get((string)type.Value);
                        original = (SymbolType)sym;
                    }
                    catch
                    {
                        throw new Except(currentLex.Line_number, currentLex.Symbol_number, $"Identifier not found \"{type.Source}\"");
                    }
                    return new SymTypeAlias((string)type.Value, original);
            }
        }
        public SymbolType ParseArrayType()
        {
            SymbolType type;
            List<OrdinalTypeNode> ordinalTypes = new List<OrdinalTypeNode>();

            if (Expect(Separator.OpenSquareBracket))
            {
                currentLex = lexer.GetLexem();
                ordinalTypes.Add(ParseArrayOrdinalType());

                while (Expect(Separator.Comma))
                {
                    currentLex = lexer.GetLexem();
                    ordinalTypes.Add(ParseArrayOrdinalType());
                }
            }
            else
            {
                throw new Except(currentLex.Line_number, currentLex.Symbol_number, "expected '['");
            }
            Require(Separator.CloseSquareBracket);
            Require(KeyWord.OF);

            if (ExpectType(LexemaType.IDENTIFIER) || ExpectType(LexemaType.KEYWORD))
            {
                type = ParseType();
            }
            else
            {
                throw new Except(currentLex.Line_number, currentLex.Symbol_number, "expected type");
            }

            return new SymArray("array", ordinalTypes, type);
        }
        public OrdinalTypeNode ParseArrayOrdinalType()
        {
            NodeExpression from = ParseSimpleExpression();
            Require(Separator.DoubleDot);
            NodeExpression to = ParseSimpleExpression();

            return new OrdinalTypeNode(from, to);
        }
        public SymbolType ParseRecordType()
        {
            SymTable fields = new SymTable(new Dictionary<string, Symbol>());
            symTableStack.AddTable(fields);

            while (ExpectType(LexemaType.IDENTIFIER))
            {
                ParseVariableDescription();
                if (Expect(KeyWord.END))
                {
                    break;
                }
                Require(Separator.Semiсolon);
            }

            Require(KeyWord.END);
            symTableStack.PopBack();
            return new SymRecord("record", fields);
        }
    }
}
