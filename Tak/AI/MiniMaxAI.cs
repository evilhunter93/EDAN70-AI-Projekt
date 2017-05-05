﻿using System;
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
        private GameBoard board;
        public MiniMaxAI(GameBoard board)
        {
            this.board = board;
            colour = board.Turn;
        }
        public string BestMove()
        {
            string move = MinMax();
            throw new NotImplementedException();
        }

        private string MinMax()
        {
            string bestMove = null;
            List<String> moves = new List<String>();
            List<Node> nodes;

            // Find all valid moves
            moves = board.ValidMoves(board.Turn);
            nodes = new List<Node>();

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
                temp = Min(node);
                if (temp < min)
                {
                    bestMove = node.move;
                }
            }
            return bestMove;
        }

        private int Min(Node node)
        {
            List<String> moves;
            List<Node> nodes;
            GameBoard nBoard = node.board;
            int nScore = EvaluationFunction(node);

            // Find all valid moves
            moves = nBoard.ValidMoves(nBoard.Turn);
            nodes = new List<Node>();

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
                temp = nScore + Max(newNode);
                if (temp < min)
                    min = temp;
            }
            return min;
        }

        private int Max(Node node)
        {
            List<String> moves = new List<String>();
            List<Node> nodes;
            GameBoard nBoard = node.board;
            int nScore = EvaluationFunction(node);

            // Find all valid moves
            moves = nBoard.ValidMoves(nBoard.Turn);
            nodes = new List<Node>();

            // For each valid move
            // Create a node
            foreach (string move in moves)
            {
                nodes.Add(new Node(nBoard.Clone(), move));
            }

            // Recursively find the best move by using the minimax algorithm (iterative deepening) on the nodes
            Node newNode;
            int temp;
            int max = int.MinValue;
            while (nodes.Count > 0)
            {
                newNode = (Node)nodes[0];
                nodes.Remove(newNode);
                temp = nScore + Min(newNode);
                if (temp > max)
                    max = temp;
            }
            return max;
        }

        private int EvaluationFunction(Node node)
        {
            int score = node.score;
            score += EvaluateStack(node.board);
            score += EvaluateTopPiece(node.board);
            return score;
        }

        private int EvaluateStack(GameBoard board)
        {
            int score = 0;
            StoneStack[,] stacks = board.Stacks;
            foreach (StoneStack stack in stacks)
            {
                if (stack.Count > 0)
                {
                    if (stack.Owner == colour)
                    {
                        score += stack.Count;
                    }
                    else
                    {
                        score -= stack.Count;
                    }
                }
            }
            return score;
        }

        private int EvaluateTopPiece(GameBoard board)
        {
            int score = 0;
            StoneStack[,] stacks = board.Stacks;
            foreach (StoneStack stack in stacks)
            {
                if (stack.Count > 0)
                {
                    if (stack.Owner == colour)
                    {
                        score += 1;
                    }
                    else
                    {
                        score -= 1;
                    }
                }
            }
            return score;
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
