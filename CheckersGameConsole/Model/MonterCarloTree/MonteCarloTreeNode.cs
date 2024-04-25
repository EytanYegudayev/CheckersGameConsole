using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CheckersGameConsole.Model
{
    public class MonteCarloTreeNode
    {
        public static readonly double EXPLORATION_PARAMENTER = Math.Sqrt(2);
        public CheckerGame State { get; set; }

        public Move Move;
        public int Visits { get; set; }
        public double Score { get; set; }
        public MonteCarloTreeNode Parent { get; set; }
        public List<MonteCarloTreeNode> Children { get; set; }

        public MonteCarloTreeNode(CheckerGame state, MonteCarloTreeNode parent = null, Move move = null)
        {
            State = state;
            Visits = 0;
            Score = 0;
            Children = new List<MonteCarloTreeNode>();
            Parent = parent;
            Move = move;
        }

        public double GetValue()
        {
            if (Visits == 0)
            {
                return -1;
            }
            return (Score / Visits) +
            Math.Sqrt(Math.Log(Visits) / Visits * EXPLORATION_PARAMENTER);
        }



    }

}
