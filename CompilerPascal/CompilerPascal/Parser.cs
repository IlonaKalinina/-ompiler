using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CompilerPascal
{
    public class Node
    {
    }
    public class BinOp
    {
        public Node left;
        public Node right;
        public string op;
    }
    public class Number
    {
        public string value;
    }
    public class Variable
    {
        public string name;
    }

    internal class Parser
    {
        public void ReadFileLexer(string path)
        {
            string input_data;

            string fileName = path;
            using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
            {
                while ((input_data = sr.ReadLine()) != null)
                {
                }
            }
        }
        public static void ParseExpression()
        {
           
        }
        public static void ParseTerm()
        {

        }
        public static void ParseFactor()
        {

        }
    }
}