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
        private GameBoard board;
        public MiniMaxAI(GameBoard board)
        {
            this.board = board;
        }
        public string BestMove()
        {
            String move = MinMax();
            throw new NotImplementedException();
        }

        private String MinMax()
        {
            String bestMove = null;
            String[] moves;
            System.Collections.ArrayList nodes;

            // Find all valid moves
            moves = board.validMoves();
            nodes = new System.Collections.ArrayList();

            // For each valid move
            // Create a node
            foreach (String move in moves)
            {
                nodes.Add(new Node(board, move));
            }

            // Recursively find the best move by using the minimax algorithm (iterative deepening) on the nodes
            Node node;
            int temp;
            int min = int.MaxValue;
            while (nodes.Count > 0)
            {
                node = (Node)nodes[0];
                nodes.Remove(node);
                temp = Min(node, 0);
                if (temp < min)
                {
                    bestMove = node.move;
                }
            }
            return bestMove;
        }

        private int Min(Node node, int score)
        {
            String[] moves;
            System.Collections.ArrayList nodes;

            // Find all valid moves
            moves = board.validMoves();
            nodes = new System.Collections.ArrayList();

            // For each valid move
            // Create a node
            foreach (String move in moves)
            {
                nodes.Add(new Node(board, move));
            }

            // Recursively find the best move by using the minimax algorithm (iterative deepening) on the nodes
            Node newNode;
            int temp;
            int min = int.MaxValue;
            while (nodes.Count > 0)
            {
                newNode = (Node)nodes[0];
                nodes.Remove(newNode);
                temp = score + Max(newNode, score);
                if (temp < min)
                    min = temp;
            }
            return min;
        }

        private int Max(Node node, int score)
        {
            String[] moves;
            System.Collections.ArrayList nodes;

            // Find all valid moves
            moves = board.validMoves();
            nodes = new System.Collections.ArrayList();

            // For each valid move
            // Create a node
            foreach (String move in moves)
            {
                nodes.Add(new Node(board, move));
            }

            // Recursively find the best move by using the minimax algorithm (iterative deepening) on the nodes
            Node newNode;
            int temp;
            int max = int.MinValue;
            while (nodes.Count > 0)
            {
                newNode = (Node)nodes[0];
                nodes.Remove(newNode);
                temp = score + Min(newNode, score);
                if (temp > max)
                    max = temp;
            }
            return max;
        }

        private class Node
        {
            GameBoard board;
            int score;
            bool end;
            string move;

            internal Node(GameBoard board, string move)
            {
                this.board = board;
                this.move = move;
            }


        }
    }
}
