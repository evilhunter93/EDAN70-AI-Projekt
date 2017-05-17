using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.AI;
using Tak.Exceptions;
using Tak.Game;
using Tak.GUI;

namespace Tak.Game
{
    public class TakGame
    {
        GameBoard board;
        StoneStack[,] stacks;
        Player p1;
        Player p2;
        Player currentPlayer;
        string winCond;
        string prevMove;
        ASCIIGUI gui;

        public TakGame(int size, string p1Type, string p2Type, int depth = 0)
        {
            board = new GameBoard(size);
            gui = new ASCIIGUI(board, size);

            p1 = CreatePlayer(p1Type, Colour.White, depth);
            p2 = CreatePlayer(p2Type, Colour.Black, depth);
            currentPlayer = (p1.Colour == Colour.White) ? p1 : p2;
        }

        private Player CreatePlayer(string playerType, Colour colour, int depth = 0)
        {
            switch (playerType.ToLower())
            {
                case "human":
                    return new HumanPlayer(board, colour);

                case "minimaxai":
                case "minmaxai":
                case "minimax":
                case "minmax":
                    IAI ai = new MiniMaxAI(board, colour, depth);
                    return new AIPlayer(board, colour, ai);

                case "learningai":
                    throw new NotImplementedException();

                default:
                    throw new TakException("Player not recognised.");
            }
        }

        public void Run()
        {
            stacks = board.StacksCopy;
            SetupRounds();

            board.Turn = Colour.White;

            while (board.GameState == GameState.InProgress)
            {
                try
                {
                    prevMove = currentPlayer.DoMove();
                    board.EndTurn();
                    stacks = board.StacksCopy;
                    currentPlayer = (board.Turn == p1.Colour) ? p1 : p2;
                    gui.Draw((currentPlayer is HumanPlayer) ? "Previous move: " + prevMove : "AI thinking...");
                }
                catch (IllegalInputException e)
                {
                    Rollback();
                    Console.WriteLine(e.Message + "\nPlease choose a new move to perform");
                }
                catch (IllegalMoveException d)
                {
                    Rollback();
                    Console.WriteLine(d.Message + "\nPlease choose a new move to perform");
                }
                catch (TakException e)
                {
                    Console.WriteLine(e.Message + "\nPlease choose a new move to perform");
                }
            }
            gui.Draw();
            gui.Write(GameOverText());
        }

        private void SetupRounds()
        {
            board.Turn = Colour.Black;
            int turns = 2;
            gui.Draw();
            currentPlayer = (board.Turn == p1.Colour) ? p2 : p1;

            while (board.GameState == GameState.InProgress && turns > 0)
            {
                try
                {
                    prevMove = currentPlayer.DoMove();
                    board.EndTurn();
                    stacks = board.StacksCopy;
                    currentPlayer = (board.Turn == p1.Colour) ? p2 : p1;
                    gui.Draw((currentPlayer is HumanPlayer) ? "Previous move: " + prevMove : "AI thinking...");
                    turns--;
                }
                catch (IllegalInputException e)
                {
                    Rollback();
                    Console.WriteLine(e.Message + "\nPlease choose a new move to perform");
                }
                catch (IllegalMoveException e)
                {
                    Rollback();
                    Console.WriteLine(e.Message + "\nPlease choose a new move to perform");
                }
                catch (TakException e)
                {
                    Console.WriteLine(e.Message + "\nPlease choose a new move to perform");
                }
            }

        }

        private string GameOverText()
        {
            switch (board.GameState)
            {
                case GameState.Tie:
                    winCond = "\nTie";
                    break;
                case GameState.WR:
                case GameState.WF:
                    winCond = "\nWhite wins!";
                    break;
                case GameState.BR:
                case GameState.BF:
                    winCond = "\nBlack wins!";
                    break;
                default:
                    winCond = "\nInvalid win-condition!";
                    break;
            }

            return winCond;
        }

        private void Rollback()
        {
            board.StacksCopy = stacks;
        }
    }
}
