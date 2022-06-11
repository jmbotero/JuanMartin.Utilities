using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuanMartin.Utilities.Euler.Objects
{
    public class Cube
    {
        public long Value { get; set; }
        public int PermutationsCount { get; set; }

        public Cube(long value)
        {
            this.PermutationsCount = 0;
            this.Value = value;
        }
    }
}
