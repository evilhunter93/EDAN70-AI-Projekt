using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tak.Game
{
    class Capstone : Stone
    {
        private bool standing = true;

        public override bool Standing
        {
            get
            {
                return standing;
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
