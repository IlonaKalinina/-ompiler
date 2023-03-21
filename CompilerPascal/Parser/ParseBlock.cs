using System.Collections.Generic;

namespace CompilerPascal
{
    public partial class Parser
    {
        public BlockStmt ParseBlock()
        {
            List<NodeStatement> body = new List<NodeStatement>();
            while (!Expect(KeyWord.END))
            {
                body.Add(ParseStatement());
                if (Expect(Separator.Semiсolon))
                {
                    currentLex = lexer.GetLexem();
                    continue;
                }
                if (!Expect(KeyWord.END))
                {
                    throw new Except(currentLex.Line_number, currentLex.Symbol_number, "expected ';'");
                }
            }
            Require(KeyWord.END);
            return new BlockStmt(body);
        }
    }
}
