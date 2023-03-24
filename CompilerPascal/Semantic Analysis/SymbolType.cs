using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompilerPascal
{
    public class SymbolType : Symbol
    {
        public SymbolType(string name) : base(name) { }

        public override string ToString(string str_1)
        {
            return $"{ToString()}";
        }
    }
    public class SymInteger : SymbolType
    {
        public SymInteger(string name) : base(name) { }
    }
    public class SymReal : SymbolType
    {
        public SymReal(string name) : base(name) { }
    }
    public class SymString : SymbolType
    {
        public SymString(string name) : base(name) { }
    }
    public class SymProc : SymbolType
    {
        public SymProc(string name) : base(name) { }
    }
    public class SymBoolean : SymbolType
    {
        public SymBoolean(string name) : base(name) { }
    }
    public class SymArray : SymbolType
    {
        List<OrdinalTypeNode> ordinalTypes;
        SymbolType type;
        public SymbolType GetTypeArray()
        {
            return type;
        }
        public List<OrdinalTypeNode> GetOrdinalTypeNode()
        {
            return ordinalTypes;
        }
        public SymArray(string name, List<OrdinalTypeNode> ordinalTypes, SymbolType type) : base(name)
        {
            this.ordinalTypes = ordinalTypes;
            this.type = type;
        }

        public override string ToString(string indent)
        {
            string result = null;

            result += "array\r\n";
            foreach (OrdinalTypeNode ordinalType in ordinalTypes)
            {
                result += indent + $"├─── {ordinalType.ToString(indent + "│    ")}\r\n";
            }
            result += indent + $"└─── {type.ToString(indent + "     ")}";
            return result;
        }
    }
    public class OrdinalTypeNode : Node
    {
        NodeExpression from;
        NodeExpression to;
        public OrdinalTypeNode(NodeExpression from, NodeExpression to)
        {
            this.from = from;
            this.to = to;
        }

        public override string ToString(string indent)
        {
            string result = null;
            result += "..\r\n";
            result += indent + $"├─── {from.ToString(indent + "│    ")}\r\n";
            result += indent + $"└─── {to.ToString(indent + "     ")}";
            return result;
        }
    }
    public class SymRecord : SymbolType
    {
        SymTable fields;
        public SymTable GetFields()
        {
            return fields;
        }
        public SymRecord(string name, SymTable fields) : base(name)
        {
            this.fields = fields;
        }

        public override string ToString(string indent)
        {
            string result;
            result = $"record \r\n";
            List<Symbol> symFields = new List<Symbol>(fields.GetData().Values);
            int i = 1;
            foreach (Symbol field in symFields)
            {
                SymbolVar varField = (SymbolVar)field;
                if (i == symFields.Count)
                {
                    result += indent + $"└─── {varField}\r\n";
                    result += indent + $"     └─── {varField.GetOriginalTypeVar().ToString(indent + indent + "     ")}";
                }
                else
                {
                    result += indent + $"├─── {varField}\r\n";
                    result += indent + $"│    └─── {varField.GetOriginalTypeVar().ToString(indent + indent + "│    ")}\r\n";
                    i++;
                }
            }
            return result;
        }
    }
    public class SymTypeAlias : SymbolType
    {
        SymbolType original;
        public SymbolType GetOriginalType()
        {
            return original;
        }
        public SymTypeAlias(string name, SymbolType original) : base(name)
        {
            this.original = original;
        }
        public override string ToString(string indent) 
        {
            string result = null;
            string name = ToString();

            result += $"{name}\r\n";
            result += indent + $"└─── {original.ToString(indent + "     ")}";

            return result;
        }
    }
}
