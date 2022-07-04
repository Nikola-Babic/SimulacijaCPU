using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimCPULibrary;

namespace DllConsoleTest
{
    public class RoundRobinSingleCPUScheduler : Scheduler
    {
        Registers[] registers;
        int currentProcess = 0;
        public static int counter = 0;

        public RoundRobinSingleCPUScheduler(CPU[] cpus, TimeSpan tick, SimulatedProgram[] programs) :
            base(cpus, tick, programs) =>
            registers = Enumerable.Repeat(new Registers(), programs.Length).ToArray();

        public RoundRobinSingleCPUScheduler(CPU[] cpus, TimeSpan tick, (SimulatedProgram program, Priority priority, CPU[] affinities)[] programs) :
            base(cpus, tick, programs) =>
            registers = Enumerable.Repeat(new Registers(), programs.Length).ToArray();

        public override void SchedulerInterrupt()
        {
           
            registers[currentProcess] = cpus[0].Registers;
            int temp = currentProcess;
            Priority tempP = processes[currentProcess].ProcessPriority;

            if ((int)tempP == 0)
            {
                if (counter < 2) counter++;
                else { currentProcess = (currentProcess + 1) % registers.Length; counter = 0;
                    cpus[0].CurrentProcess = processes[currentProcess];
                    cpus[0].Registers = registers[currentProcess];
                }
            }
        
        }
    }
}
