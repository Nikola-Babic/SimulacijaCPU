using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCPULibrary
{
    public class Registers
    {
        private int b, c;

        public string A { get; set; }
        public int B
        {
            get => b;
            set
            {
                IsZero = (b = value) == 0;
            }
        }
        public int C
        {
            get => c;
            set
            {
                IsZero = (c = value) == 0;
            }
        }
        public int IP { get; set; }
        public bool IsZero { get; private set; }

        public Registers(string a, int b, int c, int ip) =>
            (A, B, C, IP) = (a, b, c, ip);

        public Registers() { }

        public Registers(Registers other) :
            this(other.A, other.B, other.C, other.IP)
        { }
    }
}
