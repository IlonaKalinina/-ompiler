using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    public partial class Lexer
    {
        public static string[] keyWords = {"absolute", "and", "array", "as", "asm", "begin", "break", "case",  "class", "const", "constructor", "destructor",
                                    "dispinterface", "div", "do", "downto", "else", "end", "except", "exports", "false", "file", "for", "function", "finalization",
                                    "finally", "goto", "if", "implementation", "initialization", "inline", "is", "library", "on", "out",
                                    "in", "inherited", "inline", "interface", "label", "library", "nil", "not", "object", "of", "or",
                                    "packed", "property", "procedure", "program", "record", "raise", "resourcestring", "threadvar", "try",
                                    "repeat", "set", "shl", "shr", "string", "then", "to", "type", "true", "unit", "until", "uses", "var", "while", "with", "xor" };
        public static void KeyWord()
        {
            for (int j = 0; j < keyWords.Length; j++)
            {
                if (temp.ToLower() == keyWords[j])
                {
                    type = LexemaType.KEYWORD;
                }
            }
        }
    }
}
