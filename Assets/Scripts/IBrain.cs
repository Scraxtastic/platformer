using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    interface IBrain
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Mutationrate { get; set; }
        public int CurrentMoves { get; set; }
        public int TotalMoves { get; set; }

        public void Mutate();


    }
}
