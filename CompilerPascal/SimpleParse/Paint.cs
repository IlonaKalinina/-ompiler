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
       static string lineBuf = null;

       static int numNode = 0;
       static int numChildren = -1;

        /*  
         static void RunTree(Node doParse)
         {
             lineBuf = null;
             for (int i = 0; i < numNode; i++)
             {
                 if (i == numNode - 1)
                 {
                     if (numChildren == 0)
                     {
                         lineBuf += "├───";
                         openBranch.Add(true);
                     }
                     else if (numChildren == 1)
                     {
                         lineBuf += "└───";
                         openBranch[i] = false;
                     }
                 }
                 else
                 {
                    if (openBranch[i])
                    {
                        lineBuf += "│   ";
                    }
                    else
                    {
                         lineBuf += "    ";
                    }
                 }
             }
             lineAnswer = lineBuf + doParse.value;
             Console.WriteLine(lineAnswer);
             answerList.Add(lineAnswer);

             if (doParse.children != null)
             {
                 numNode++;
                 numChildren = 0;

                 RunTree(doParse.children[0]);
                 numChildren = -1;

                 if (doParse.children.Count > 1)
                 {
                     numChildren = 1;

                     RunTree(doParse.children[1]);
                     numChildren = -1;
                 }
                 numNode--;
             }
             return;
         }*/
    }
}
