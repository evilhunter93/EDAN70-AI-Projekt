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
        private GameBoard board; // §readonly, rename to something fancy
        private int depth;

        public MiniMaxAI(GameBoard board, int depth = 0)
        {
            this.board = board;
            this.depth = depth;
        }

        public string BestMove()
        {
            return MinMax(depth);
        }

        private string MinMax(int depth, int alpha = int.MinValue, int beta = int.MaxValue)
        {
            string bestMove = null;
            IEnumerable<string> moves = board.ValidMoves(board.Turn);

            // Recursively find the best move by using the minimax algorithm (iterative deepening) on the nodes
            Node node;
            int score;
            foreach (string move in moves)
            {
                node = new Node(board.Clone(), move);
                score = Min(node, depth - 1, alpha, beta);
                if (score > alpha)
                {
                    alpha = score;
                    bestMove = node.move;
                }
                if (score >= beta)
                    return bestMove;
            }
            return bestMove;
        }

        private int Min(Node node, int depth, int alpha, int beta)
        {
            if (depth <= 0)
                return node.score;

            // Recursively find the best move by using the minimax algorithm (iterative deepening) on the nodes
            Node newNode;
            int score = int.MaxValue;
            foreach (string move in node.moves)
            {
                newNode = new Node(node.board.Clone(), move);
                score = Math.Min(score, Max(newNode, depth - 1, alpha, beta));
                if (score <= alpha)
                    return score;
                beta = Math.Min(beta, score);
            }
            return score;
        }

        private int Max(Node node, int depth, int alpha, int beta)
        {
            if (depth <= 0)
                return node.score;

            // Recursively find the best move by using the minimax algorithm (iterative deepening) on the nodes
            Node newNode;
            int score = int.MinValue;
            foreach (string move in node.moves)
            {
                newNode = new Node(node.board.Clone(), move);
                score = Math.Max(score, Min(newNode, depth - 1, alpha, beta));
                if (score >= beta)
                    return score;
                alpha = Math.Max(alpha, score);
            }
            return score;
        }

        class Node
        {
            internal GameBoard board;
            internal int score;
            internal string move;
            internal IEnumerable<string> moves;
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
