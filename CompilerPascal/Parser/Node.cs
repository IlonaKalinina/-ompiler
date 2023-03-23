using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public class Node
    {
        public virtual string ToString(string indent)
        {
            return "node";
        }

        public override string ToString()
        {
            return "node";
        }

        public static string Indent(bool last)
        {
            return last ? "│    " : "     ";
        }
    }

    public class NodeMainProgram : Node
    {
        string name;
        List<NodeDescriptions> types;
        BlockStmt body;
        public NodeMainProgram(string name, List<NodeDescriptions> types, BlockStmt body)
        {
            this.name = name;
            this.types = types;
            this.body = body;
        }

        public override string ToString(string indent)
        {
            string result = null;
            indent = "│    ";
            result += $"program {name}\r\n";
            foreach (NodeDescriptions type in types)
            {
                result += $"├───{type.ToString(indent)}\r\n";
            }
            result += indent + "\r\n";
            result += body.ToString(indent);
            return result;
        }
    }
}
