using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGameConsole.Model.MiniMaxAlgorithm
{
    public interface MiniMaxClass
    {
        public List<MiniMaxTreeNode> GetChildren(MiniMaxTreeNode parent);
        public int GetEvaluation();

        public bool IsOver();

        public bool IsMaximaize();
    }
}
