using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Lexer
{
    public class Lexema
    {
        public Lexema(int line_number, int symbol_number, LexemaType type, object value, string source)
        {
            Line_number = line_number;
            Symbol_number = symbol_number;
            Type = type;
            Value = value;
            Source = source;
        }
        public int Line_number { get; set; }
        public int Symbol_number { get; set; }
        public LexemaType Type;
        public object Value { get; }
        public string Source { get; }

        public override string ToString()
        {
            return $"{Line_number}\t{Symbol_number}\t{Type}\t{Value}\t{Source}";
        }

    }
    public enum LexemaType
    {
        REAL,
        INT,
        CHAR,
        STRING,
        IDENTIFIER,
        KEYWORD,
        OPERATOR,
        EOF,
        ERROR,
        SEPARATOR,
        NONE
    }
}
