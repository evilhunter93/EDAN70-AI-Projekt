using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Game;
using Tak.GUI;

namespace Tak.Game
{
    class Game
    {
        GameBoard board;
        Player p1;
        Player p2;
        string winCond;
        ASCIIGUI gui;

        public void Initialize(int size, string p1, string p2)
        {
            board = new GameBoard(size);
            Interpreter interpreter = new Interpreter(board);
            gui = new ASCIIGUI(board, size);
            if (p1 == "Human")
            {
                this.p1 = new HumanPlayer(Colour.White, interpreter);
            }
            else
            {
                this.p1 = new AIPlayer(Colour.White, interpreter);
            }
            if (p2 == "Human")
            {
                this.p2 = new HumanPlayer(Colour.Black, interpreter);
            }
            else
            {
                this.p2 = new AIPlayer(Colour.Black, interpreter);
            }
            winCond = TurnManager(this.p1, this.p2);
        }

        private string TurnManager(Player p1, Player p2)
        {
            while (board.GameState == GameState.InProgress)
            {
                p1.DoMove();
                gui.Draw();
                p2.DoMove();
                gui.Draw();
            }
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
    }
}
