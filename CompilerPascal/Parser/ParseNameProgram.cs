namespace CompilerPascal
{
    public partial class Parser
    {
        // program_name ::= 'porgam' id ';'
        public string ParseProgramName()
        {
            string name;
            if (ExpectType(LexemaType.IDENTIFIER))
            {
                name = (string)currentLex.Value;
                currentLex = lexer.GetLexem();
            }
            else
            {
                throw new Except(currentLex.Line_number, currentLex.Symbol_number, $"expected {LexemaType.IDENTIFIER}");
            }
            Require(Separator.Semiсolon); // ;
            return name;
        }
    }
}