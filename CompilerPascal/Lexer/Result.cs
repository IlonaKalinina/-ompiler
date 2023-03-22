using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public partial class Lexer
    {
        public static Operator GetEnumOperator(string operation)
        {
            switch (operation)
            {
                case "=":
                    return Operator.Equal;
                case ":":
                    return Operator.Colon;
                case "+":
                    return Operator.Plus;
                case "-":
                    return Operator.Minus;
                case "*":
                    return Operator.Multiply;
                case "/":
                    return Operator.Divide;
                case ">":
                    return Operator.Greater;
                case "<":
                    return Operator.Less;
                case "<<":
                    return Operator.BitwiseShiftToTheLeft;
                case ">>":
                    return Operator.BitwiseShiftToTheRight;
                case "<>":
                    return Operator.NotEqual;
                case "><":
                    return Operator.SymmetricalDifference;
                case "<=":
                    return Operator.LessOrEqual;
                case ">=":
                    return Operator.GreaterOrEqual;
                case ":=":
                    return Operator.Assign;
                case "+=":
                    return Operator.PlusEquality;
                case "-=":
                    return Operator.MinusEquality;
                case "*=":
                    return Operator.MultiplyEquality;
                case "/=":
                    return Operator.DivideEquality;
                case ".":
                    return Operator.DotRecord;
                default:
                    return Operator.Unidentified;
            }
        }
        public static Separator GetEnumSeparator(string separator)
        {
            switch (separator)
            {
                case ",":
                    return Separator.Comma;
                case ";":
                    return Separator.Semiсolon;
                case "(":
                    return Separator.OpenBracket;
                case ")":
                    return Separator.CloseBracket;
                case "[":
                    return Separator.OpenSquareBracket;
                case "]":
                    return Separator.CloseSquareBracket;
                case ".":
                    return Separator.Dot;
                case "..":
                    return Separator.DoubleDot;
                case "@":
                    return Separator.At;
                default:
                    return Separator.Unidentified;
            }
        }
        public static string ConvertEnumOperator(Operator operation)
        {
            switch (operation)
            {
                case Operator.Equal:
                    return "=";
                case Operator.Colon:
                    return ":";
                case Operator.Plus:
                    return "+";
                case Operator.Minus:
                    return "-";
                case Operator.Multiply:
                    return "*";
                case Operator.Divide:
                    return "/";
                case Operator.Greater:
                    return ">";
                case Operator.Less:
                    return "<";
                case Operator.BitwiseShiftToTheLeft:
                    return "<<";
                case Operator.BitwiseShiftToTheRight:
                    return ">>";
                case Operator.NotEqual:
                    return "<>";
                case Operator.SymmetricalDifference:
                    return "><";
                case Operator.LessOrEqual:
                    return "<=";
                case Operator.GreaterOrEqual:
                    return ">=";
                case Operator.Assign:
                    return ":=";
                case Operator.PlusEquality:
                    return "+=";
                case Operator.MinusEquality:
                    return "-=";
                case Operator.MultiplyEquality:
                    return "*=";
                case Operator.DivideEquality:
                    return "/=";
                case Operator.DotRecord:
                    return ".";
                default:
                    return "";
            }
        }
        public static string ConvertEnumSeparator(Separator separator)
        {
            switch (separator)
            {
                case Separator.Comma:
                    return ",";
                case Separator.Semiсolon:
                    return ";";
                case Separator.OpenBracket:
                    return "(";
                case Separator.CloseBracket:
                    return ")";
                case Separator.OpenSquareBracket:
                    return "[";
                case Separator.CloseSquareBracket:
                    return "]";
                case Separator.Dot:
                    return ".";
                case Separator.DoubleDot:
                    return "..";
                case Separator.At:
                    return "@";
                default:
                    return "";
            }
        }

        public static void Result()
        {
            symbol_number = flag + 1;

            object newValueLexema = null;

            switch (type)
            {
                case (LexemaType.INT):
                    newValueLexema = int.Parse(value);
                    break;
                case (LexemaType.REAL):
                    value = value.Replace(".", ",");
                    newValueLexema = double.Parse(value);
                    break;
                case (LexemaType.OPERATOR):
                    newValueLexema = GetEnumOperator(value);
                    break;
                case (LexemaType.SEPARATOR):
                    newValueLexema = GetEnumSeparator(value);
                    break;
                case (LexemaType.KEYWORD):
                    //newValueLexema = GetEnumKeyword(value);
                    Enum.TryParse(value.ToUpper(), out KeyWord res);
                    newValueLexema = res;
                    break;
                default:
                    newValueLexema = value;
                    break;
            }
            foundlexem = new Lexema(line_number, symbol_number, type, newValueLexema, temp);

            if (temp != null) flag += temp.Length;
            temp = null;
            value = null;
            type = LexemaType.NONE;
        }
    }
}
