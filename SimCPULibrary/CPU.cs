using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimCPULibrary
{
    public class CPU
    {
        Registers registers = new Registers();

        internal object locker;
        internal bool halt = false;

        public SimulatedProcess CurrentProcess { get; set; }
        public TimeSpan Tick { get; private set; }
        public int Id { get; private set; }

        public Registers Registers
        {
            get => new Registers(registers);
            set => registers = new Registers(value);
        }

        public CPU(int id, TimeSpan tick) => (Id, Tick) = (id, tick);

        void AddValueToRegister(RegCode register, int value)
        {
            switch (register)
            {
                case RegCode.A:
                    registers.A += value.ToString();
                    break;
                case RegCode.B:
                    registers.B += value;
                    break;
                case RegCode.C:
                    registers.C += value;
                    break;
            }
        }

        object GetRegisterValue(RegCode register)
        {
            switch (register)
            {
                case RegCode.A:
                    return registers.A;
                case RegCode.B:
                    return registers.B;
                case RegCode.C:
                    return registers.C;
                default:
                    throw new NotImplementedException("Requested register is not implemented.");
            }
        }

        void ExecuteNextInstruction()
        {
            if (CurrentProcess == null || CurrentProcess.Completed)
                return;
            object instr = CurrentProcess[registers.IP++];
            switch (instr)
            {
                case null:
                    CurrentProcess = null;
                    break;
                case OpCode.Exit:
                    CurrentProcess.Completed = true;
                    break;
                case OpCode.Print:
                    Console.Write(registers.A);
                    break;
                case OpCode.PrintLn:
                    Console.WriteLine($"CPU {Id}: {registers.A}");
                    break;
                case ValueTuple<OpCode, RegCode> x when x.Item1 == OpCode.Print || x.Item1 == OpCode.PrintLn:
                    (OpCode printCode, RegCode printRegCode) = x;
                    Action<object> f = (printCode == OpCode.Print) ? new Action<object>(y => Console.Write(y)) : y => Console.WriteLine(y);
                    if (printRegCode == RegCode.A)
                        f(registers.A);
                    else if (printRegCode == RegCode.B)
                        f(registers.B);
                    else if (printRegCode == RegCode.C)
                        f(registers.C);
                    break;
                case ValueTuple<OpCode, RegCode, string> x when x.Item1 == OpCode.Mov && x.Item2 == RegCode.A:
                    (_, _, string value) = x;
                    registers.A = value;
                    break;
                case ValueTuple<OpCode, RegCode, int> x when x.Item1 == OpCode.Mov:
                    (_, RegCode code, int @int) = x;
                    if (code == RegCode.B)
                        registers.B = @int;
                    else
                        registers.C = @int;
                    break;
                case ValueTuple<OpCode, RegCode, int> x when x.Item1 == OpCode.Add:
                    (_, RegCode addRegCode, int addValue) = x;
                    AddValueToRegister(addRegCode, addValue);
                    break;
                case ValueTuple<OpCode, RegCode, RegCode> x when x.Item1 == OpCode.Add && x.Item3 != RegCode.A:
                    (_, RegCode addRegCode1, RegCode addRegCode2) = x;
                    AddValueToRegister(addRegCode1, (int)GetRegisterValue(addRegCode2));
                    break;
                case ValueTuple<OpCode, int> x when x.Item1 == OpCode.Jmp || (x.Item1 == OpCode.Jz && registers.IsZero) || (x.Item1 == OpCode.Jnz && !registers.IsZero):
                    (_, int address) = x;
                    registers.IP = address;
                    break;
                case ValueTuple<OpCode, int> x when (x.Item1 == OpCode.Jz && !registers.IsZero) || (x.Item1 == OpCode.Jnz && registers.IsZero):
                    break;
                case ValueTuple<OpCode, Action> x when x.Item1 == OpCode.Call:
                    x.Item2();
                    break;
                default:
                    throw new NotImplementedException("The instruction is not implemented.");
            }
        }

        public Thread Run()
        {
            Thread thread = new Thread(() =>
            {
                while (!halt)
                {
                    Thread.Sleep(Tick);
                    lock (locker)
                        ExecuteNextInstruction();
                }
            });
            thread.Start();
            return thread;
        }

        public async Task RunAsync() =>
            await Task.Run(async () =>
            {
                while (!halt)
                {
                    await Task.Delay(Tick);
                    lock (locker)
                        ExecuteNextInstruction();
                }
            });
    }
}
