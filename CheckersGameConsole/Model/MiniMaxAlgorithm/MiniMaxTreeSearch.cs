using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGameConsole.Model.MiniMaxAlgorithm
{
    public class MiniMaxTreeSearch
    {
        public static readonly int INFINITY = int.MaxValue;
        public MiniMaxTreeNode Root { get; set; }
        public MiniMaxTreeSearch(CheckerGame game) 
        { 
            Root = new MiniMaxTreeNode(game);
            
        }

        public MiniMaxTreeNode MiniMaxSearch(MiniMaxTreeNode miniMaxClass, int depth, int alpha, int beta, bool maximizingNode)
        {
            if (depth == 0 || miniMaxClass.IsOver())
            {
                miniMaxClass.Value = miniMaxClass.GetEvaluation();
                return miniMaxClass;
            }
                
            int eval;
            if (maximizingNode) 
            {
                int maxEval = -INFINITY;
                MiniMaxTreeNode maxChild = miniMaxClass;
                foreach (MiniMaxTreeNode child in miniMaxClass.GetChildern())
                {
                    eval = MiniMaxSearch(child, depth - 1, alpha, beta, child.MaximaizeNode).Value;
                    if(eval > maxEval)
                    {
                        maxEval = eval;
                        maxChild = child;
                        maxChild.Value = maxEval;
                    }
                    
                    alpha = Math.Max(alpha, maxEval);
                    if (beta <= alpha)
                        break;
                }
                return maxChild;
            }
            else
            {
                int minEval = INFINITY;
                MiniMaxTreeNode minChild = miniMaxClass;
                foreach (MiniMaxTreeNode child in miniMaxClass.GetChildern())
                {
                    
                    eval = MiniMaxSearch(child, depth - 1, alpha, beta, child.MaximaizeNode).Value;
                    if(eval < minEval)
                    {
                        minEval = eval;
                        minChild = child;
                        minChild.Value = minEval;
                    }
                    alpha = Math.Min(beta, minEval);
                    if (beta <= alpha)
                        break;
                }
                return minChild;
            }
        }
    }
}
