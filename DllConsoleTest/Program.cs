using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimCPULibrary;

namespace DllConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CPU cpu = new CPU(0, TimeSpan.FromMilliseconds(100));
            SimulatedProgram program1 = new SimulatedProgram(new object[]
            {
                (OpCode.Mov, RegCode.B, 0),
                (OpCode.Mov, RegCode.C, -5),
                (OpCode.Mov, RegCode.A, "Sim1 "),
                (OpCode.Add, RegCode.A, RegCode.B),
                (OpCode.PrintLn),
                (OpCode.Add, RegCode.B, 1),
                (OpCode.Add, RegCode.C, 1),
                (OpCode.Jnz, 2)
            });
            SimulatedProgram program2 = new SimulatedProgram(new object[]
            {
                (OpCode.Mov, RegCode.B, 0),
                (OpCode.Mov, RegCode.C, -6),
                (OpCode.Mov, RegCode.A, "Sim2 "),
                (OpCode.Add, RegCode.A, RegCode.B),
                (OpCode.PrintLn),
                (OpCode.Add, RegCode.B, 1),
                (OpCode.Add, RegCode.C, 1),
                (OpCode.Jnz, 2)
            });

            SimulatedProgram program3 = new SimulatedProgram(new object[]
            {
                (OpCode.Mov, RegCode.B, 0),
                (OpCode.Mov, RegCode.C, -7),
                (OpCode.Mov, RegCode.A, "Sim3 "),
                (OpCode.Add, RegCode.A, RegCode.B),
                (OpCode.PrintLn),
                (OpCode.Add, RegCode.B, 1),
                (OpCode.Add, RegCode.C, 1),
                (OpCode.Jnz, 2)
            });

            SimulatedProgram program4 = new SimulatedProgram(new object[]
         {
                (OpCode.Mov, RegCode.B, 0),
                (OpCode.Mov, RegCode.C, -8),
                (OpCode.Mov, RegCode.A, "Sim4 "),
                (OpCode.Add, RegCode.A, RegCode.B),
                (OpCode.PrintLn),
                (OpCode.Add, RegCode.B, 1),
                (OpCode.Add, RegCode.C, 1),
                (OpCode.Jnz, 2)
         });

            SimulatedProgram program5 = new SimulatedProgram(new object[]
             {
                (OpCode.Mov, RegCode.B, 0),
                (OpCode.Mov, RegCode.C, -9),
                (OpCode.Mov, RegCode.A, "Sim5 "),
                (OpCode.Add, RegCode.A, RegCode.B),
                (OpCode.PrintLn),
                (OpCode.Add, RegCode.B, 1),
                (OpCode.Add, RegCode.C, 1),
                (OpCode.Jnz, 2)
             });

            (SimulatedProgram program, Priority priority, CPU[] affinities)[] processes = { (program1, Priority.Normal, new CPU[] { cpu }), (program2, Priority.Normal, new CPU[] { cpu}), (program3, Priority.Normal, new CPU[] { cpu }), (program4, Priority.Normal, new CPU[] { cpu }), (program5, Priority.Normal, new CPU[] { cpu }) };

            Scheduler scheduler = new RoundRobinSingleCPUScheduler(new CPU[] { cpu }, TimeSpan.FromMilliseconds(100),processes);
            scheduler.Run();
        }
    }
}
