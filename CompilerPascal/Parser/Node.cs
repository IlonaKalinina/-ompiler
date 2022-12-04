using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal.Parser
{
    public class Node
    {
        public string category = "";
        public string value = "";
        public List<Node> children;
    }
}
