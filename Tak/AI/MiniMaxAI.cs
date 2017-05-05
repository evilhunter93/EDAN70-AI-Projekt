using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Game;

namespace Tak.AI
{
    class MiniMaxAI : AI
    {
        public string BestMove()
        {
            throw new NotImplementedException();
        }

        public void MiniMax()
        {
            // Find all valid moves

            // For each valid move
            // Create a node

            // Recursively find the best move by using the minimax algorithm (iterative deepening) on the nodes
        }

        private class Node
        {
            GameBoard board;
            int score;
            bool end;
            string move;

            Node(GameBoard board, string move)
            {
                this.board = board;
                this.move = move;
            }
        }
    }
}
