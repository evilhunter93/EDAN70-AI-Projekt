using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Interpreter interpreter = new Interpreter(board);
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
            winCond = turnManager(this.p1, this.p2);
        }

        private string turnManager(Player p1, Player p2)
        {
            switch (board.GameState)
            {
                case InProgress:
                    p1.doMove();
                    return turnManager(p2, p1);
                    break;
                case Tie:
                    winCond = "\nTie";
                    break;
                case (WR || WF):
                    winCond = "\nWhite wins!";
                    break;
                case (BR || BF):
                    winCond = "\nBlack wins!";
                    break;
                default:
                    winCond = "\nInvalid win-condition!";
                    break;
            }
        }
    }
}
