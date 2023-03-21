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
            result += $"program {name}\n\r";
            foreach (NodeDescriptions type in types)
            {
                result += $"├───{type.ToString(indent)}\n\r";
            }
            result += indent + "\n\r";
            result += body.ToString(indent);
            return result;
        }
    }

    public class NodeProgram : Node
    {
        List<NodeDescriptions> types;
        BlockStmt body;
        public NodeProgram(List<NodeDescriptions> types, BlockStmt body)
        {
            this.types = types;
            this.body = body;
        }

        public override string ToString(string indent)
        {
            string result = null;
            indent = Indent(true);
            result += $"program\n\r";
            foreach (NodeDescriptions type in types)
            {
                result += $"├───{type.ToString(indent)}\n\r";
            }
            result += body.ToString(indent);
            return result;
        }
    }
}
