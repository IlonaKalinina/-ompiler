using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompilerPascal
{
    public class NodeExpression : Node
    {
        private protected SymbolType cachedType = new SymbolType("");
        public SymbolType GetCachedType()
        {
            return cachedType;
        }
        public virtual SymbolType CalcType()
        {
            return new SymbolType("");
        }
    }
    public class NodeCast : NodeExpression
    {
        SymbolType cast;
        NodeExpression exp;
        public NodeCast(SymbolType cast, NodeExpression exp)
        {
            this.cast = cast;
            this.exp = exp;
        }
        public override string ToString(string indent)
        {
            string result = null;
            result += cast + "\r\n";
            result += indent + $"└─── {exp.ToString(indent + "     ")}";
            return result;
        }
    }
    public class NodeBinOp : NodeExpression
    {
        object opname;
        NodeExpression left;
        NodeExpression right;
        public NodeBinOp(object opname, NodeExpression left, NodeExpression right)
        {
            this.opname = opname;
            this.left = left;
            this.right = right;
            cachedType = CalcType();
        }
        public override SymbolType CalcType()
        {
            SymbolType leftType = left.GetCachedType();
            SymbolType rightType = right.GetCachedType();
            Operator BinOp = (Operator)opname;

            if (leftType.GetType() != rightType.GetType() && BinOp != Operator.DotRecord)
            {
                if ((leftType.GetType() == typeof(SymInteger) || leftType.GetType() == typeof(SymReal)) &&
                   (rightType.GetType() == typeof(SymInteger) || rightType.GetType() == typeof(SymReal)))
                {
                    if (leftType.GetType() == typeof(SymInteger))
                    {
                        left = new NodeCast(rightType, left);
                    }
                    else
                    {
                        right = new NodeCast(leftType, right);
                    }
                    return new SymReal("real");
                }
                else
                {
                    throw new Exception($"Incompatible types");
                }
            }
            if (opname.GetType() == typeof(Operator))
            {
                if (BinOp == Operator.Equal || BinOp == Operator.Less || BinOp == Operator.LessOrEqual ||
                    BinOp == Operator.Greater || BinOp == Operator.GreaterOrEqual || BinOp == Operator.NotEqual)
                {
                    return new SymBoolean("boolean");
                }
                if ((BinOp == Operator.Minus || BinOp == Operator.Multiply || BinOp == Operator.Divide ||
                    BinOp == Operator.MinusEquality || BinOp == Operator.MultiplyEquality)
                    && leftType.GetType() == typeof(SymString))
                {
                    throw new Exception("Operator is not overloaded");
                }
            }
            return leftType;
        }

        public override string ToString(string indent)
        {
            string result = null;
            string binOpName = opname.ToString().ToLower();

            if (opname.GetType() == typeof(Operator))
            {
                binOpName = Lexer.ConvertEnumOperator((Operator)opname);
            }
            result += binOpName + "\r\n";
            result += $"│         ├─── {left.ToString(indent + "│    ")}\r\n";
            result += $"│         └─── {right.ToString(indent + "     ")}";
            return result;
        }
    }
    public class NodeRecordAccess : NodeBinOp
    {
        public NodeRecordAccess(Operator opname, NodeExpression left, NodeExpression right) : base(opname, left, right) { }
    }
    public class NodeUnOp : NodeExpression
    {
        object opname;
        NodeExpression arg;
        public NodeUnOp(object opname, NodeExpression arg)
        {
            this.opname = opname;
            this.arg = arg;
            cachedType = CalcType();
        }
        public override SymbolType CalcType()
        {
            return arg.CalcType();
        }
        public override string ToString(string indent)
        {
            string result = null;
            string UnOpName = opname.ToString().ToLower();

            if (opname.GetType() == typeof(Operator))
            {
                UnOpName = Lexer.ConvertEnumOperator((Operator)opname);
            }
            result += UnOpName + "\r\n";
            result += indent + $"└─── {arg.ToString(indent + "     ")}";
            return result;
        }
    }
    public class NodeArrayPosition : NodeExpression
    {
        string name;
        SymArray symArray;
        public List<NodeExpression> args;
        public string GetName()
        {
            return name;
        }
        public NodeArrayPosition(string name, SymArray symArray, List<NodeExpression> arg)
        {
            this.name = name;
            this.symArray = symArray;
            this.args = arg;
            cachedType = CalcType();
        }
        public override SymbolType CalcType()
        {
            SymbolType res = symArray.GetTypeArray();
            while (res.GetType() == typeof(SymArray))
            {
                res = ((SymArray)res).GetTypeArray();
            }
            return res;
        }
        public override string ToString(string indent)
        {
            string result = null;
            result += $"{name}\r\n";

            if (args != null)
            {
                foreach (NodeExpression arg in args)
                {
                    if (arg == args.Last())
                    {
                        result += indent + $"│    └─── {arg.ToString(indent + "│    ")}";
                    }
                    else
                    {
                        result += indent + $"│    ├─── {arg.ToString(indent + "     ")}\r\n";
                    }
                }
            }
            return result;
        }
    }
    public class NodeVar : NodeExpression
    {
        SymbolVar var_;
        public string GetName()
        {
            return var_.ToString();
        }
        public SymbolVar GetSymbolVar()
        {
            return var_;
        }
        public NodeVar(SymbolVar var_)
        {
            this.var_ = var_;
            cachedType = CalcType();
        }
        public override SymbolType CalcType()
        {
            return var_.GetTypeVar();
        }
        public override string ToString(string indent)
        {
            return $"{var_}";
        }
    }
    public class NodeInt : NodeExpression
    {
        int value;
        public NodeInt(int value)
        {
            this.value = value;
            cachedType = CalcType();
        }
        public override SymbolType CalcType()
        {
            return new SymInteger("integer");
        }

        public override string ToString(string indent)
        {
            return value.ToString();
        }
    }
    public class NodeReal : NodeExpression
    {
        double value;
        public NodeReal(double value)
        {
            this.value = value;
            cachedType = CalcType();
        }
        public override SymbolType CalcType()
        {
            return new SymReal("real");
        }
        public override string ToString(string indent)
        {
            return value.ToString();
        }
    }
    public class NodeString : NodeExpression
    {
        string value;
        public NodeString(string value)
        {
            this.value = value;
            cachedType = CalcType();
        }
        public override SymbolType CalcType()
        {
            return new SymString("string");
        }
        public override string ToString(string indent)
        {
            return value;
        }
    }
}
