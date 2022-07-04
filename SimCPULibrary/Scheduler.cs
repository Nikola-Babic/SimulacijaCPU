using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimCPULibrary
{
    public abstract class Scheduler
    {
        protected CPU[] cpus;
        TimeSpan tick;
        protected SimulatedProcess[] processes;
        object locker = new object();

        public Scheduler(CPU[] cpus, TimeSpan tick, SimulatedProgram[] programs) =>
            (this.cpus, this.tick, processes) = (cpus, tick, programs.Select(x => new SimulatedProcess(x)).ToArray());

        public Scheduler(CPU[] cpus, TimeSpan tick, (SimulatedProgram program, Priority priority, CPU[] affinities)[] programs)
        {
            if (programs.Any(x => x.affinities.Except(cpus).Any()))
                throw new ArgumentException("The affinities can only be specified for CPUs that the scheduler uses.");
            (this.cpus, this.tick, processes) = (cpus, tick, programs.Select(x => new SimulatedProcess(x.program, x.priority, x.affinities)).ToArray());
        }

        private void AssignLocks()
        {
            foreach (CPU cpu in cpus)
                cpu.locker = locker;
        }

        private void HaltCPUs()
        {
            foreach (CPU cpu in cpus)
                cpu.halt = true;
        }

        public void Run()
        {
            AssignLocks();
            Thread[] cpusThreads = cpus.Select(x => x.Run()).ToArray();
            while (processes.Any(x => !x.Completed))
            {
                Thread.Sleep(tick);
                lock (locker)
                    SchedulerInterrupt();
            }
            HaltCPUs();
            foreach (Thread thread in cpusThreads)
                thread.Join();
        }

        public async Task RunAsync() =>
            await Task.Run(async () =>
            {
                AssignLocks();
                Task[] cpuTasks = cpus.Select(x => x.RunAsync()).ToArray();
                while (processes.Any(x => !x.Completed))
                {
                    await Task.Delay(tick);
                    lock (locker)
                        SchedulerInterrupt();
                }
                HaltCPUs();
                await Task.WhenAll(cpuTasks);
            });

        public abstract void SchedulerInterrupt();
    }
}
