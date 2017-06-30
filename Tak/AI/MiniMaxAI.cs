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
        private Colour player;

        public MiniMaxAI(GameBoard board, Colour player, int depth = 0)
        {
            this.board = board;
            this.depth = depth;
            this.player = player;
        }

        public string BestMove()
        {
            return MinMax(depth);
        }

        private string MinMax(int depth, int alpha = int.MinValue, int beta = int.MaxValue)
        {
            string bestMove = "hej";
            GameBoard cBoard = board.Clone();
            IEnumerable<string> moves = cBoard.ValidMoves(cBoard.Turn);
            Console.WriteLine("nbr of moves: " + moves.Count());
            // Recursively find the best move by using the minimax algorithm on the nodes
            Node node;
            int score;
            foreach (string move in moves)
            {
                node = new Node(cBoard.Clone(), move, player);
                if (!node.end)
                    score = Min(node, depth - 1, alpha, beta);
                else
                    score = node.score;

                if (score > alpha || alpha == int.MinValue)
                {
                    alpha = score;
                    bestMove = node.move;
                }
            }
            return bestMove;
        }

        private int Min(Node node, int depth, int alpha, int beta)
        {
            if (depth <= 0)
                return node.score;

            // Recursively find the best move by using the minimax algorithm on the nodes
            Node newNode;
            int score = int.MaxValue;
            foreach (string move in node.moves)
            {
                newNode = new Node(node.board.Clone(), move, player);
                if (!node.end)
                    score = Math.Min(score, Max(newNode, depth - 1, alpha, beta));
                else
                    score = node.score;

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

            // Recursively find the best move by using the minimax algorithm on the nodes
            Node newNode;
            int score = int.MinValue;
            foreach (string move in node.moves)
            {
                newNode = new Node(node.board.Clone(), move, player);
                if (!node.end)
                    score = Math.Max(score, Min(newNode, depth - 1, alpha, beta));
                else
                    score = node.score;

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
            internal Colour player;
            internal bool end;

            internal Node(GameBoard nBoard, string nMove, Colour player)
            {
                board = nBoard;
                score = 0;
                move = nMove;
                end = false;
                Interpreter.Input(move, board);
                this.player = player;
                board.EndTurn();
                moves = board.ValidMoves(board.Turn);
                Evaluate();
            }

            private void Evaluate()
            {
                end = board.GameState != GameState.InProgress;
                if (!end)
                {
                    score += Evaluator.StackScore(board, player);
                    score += Evaluator.TopPieceScore(board, player);
                    score += Evaluator.RoadScore(board, player);
                    score += Evaluator.ConnectedComponentScore(board, player);
                    score += Evaluator.ProximityScore(board, player);
                }
                else
                    score = Evaluator.GameStateScore(board.GameState, player);
            }
        }
    }
}
