using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompilerPascal
{
    public class NodeStatement : Node { }
    public class NullStmt : NodeStatement
    {
        public NullStmt() { }

        public override string ToString(string str_1)
        {
            return "";
        }
    }
    public class AssignmentStmt : NodeStatement
    {
        Operator opname;
        NodeExpression left;
        NodeExpression right;
        public AssignmentStmt(Operator opname, NodeExpression left, NodeExpression right)
        {
            this.opname = opname;
            this.left = left;
            this.right = right;
        }

        public override string ToString(string indent)
        {
            string result = null;
            string binOpName = opname.ToString().ToLower();

            if (opname.GetType() == typeof(Operator))
            {
                binOpName = Lexer.ConvertEnumOperator(opname);
            }
            result += binOpName + "\r\n";
            result += indent + $"├─── {left.ToString(indent + "│     ")}\r\n";
            result += indent + $"└─── {right.ToString(indent + "     ")}";
            return result;
        }
    }
    public class CallStmt : NodeStatement
    {
        SymProc proc;
        List<NodeExpression> args;
        public CallStmt(Symbol proc, List<NodeExpression> arg)
        {
            this.proc = (SymProc)proc;
            this.args = arg;
        }
        public CallStmt(SymProc proc, List<NodeExpression> arg)
        {
            this.proc = proc;
            this.args = arg;
        }
        public override string ToString(string indent)
        {
            string result = null;
            result += $"{proc}\r\n";
            foreach (NodeExpression arg in args)
            {
                if (arg != args.Last())
                {
                    result += indent + $"└─── {arg.ToString(indent + "│    ")}\r\n";
                }
                else
                {
                    result += indent + $"└─── {arg.ToString(indent + "     ")}";
                }
            }
            return result;
        }
    }
    public class IfStmt : NodeStatement
    {
        NodeExpression condition;
        NodeStatement body;
        NodeStatement elseBody;
        public IfStmt(NodeExpression condition, NodeStatement body, NodeStatement elseBody)
        {
            this.condition = condition;
            this.body = body;
            this.elseBody = elseBody;
        }

        public override string ToString(string indent)
        {
            string result = null;
            result += $"if\r\n";
            result += indent + $"├─── {condition.ToString(indent + "│    ")}\r\n";

            if (elseBody.GetType() == typeof(NullStmt))
            {
                result += indent + $"└─── {body.ToString(indent + "     ")}";
            }
            else
            {
                result += indent + $"├─── {body.ToString(indent + "│    ")}\r\n";
                result += indent + $"else\r\n";
                result += indent + $"└─── {elseBody.ToString(indent + "     ")}";
            }
            return result;
        }
    }
    public class WhileStmt : NodeStatement
    {
        NodeExpression condition;
        NodeStatement body;
        public WhileStmt(NodeExpression condition, NodeStatement body)
        {
            this.condition = condition;
            this.body = body;
        }

        public override string ToString(string indent)
        {
            string result = null;
            result += $"while\r\n";
            result += indent + $"├─── {condition.ToString(indent + "│    ")}\r\n";
            result += indent + $"└─── {body.ToString(indent + "     ")}";
            return result;
        }
    }
    public class ForStmt : NodeStatement
    {
        NodeVar controlVar;
        NodeExpression initialVal;
        KeyWord toOrDownto;
        NodeExpression finalVal;
        NodeStatement body;
        public ForStmt(NodeVar controlVar, NodeExpression initialVal, KeyWord toOrDownto, NodeExpression finalVal, NodeStatement body)
        {
            this.controlVar = controlVar;
            this.initialVal = initialVal;
            this.toOrDownto = toOrDownto;
            this.finalVal = finalVal;
            this.body = body;
        }

        public override string ToString(string indent)
        {
            string result = null;
            
            result += $"for\r\n";
            result += indent + $"├─── :=\r\n";
            result += indent + $"│    ├─── {controlVar.ToString(indent)}\r\n";
            result += indent + $"│    └─── {initialVal.ToString(indent + "     ")}\r\n";
            result += indent + $"├─── {toOrDownto.ToString().ToLower()}\r\n";
            result += indent + $"│    └─── {finalVal.ToString(indent + "     ")}\r\n";
            result += indent + $"└─── {body.ToString(indent + "     ")}";
            return result;
        }
    }
    public class RepeatStmt : NodeStatement
    {
        NodeExpression condition;
        List<NodeStatement> body;
        public RepeatStmt(List<NodeStatement> body, NodeExpression condition)
        {
            this.condition = condition;
            this.body = body;
        }

        public override string ToString(string indent)
        {
            string result = null;
            result += $"repeat\r\n";
            foreach (NodeStatement stmt in body)
            {
                result += indent + $"├─── {stmt.ToString(indent + "│    ")}\r\n";
            }
            result += indent + $"└─── {condition.ToString(indent + "     ")}";
            return result;
        }
    }
    public class BlockStmt : NodeStatement
    {
        public List<NodeStatement> body;
        public BlockStmt(List<NodeStatement> body)
        {
            this.body = body;
        }

        public override string ToString(string indent)
        {
            string result = null;
            result += $"begin\r\n";
            foreach (NodeStatement stmt in body)
            {
                result += $"├─── {stmt.ToString(indent)}\r\n";
            }
            result += $"└─── end";
            return result;
        }
    }
}
