using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGameConsole.Model.MiniMaxAlgorithm
{
    public class MiniMaxTreeNode
    {


        public MiniMaxTreeNode? Parent { get; set; }
        public List<MiniMaxTreeNode> Children { get; set; }

        public MiniMaxClass MiniMaxClass { get; set; }

        public bool MaximaizeNode { get; set; }

        public int Value { get; set; }
        
        public MiniMaxTreeNode(MiniMaxClass miniMaxClass, MiniMaxTreeNode? parent = null, bool maximazieNode = true) 
        {
            MiniMaxClass = miniMaxClass;
            Parent = parent;
            MaximaizeNode = MiniMaxClass.IsMaximaize();
            Value = SetInitialValue();
            Children = new List<MiniMaxTreeNode>();
        }
        public List<MiniMaxTreeNode> GetChildern()
        {
            Children = MiniMaxClass.GetChildren(this);
            return Children;
        }
        private int SetInitialValue()
        {
            if (MaximaizeNode)
                return -MiniMaxTreeSearch.INFINITY;
            return MiniMaxTreeSearch.INFINITY;
        }

        public int GetEvaluation()
        {
            return MiniMaxClass.GetEvaluation();
        }

        public bool IsOver()
        {
            return MiniMaxClass.IsOver();
        }
    }
}
