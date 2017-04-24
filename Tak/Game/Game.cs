using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        ASCIIGUI gui;

        public TakGame(int size, string p1Type, string p2Type)
        {
            Initialize(size, p1Type, p2Type);
        }

        public void Initialize(int size, string p1Type, string p2Type)
        {
            board = new GameBoard(size);
            Interpreter interpreter = new Interpreter(board);
            gui = new ASCIIGUI(board, size);

            if (p1Type == "Human")
                p1 = new HumanPlayer(Colour.White, interpreter);
            else
                p1 = new AIPlayer(Colour.White, interpreter);

            if (p2Type == "Human")
                p2 = new HumanPlayer(Colour.Black, interpreter);
            else
                p2 = new AIPlayer(Colour.Black, interpreter);

            currentPlayer = (p1.Colour == Colour.White) ? p1 : p2;
        }

        public void Start()
        {
            gui.Draw();

            while (board.GameState == GameState.InProgress)
            {
                stacks = board.Stacks;
                try
                {
                    currentPlayer.DoMove();
                    board.EndTurn();
                    currentPlayer = (board.Turn == p1.Colour) ? p1 : p2;
                    gui.Draw();
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
            }

            gui.Write(GameOverText());
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

        }
    }
}
