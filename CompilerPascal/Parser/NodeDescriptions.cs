using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompilerPascal
{
    public class NodeDescriptions : Node {}
    public class VarTypesNode : NodeDescriptions
    {
        List<VarDeclarationNode> body;
        public VarTypesNode(List<VarDeclarationNode> body)
        {
            this.body = body;
        }

        public override string ToString(string indent)
        {
            string result;
            result = $"var\r\n";
            int i = 1;
            foreach (VarDeclarationNode el in body)
            {
                if (i == body.Count)
                {
                    if (el != null)
                    {
                        result += indent + $"└─── {el.ToString(indent + "     ")}";
                    }
                }
                else
                {
                    if (el != null)
                    {
                        result += indent + $"├─── {el.ToString(indent + "│    ")}\r\n";
                    }
                    i++;
                }
            }
            return result;
        }
    }
    public class TypeTypesNode : NodeDescriptions
    {
        List<DeclarationNode> body;
        public TypeTypesNode(List<DeclarationNode> body)
        {
            this.body = body;
        }

        public override string ToString(string indent)
        {
            string result;
            result = $"type\r\n";
            int i = 1;
            foreach (DeclarationNode el in body)
            {
                if (i == body.Count)
                {
                    if (el != null)
                    {
                        result += indent + $"└─── {el.ToString(indent + "     ")}";
                    }
                }
                else
                {
                    if (el != null)
                    {
                        result += indent + $"├─── {el.ToString(indent + "│    ")}\r\n";
                    }
                    i++;
                }
            }
            return result;
        }
    }
    public class ConstTypesNode : NodeDescriptions
    {
        List<ConstDeclarationNode> body;
        public ConstTypesNode(List<ConstDeclarationNode> body)
        {
            this.body = body;
        }

        public override string ToString(string indent)
        {
            string result;
            result = $"const\r\n";
            int i = 1;
            foreach (ConstDeclarationNode el in body)
            {
                if (i == body.Count)
                {
                    if (el != null)
                    {
                        result += indent + $"└─── {el.ToString(indent + "     ")}";
                    }
                }
                else
                {
                    if (el != null)
                    {
                        result += indent + $"├─── {el.ToString(indent + "│    ")}\r\n";
                    }
                    i++;
                }
            }
            return result;
        }
    }

    public class DeclarationNode : Node { }
    public class VarDeclarationNode : DeclarationNode
    {
        List<SymbolVar> vars;
        SymbolType type;
        NodeExpression? value = null;
        public VarDeclarationNode(List<SymbolVar> name, SymbolType type, NodeExpression? value)
        {
            this.vars = name;
            this.type = type;
            this.value = value;
            if (value != null)
            {
                if (vars[0].GetOriginalTypeVar().GetType() != value.GetCachedType().GetType())
                {
                    if (type.GetType() == typeof(SymReal) && value.GetCachedType().GetType() == typeof(SymInteger))
                    {
                        this.value = new NodeCast(type, value);
                    }
                    else
                    {
                        throw new Exception($"Incompatible types");
                    }
                }
            }
        }
        public override string ToString(string indent)
        {
            string res;
            res = $":\r\n";
            if (vars.Count > 1)
            {
                res += indent + $"├─── \r\n";
                for (int i = 0; i < vars.Count - 1; i++)
                {
                    res += indent + $"│    ├─── {vars[i]}\r\n";
                }
                res += indent + $"│    └─── {vars[^1]}\r\n";
            }
            else
            {
                res += indent + $"├─── {vars[0]}\r\n";
            }
            if (value != null)
            {
                res += indent + $"├─── {type.ToString(indent + "│    ")}\r\n";
                res += indent + $"└─── =\r\n" +
                       indent + $"     └─── {value.ToString(indent + "     " + "     ")}";
            }
            else
            {
                res += indent + $"└─── {type.ToString(indent + "     ")}";
            }
            return res;
        }
    }
    public class TypeDeclarationNode : DeclarationNode
    {
        string name;
        SymTypeAlias type;
        public TypeDeclarationNode(string name, SymTypeAlias type)
        {
            this.name = name;
            this.type = type;
        }

        public override string ToString(string indent)
        {
            string res;
            res = $"=\r\n";
            res += indent + $"├─── {name}\r\n";
            res += indent + $"└─── {type.GetOriginalType().ToString(indent + "     ")}";
            return res;
        }
    }
    public class ConstDeclarationNode : DeclarationNode
    {
        SymVarConst var;
        NodeExpression value;
        public ConstDeclarationNode(SymVarConst var, NodeExpression value)
        {
            this.var = var;
            this.value = value;
            if ((value.GetCachedType().GetType() != typeof(SymInteger)) &&
                (value.GetCachedType().GetType() != typeof(SymReal)) &&
                (value.GetCachedType().GetType() != typeof(SymString)))
            {
                throw new Exception($"Incompatible types");
            }
        }
        public override string ToString(string indent)
        {
            string res;
            res = $"=\r\n";
            res += indent + $"├─── {var}\r\n";
            res += indent + $"└─── {value.ToString(indent + "│    ")}";
            return res;
        }
    }
}
