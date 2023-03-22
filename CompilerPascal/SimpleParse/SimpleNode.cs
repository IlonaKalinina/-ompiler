using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public class SimpleNode
    {
        public LexemaType type = LexemaType.NONE;
        public object value = null;
        public List<SimpleNode> children;
    }
}
