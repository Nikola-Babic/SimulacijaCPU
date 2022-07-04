using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCPULibrary
{
    public class SimulatedProcess : SimulatedProgram
    {
        public bool Completed { get; set; } = false;

        public Priority ProcessPriority { get; private set; }

        public CPU[] Affinities { get; private set; }

        public SimulatedProcess(SimulatedProgram program) : base(program.Instructions) { }

        public SimulatedProcess(SimulatedProgram program, Priority processPriority, CPU[] affinities)
            : base(program.Instructions) =>
            (ProcessPriority, Affinities) = (processPriority, affinities);
    }

    public enum Priority { Normal, Low, High };
}
