using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCPULibrary
{
    public class SimulatedProgram
    {
        public List<object> Instructions { get; } = new List<object>();

        public int Size => Instructions.Count;

        public SimulatedProgram(IEnumerable<object> instructions) =>
            Instructions = instructions.ToList();

        public object this[int i]
        {
            get
            {
                if (i == Instructions.Count)
                    return OpCode.Exit;
                return i >= 0 && i < Instructions.Count ? Instructions[i] : null;
            }
        }
    }
}
