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
            string OutSymbolRecord(Dictionary<string, Symbol> dic, int index, int depth)
            {
                string res = "";
                SymRecord symRecord = (SymRecord)dic.ElementAt(index).Value;
                Dictionary<string, Symbol> dicLocals = symRecord.GetFields().GetData();
                if (dicLocals.Count > 0)
                {
                    for (int i = 0; i < depth; i++)
                    {
                        res += "\t";
                    }
                    res += $"locals of record \"{dic.ElementAt(index).Key}\": \r\n";
                    for (int z = 0; z < dicLocals.Count; z++)
                    {
                        for (int i = 0; i < depth; i++)
                        {
                            res += "\t";
                        }
                        res += dicLocals.ElementAt(z).Key.ToString() + ": " + dicLocals.ElementAt(z).Value.GetType().Name + "\r\n";
                        if (dicLocals.ElementAt(z).Value.GetType() == typeof(SymRecord))
                        {
                            res += OutSymbolRecord(dicLocals, z, depth + 1);
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
                Dictionary<string, Symbol> dic = symTableStack.GetTable(i).GetData();
                for (int j = 0; j < dic.Count; j++)
                {
                    res += "\t" + dic.ElementAt(j).Key.ToString() + ": " + dic.ElementAt(j).Value.GetType().Name + "\r\n";
                    if (dic.ElementAt(j).Value.GetType() == typeof(SymRecord))
                    {
                        res += OutSymbolRecord(dic, j, 2);
                    }
                }
            }
            return res;
        }
    }
}
