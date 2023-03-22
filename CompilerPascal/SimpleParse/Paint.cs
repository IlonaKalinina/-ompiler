using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public partial class SimpleParser
    {
       static List<string> answerList = new List<string>();
       static List<bool> openBranch = new List<bool>();

       static string lineAnswer = null;
       static string indent = null;

       static int numNode = 0;
       static int numChildren = -1;

       public static void RunTree(SimpleNode firstNode)
        {
            indent = null;
            for (int i = 0; i < numNode; i++)
            {
                if (i == numNode - 1)
                {
                    if (numChildren == 0)
                    {
                        indent += "├───";
                        openBranch.Add(true);
                    }
                    else if (numChildren == 1)
                    {
                        indent += "└───";
                        openBranch[i] = false;
                    }
                }
                else
                {
                    if (openBranch[i])
                    {
                        indent += "│   ";
                    }
                    else
                    {
                        indent += "    ";
                    }
                }
            }
            if (firstNode.value.GetType() == typeof(Operator))
            {
                string binOp = Lexer.ConvertEnumOperator((Operator)firstNode.value);
                lineAnswer = indent + binOp;
            }
            else
            {
                lineAnswer = indent + firstNode.value;
            }
            Console.WriteLine(lineAnswer);
            answerList.Add(lineAnswer);

            if (firstNode.children != null)
            {
                numNode++;
                numChildren = 0;

                RunTree(firstNode.children[0]);
                numChildren = -1;

                if (firstNode.children.Count > 1)
                {
                    numChildren = 1;

                    RunTree(firstNode.children[1]);
                    numChildren = -1;
                }
                numNode--;
            }
            return;
        }
    }
}
