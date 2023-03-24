using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
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
        public int Line_number;
        public int Symbol_number;
        public LexemaType Type;
        public object Value;
        public string Source;

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
        COMM,
        NONE
    }

    public enum Separator
    {
        Unidentified,
        Comma,              // ,
        Semiсolon,          // ;
        Dot,                // .
        DoubleDot,          // ..
        OpenBracket,        // (
        CloseBracket,       // )
        OpenSquareBracket,  // [
        CloseSquareBracket, // ]
        At,                 // @
    }

    public enum Operator
    {
        Unidentified,
        Equal,                  // =
        Colon,                  // :
        Plus,                   // +
        Minus,                  // -
        Multiply,               // *
        Divide,                 // /
        Greater,                // >
        Less,                   // <
        BitwiseShiftToTheLeft,  // <<
        BitwiseShiftToTheRight, // >>
        NotEqual,               // <>
        SymmetricalDifference,  // ><
        LessOrEqual,            // <=
        GreaterOrEqual,         // >=
        Assign,                 // :=
        PlusEquality,           // +=
        MinusEquality,          // -=
        MultiplyEquality,       // *=
        DivideEquality,         // /=
        DotRecord,              // .
    }

    public enum KeyWord
    {
        AND,
        ARRAY,
        AS,
        ASM,
        BEGIN,
        CASE,
        CONST,
        CONSTRUCTOR,
        DESTRUCTOR,
        DIV,
        DO,
        DOWNTO,
        ELSE,
        END,
        FILE,
        FOR,
        FOREACH,
        FUNCTION,
        GOTO,
        IMPLEMENTATION,
        IF,
        IN,
        INHERITED,
        INLINE,
        INTERFACE,
        LABEL,
        MOD,
        NIL,
        NOT,
        OBJECT,
        OF,
        OPERATOR,
        OR,
        PACKED,
        PROCEDURE,
        PROGRAM,
        RECORD,
        REPEAT,
        SELF,
        SET,
        SHL,
        SHR,
        STRING,
        THEN,
        TO,
        TYPE,
        UNIT,
        UNTIL,
        USES,
        VAR,
        WHILE,
        WITH,
        XOR,
        DISPOSE,
        EXIT,
        FALSE,
        NEW,
        TRUE,
        CLASS,
        DISPINTERFACE,
        EXCEPT,
        EXPORTS,
        FINALIZATION,
        FINALLY,
        INITIALIZATION,
        IS,
        LIBRARY,
        ON,
        OUT,
        PROPERTY,
        RAISE,
        RESOURCESTRING,
        THREADVAR,
        TRY
    }
}
