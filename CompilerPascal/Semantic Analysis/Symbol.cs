using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public class Symbol : Node
    {
        string name;
        public Symbol(string name)
        {
            this.name = name;
        }
        public override string ToString()
        {
            return name;
        }
    }
    public class SymbolVar : Symbol
    {
        SymbolType type;
        public SymbolType GetTypeVar()
        {
            return type;
        }
        public SymbolType GetOriginalTypeVar()
        {
            SymbolType buildsType = type;
            while (buildsType.GetType().Name == "SymTypeAlias")
            {
                SymTypeAlias symTypeAlias = (SymTypeAlias)buildsType;
                buildsType = symTypeAlias.GetOriginalType();
            }
            return buildsType;
        }
        public SymbolVar(string name, SymbolType type) : base(name)
        {
            this.type = type;
        }
    }
    public class SymVarParamVar : SymbolVar
    {
        public SymVarParamVar(SymbolVar var) : base("var " + var.ToString(), var.GetTypeVar()) { }
    }
    public class SymVarParamOut : SymbolVar
    {
        public SymVarParamOut(SymbolVar var) : base("out " + var.ToString(), var.GetTypeVar()) { }
    }
    public class SymVarParam : SymbolVar
    {
        public int offset = 0;
        public SymVarParam(SymbolVar var) : base(var.ToString(), var.GetTypeVar()) { }
    }

    public class SymVarConst : SymbolVar
    {
        public NodeExpression value;
        public SymVarConst(string name, SymbolType type, NodeExpression value) : base(name, type)
        {
            this.value = value;
        }
    }

}
