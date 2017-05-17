using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Game;

namespace Tak.AI
{
    class MonteCarloAI : IAI
    {
        // Implementation found at: https://jeffbradberry.com/posts/2015/09/intro-to-monte-carlo-tree-search/

        private GameBoard board; // §readonly, rename to something fancy
        private int depth;
        private Colour player;
        private Dictionary<Node, int> plays, wins;
        private TimeSpan timeLimit;

        public MonteCarloAI(GameBoard board, Colour player, int timeLimitSeconds = 1)
        {
            this.board = board;
            timeLimit = new TimeSpan(0, 0, timeLimitSeconds);
            this.player = player;

            // TODO: load from file instead
            plays = new Dictionary<Node, int>();
            wins = new Dictionary<Node, int>();
        }

        public string BestMove()
        {
            string move = "";

            depth = 0;
            //        state = self.states[-1]
            IEnumerable<string> legal = board.ValidMoves(player);

            if (legal.Count() == 0)
                return null;
            if (legal.Count() == 1)
                return legal.First();


            int games = 0;
            DateTime begin = DateTime.UtcNow;
            while (DateTime.UtcNow - begin < timeLimit)
            {
                Simulate();
                games++;
            }

            Dictionary<string, Node> moveNodes = new Dictionary<string, Node>();
            foreach (string m in legal)
                moveNodes.Add(move, new Node(board.Clone(), m, player));

            //        # Display the number of calls of `run_simulation` and the
            //        # time elapsed.
            //        print games, datetime.datetime.utcnow() - begin

            double percentWins = 0;
            foreach (KeyValuePair<string, Node> pair in moveNodes)
            {
                if (!wins.ContainsKey(pair.Value))
                    continue; // Default move value?

                double currentPercentWins = wins[pair.Value] / plays[pair.Value];
                if (currentPercentWins > percentWins)
                {
                    percentWins = currentPercentWins;
                    move = pair.Key;
                }
            }

            //         # Display the stats for each possible play.
            //                    for x in sorted(
            //                        ((100 * self.wins.get((player, S), 0) /
            //                          self.plays.get((player, S), 1),
            //                          self.wins.get((player, S), 0),
            //                          self.plays.get((player, S), 0), p)
            //                         for p, S in moves_states),
            //            reverse = True
            //        ):
            //            print "{3}: {0:.2f}% ({1} / {2})".format(*x)

            //        print "Maximum depth searched:", self.max_depth

            return move;
        }

        private void Simulate()
        {
            throw new NotImplementedException();
        }

        private string MinMax(int depth, int alpha = int.MinValue, int beta = int.MaxValue)
        {

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
            }
        }
    }
}
