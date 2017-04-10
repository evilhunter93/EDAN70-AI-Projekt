using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tak.Game;

namespace Tak.Game
{
    class Game
    {
        GameBoard board;
        Player p1;
        Player p2;
        String winCond;

        public void initialize(int size, String p1, String p2)
        {
            board = new GameBoard(size);
            if (p1 == "Human")
            {
                this.p1 = new HumanPlayer(Colour.White);
            }
            else
            {
                this.p1 = new AIPlayer(Colour.White);
            }
            if (p2 == "Human")
            {
                this.p2 = new HumanPlayer(Colour.Black);
            }
            else
            {
                this.p2 = new AIPlayer(Colour.Black);
            }
            winCond = turnManager(this.p1, this.p2);
        }

        private string turnManager(Player p1, Player p2)
        {
            switch (board.GameState)
            {
                case GameState.InProgress:
                    p1.DoMove();
                    return turnManager(p2, p1);
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
