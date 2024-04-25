using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGameConsole.Model.MonterCarloTree
{
    public class MonteCarloTreeSearch
    {

        private readonly Random _random;
        private CheckerTool Player;

        public MonteCarloTreeSearch(CheckerTool player)
        {
            _random = new Random();
            Player = player;
        }


        public MonteCarloTreeNode FindBestMove(CheckerGame initialState, int simulationsCount)
        {
            var rootNode = new MonteCarloTreeNode(initialState, null);
            
            for (int i = 0; i < simulationsCount; i++)
            {
                MonteCarloTreeNode node = SelectNode(rootNode);
                MonteCarloTreeNode newState = ExpandNode(node);
                double score = Simulate(newState);
                node.Children.Add(newState);
                Backpropagate(node, score);
            }

            return GetBestChild(rootNode);
        }

        private MonteCarloTreeNode SelectNode(MonteCarloTreeNode rootNode)
        {
            
            var node = rootNode;
            while (node.Children.Any())
            {
                
                node = node.Children.OrderByDescending(child =>
                    child.GetValue())
                    .First();
            }

            return node;
        }

        private MonteCarloTreeNode ExpandNode(MonteCarloTreeNode node)
        {
            // clone the game board and node is the parent node
            MonteCarloTreeNode newState = new MonteCarloTreeNode(new CheckerGame(node.State), node);

            // Perform a random move to generate a new state
            var legalMoves = newState.State.GetAllPossibleMoveByPlayer(newState.State.CurrentPlayer);
            if (legalMoves.Count > 0)
            {
                Move randomMove = legalMoves[_random.Next(legalMoves.Count)];
                newState.Move = randomMove;
                newState.State.MovePiece(randomMove.FromRow, randomMove.FromColumn, randomMove.ToRow, randomMove.ToColumn);
            }

            return newState;
        }

        private double Simulate(MonteCarloTreeNode node)
        {
            if (node.State.IsGameOver() && node.State.CurrentPlayer == Player)
            {

                Console.WriteLine("AI Win!");
                return 1;
            }
            if (node.State.GetAllPossibleMoveByPlayer(node.State.CurrentPlayer).Count == 0)
                return 0.5;
            return 0; 
        }


        private void Backpropagate(MonteCarloTreeNode node, double score)
        {
            while (node != null)
            {
                node.Visits++;
                node.Score += score;
                node = node.Parent;
            }
        }

        private MonteCarloTreeNode GetBestChild(MonteCarloTreeNode node)
        {
            return node.Children.OrderByDescending(child => child.Score).First();
        }

    }
}
