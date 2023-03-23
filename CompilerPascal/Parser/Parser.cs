using System.Collections.Generic;

namespace CompilerPascal
{
    public partial class Parser
    {
        Lexer lexer;
        Lexema currentLex;
        SymTableStack symTableStack;

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
            symTableStack = new SymTableStack();
            Dictionary<string, Symbol> builtins = new Dictionary<string, Symbol>();
            builtins.Add("integer", new SymInteger("integer"));
            builtins.Add("real", new SymReal("real"));
            builtins.Add("string", new SymString("string"));
            builtins.Add("write", new SymProc("write"));
            builtins.Add("read", new SymProc("read"));
            symTableStack.AddTable(new SymTable(builtins));
            symTableStack.AddTable(new SymTable(new Dictionary<string, Symbol>()));

            currentLex = lexer.GetLexem();
        }

        private bool Expect(params object[] requires)
        {
            foreach (object require in requires)
            {
                if (Equals(currentLex.Value, require))
                {
                    return true;
                }
            }
            return false;
        }
        private void Require(object require)
        {
            if (!Expect(require))
            {
                if (require is Separator req)
                {
                    require = Lexer.ConvertEnumSeparator(req);
                }
                if (require.GetType() == typeof(Operator))
                {
                    require = Lexer.ConvertEnumOperator((Operator)require);
                }
                throw new Except(currentLex.Line_number, currentLex.Symbol_number, $"expected '{require}'");
            }
            currentLex = lexer.GetLexem();
        }
        private bool ExpectType(params LexemaType[] types)
        {
            foreach (LexemaType type in types)
            {
                if (currentLex.Type == type)
                {
                    return true;
                }
            }
            return false;
        }
        private void RequireType(LexemaType type)
        {
            if (!ExpectType(type))
            {
                throw new Except(currentLex.Line_number, currentLex.Symbol_number, $"expected {type}");
            }
        }
        private void NotRequireType(LexemaType type)
        {
            if (ExpectType(type))
            {
                throw new Except(currentLex.Line_number, currentLex.Symbol_number, $"expected {type}");
            }
        }

        public Node ParseMainProgram()
        {
            string name = "";
            List<NodeDescriptions> types;
            BlockStmt body;

            if (Expect(KeyWord.PROGRAM))
            {
                currentLex = lexer.GetLexem();
                name = ParseProgramName();
            }

            types = ParseDescription();
            Require(KeyWord.BEGIN);
            body = ParseBlock();
            Require(Separator.Dot);
            return new NodeMainProgram(name, types, body);
        }
    }
}