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
        Colour colour;
        private GameBoard board; // §readonly, rename to something fancy
        public MiniMaxAI(GameBoard board)
        {
            this.board = board;
            colour = board.Turn;
        }

        public string BestMove()
        {
            return BestMove(0);
        }

        public string BestMove(int depth)
        {
            return MinMax(depth);
        }

        private string MinMax(int depth)
        {
            string bestMove = null;
            List<string> moves = new List<string>();
            Queue<Node> nodes = new Queue<Node>();

            // Find all valid moves
            moves = board.ValidMoves(board.Turn);

            // For each valid move
            // Create a node
            foreach (string move in moves)
            {
                nodes.Enqueue(new Node(board.Clone(), move));
            }

            // Recursively find the best move by using the minimax algorithm (iterative deepening) on the nodes
            Node node;
            int score;
            int max = int.MinValue;
            while (nodes.Count > 0)
            {
                node = nodes.Dequeue();
                score = Min(node, depth - 1);
                if (score > max)
                {
                    max = score;
                    bestMove = node.move;
                }
            }
            return bestMove;
        }

        private int Min(Node node, int depth)
        {
            if (depth <= 0)
                return node.score;

            Queue<Node> nodes = new Queue<Node>();

            // For each valid move
            // Create a node
            NodeInsertion(node, nodes);

            // Recursively find the best move by using the minimax algorithm (iterative deepening) on the nodes
            Node newNode;
            int score;
            int min = int.MaxValue;
            while (nodes.Count > 0)
            {
                newNode = nodes.Dequeue();
                score = Max(newNode, depth - 1);
                if (score < min)
                    min = score;
            }
            return min;
        }

        private int Max(Node node, int depth)
        {
            if (depth <= 0)
                return node.score;

            Queue<Node> nodes = new Queue<Node>();

            // For each valid move
            // Create a node
            NodeInsertion(node, nodes);

            // Recursively find the best move by using the minimax algorithm (iterative deepening) on the nodes
            Node newNode;
            int score;
            int max = int.MinValue;
            while (nodes.Count > 0)
            {
                newNode = nodes.Dequeue();
                score = Min(newNode, depth - 1);
                if (score > max)
                    max = score;
            }
            return max;
        }

        private static void NodeInsertion(Node node, Queue<Node> nodes)
        {
            foreach (string move in node.moves)
            {
                GameBoard newBoard = node.board.Clone();
                nodes.Enqueue(new Node(newBoard, move));
            }
        }

        class Node
        {
            internal GameBoard board;
            internal int score;
            internal string move;
            internal List<string> moves;
            internal Colour turn;

            internal Node(GameBoard nBoard, string nMove)
            {
                board = nBoard;
                move = nMove;
                new Interpreter(board).Input(move);
                moves = board.ValidMoves(board.Turn);
                turn = (board.Turn == Colour.Black) ? Colour.White : Colour.Black;
                Evaluate();
            }

            private void Evaluate()
            {
                score = 0;
                score += Evaluator.EvaluateStack(board, turn);
                score += Evaluator.EvaluateTopPiece(board, turn);
                GameState gs = board.GameState;
                if (gs != GameState.InProgress && gs != GameState.Tie)
                {
                    if (((gs == GameState.BF || gs == GameState.BR) && turn == Colour.Black) ||
                        ((gs == GameState.WF || gs == GameState.WR) && turn == Colour.White))
                        score = int.MaxValue;
                    else
                        score = int.MinValue;
                }
            }
        }
    }
}
