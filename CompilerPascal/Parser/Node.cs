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
}
