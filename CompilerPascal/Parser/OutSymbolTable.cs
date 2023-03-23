using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompilerPascal
{
    public partial class Parser
    {
        public string OutSymbolTable()
        {
            string OutSymbolRecord(Dictionary<string, Symbol> types, int index, int depth)
            {
                string res = "";
                SymRecord symRecord = (SymRecord)types.ElementAt(index).Value;
                Dictionary<string, Symbol> dicLocals = symRecord.GetFields().GetData();
                if (dicLocals.Count > 0)
                {
                    for (int i = 0; i < depth; i++)
                    {
                        res += "\t";
                    }
                    res += $"record \"{types.ElementAt(index).Key}\": \r\n";
                    for (int k = 0; k < dicLocals.Count; k++)
                    {
                        for (int i = 0; i < depth; i++)
                        {
                            res += "\t";
                        }
                        res += dicLocals.ElementAt(k).Key.ToString() + ": " + dicLocals.ElementAt(k).Value.GetType().Name + "\r\n";
                        if (dicLocals.ElementAt(k).Value.GetType() == typeof(SymRecord))
                        {
                            res += OutSymbolRecord(dicLocals, k, depth + 1);
                        }
                    }
                }
                return res;
            }
            string res = "\r\nSymbol Tables:\r\n";
            for (int i = 0; i < symTableStack.GetCountTables(); i++)
            {
                switch (i)
                {
                    case 0:
                        res += $"builtins:\r\n";
                        break;
                    case 1:
                        res += $"globals:\r\n";
                        break;
                    default:
                        res += $"table #{i}\r\n";
                        break;
                }
                Dictionary<string, Symbol> types = symTableStack.GetTable(i).GetData();
                for (int j = 0; j < types.Count; j++)
                {
                    res += "\t" + types.ElementAt(j).Key.ToString() + ": " + types.ElementAt(j).Value.GetType().Name + "\r\n";
                    if (types.ElementAt(j).Value.GetType() == typeof(SymRecord))
                    {
                        res += OutSymbolRecord(types, j, 2);
                    }
                }
            }
            return res;
        }
    }
}
