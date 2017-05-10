using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Game;

namespace Tak.AI
{
    class MiniMaxAI : IAI
    {
        Colour colour;
        private GameBoard board; // §readonly, rename to something fancy
        private int depth;

        public MiniMaxAI(GameBoard board, int depth = 0)
        {
            this.board = board;
            this.depth = depth;
            colour = board.Turn;
        }

        public string BestMove()
        {
            return MinMax(depth);
        }

        private string MinMax(int depth, int alpha = int.MinValue, int beta = int.MaxValue)
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
            while (nodes.Count > 0)
            {
                node = nodes.Dequeue();
                score = Min(node, depth - 1, alpha, beta);
                if (score > alpha)
                {
                    alpha = score;
                    bestMove = node.move;
                }
                if (beta <= alpha)
                    break; // beta cut-off
            }
            return bestMove;
        }

        private int Min(Node node, int depth, int alpha, int beta)
        {
            if (depth <= 0)
                return node.score;

            Queue<Node> nodes = new Queue<Node>();

            // For each valid move
            // Create a node
            NodeInsertion(node, nodes);

            // Recursively find the best move by using the minimax algorithm (iterative deepening) on the nodes
            Node newNode;
            int score = int.MaxValue;
            while (nodes.Count > 0)
            {
                newNode = nodes.Dequeue();
                score = Math.Min(score, Max(newNode, depth - 1, alpha, beta));
                beta = Math.Min(beta, score);
                if (beta <= alpha)
                    break; // alpha cut-off
            }
            return score;
        }

        private int Max(Node node, int depth, int alpha, int beta)
        {
            if (depth <= 0)
                return node.score;

            Queue<Node> nodes = new Queue<Node>();

            // For each valid move
            // Create a node
            NodeInsertion(node, nodes);

            // Recursively find the best move by using the minimax algorithm (iterative deepening) on the nodes
            Node newNode;
            int score = int.MinValue;
            while (nodes.Count > 0)
            {
                newNode = nodes.Dequeue();
                score = Math.Max(score, Min(newNode, depth - 1, alpha, beta));
                alpha = Math.Max(alpha, score);
                if (beta <= alpha)
                    break; // beta cut-off
            }
            return score;
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
                Interpreter.Input(move, board);
                turn = board.Turn;
                board.EndTurn();
                moves = board.ValidMoves(board.Turn);
                Evaluate();
            }

            private void Evaluate()
            {
                score = Evaluator.EvaluateGameState(board, turn);
                if (score == 0)
                {
                    score += Evaluator.EvaluateStack(board, turn);
                    score += Evaluator.EvaluateTopPiece(board, turn);
                }
            }
        }
    }
}
