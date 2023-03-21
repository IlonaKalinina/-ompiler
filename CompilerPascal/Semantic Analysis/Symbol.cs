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
    public enum VarType
    {
        Param,
        Const,
        Global,
        Local
    }
    public class SymVarParam : SymbolVar
    {
        public int offset = 0;
        public SymVarParam(SymbolVar var) : base(var.ToString(), var.GetTypeVar()) { }
    }

    public class SymVarGlobal : SymbolVar
    {
        public SymVarGlobal(SymbolVar var) : base(var.ToString(), var.GetTypeVar()) { }
    }

    public class SymVarConst : SymbolVar
    {
        public NodeExpression value;
        public SymVarConst(string name, SymbolType type, NodeExpression value) : base(name, type)
        {
            this.value = value;
        }
    }
    public class SymVarLocal : SymbolVar
    {
        public int offset = 0;
        public SymVarLocal(SymbolVar var) : base(var.ToString(), var.GetTypeVar()) { }
    }
    public class SymProc : Symbol
    {
        bool unlimitedParameters = false;
        List<SymbolVar> args;
        SymTable params_;
        SymTable locals;
        BlockStmt body;

        public List<SymbolVar> GetParams()
        {
            return args;
        }
        public SymTable GetLocals()
        {
            return locals;
        }

        public int GetCountParams()
        {
            if (unlimitedParameters)
            {
                return -1;
            }
            else
            {
                return params_.GetSize();
            }
        }
        public BlockStmt GetBody()
        {
            return body;
        }
        public SymProc(string name, SymTable params_, SymTable locals, BlockStmt body) : base(name)
        {
            this.params_ = params_;
            this.locals = locals;
            this.body = body;
        }
        public SymProc(string name) : base(name)
        {
            unlimitedParameters = true;
            this.params_ = new SymTable(new Dictionary<string, Symbol>());
            this.locals = new SymTable(new Dictionary<string, Symbol>());
            this.body = new BlockStmt(new List<NodeStatement>());
        }
    }
}
