using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Parser
{
    public class Node
    {
        public string category = "";
        public object value = null;
        public List<Node> children;
    }

    public enum NodeType
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
        NONE
    }
}
