using System;
using System.Collections;
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
            string move = MinMax();
            throw new NotImplementedException();
        }

        private string MinMax()
        {
            string bestMove = null;
            ArrayList moves = new ArrayList();
            ArrayList nodes;

            // Find all valid moves
            moves = board.ValidMoves(board.Turn);
            nodes = new ArrayList();

            // For each valid move
            // Create a node
            foreach (string move in moves)
            {
                nodes.Add(new Node(board.Clone(), move));
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
            ArrayList moves;
            ArrayList nodes;
            GameBoard nBoard = node.board;

            // Find all valid moves
            moves = nBoard.ValidMoves(nBoard.Turn);
            nodes = new ArrayList();

            // For each valid move
            // Create a node
            foreach (string move in moves)
            {
                nodes.Add(new Node(nBoard.Clone(), move));
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
            ArrayList moves = new ArrayList();
            ArrayList nodes;
            GameBoard nBoard = node.board;

            // Find all valid moves
            moves = nBoard.ValidMoves(nBoard.Turn);
            nodes = new ArrayList();

            // For each valid move
            // Create a node
            foreach (string move in moves)
            {
                nodes.Add(new Node(nBoard, move));
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

        class Node
        {
            internal GameBoard board;
            internal int score;
            internal bool end;
            internal string move;

            internal Node(GameBoard board, string move)
            {
                this.board = board;
                this.move = move;
            }


        }
    }
}
