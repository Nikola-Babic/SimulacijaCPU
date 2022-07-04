#include <iostream>
#include <vector>

using namespace SimCPULibrary;
using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;

ref class RoundRobinScheduler : public SimCPULibrary::Scheduler
{
	array<Registers^>^ registers;
	int currentProcess;
public:
	RoundRobinScheduler(array<CPU^>^ cpus, TimeSpan tick, array<SimulatedProgram^>^ programs)
		: Scheduler(cpus, tick, programs)
	{
		currentProcess = 0;
		registers = gcnew array<Registers^>(programs->Length);
		for (int i = 0; i < programs->Length; ++i)
			registers[i] = gcnew Registers();
	}

	void SchedulerInterrupt() override
	{
		registers[currentProcess] = cpus[0]->Registers;
		int temp = currentProcess;
		while (processes[currentProcess = (currentProcess + 1) % registers->Length]->Completed)
			if (currentProcess == temp)
				return;
		cpus[0]->CurrentProcess = processes[currentProcess];
		cpus[0]->Registers = registers[currentProcess];
	}
};

int main()
{
	std::cout << "Program started." << std::endl << std::endl;

	CPU^ cpu1 = gcnew CPU(0, TimeSpan::FromMilliseconds(100));
	List<Object^>^ list1 = gcnew List<Object^>();
	List<Object^>^ list2 = gcnew List<Object^>();
	auto instr1 = gcnew ValueTuple<OpCode, RegCode, String^>(OpCode::Mov, RegCode::A, gcnew String("test122"));
	auto instr2 = gcnew ValueTuple<OpCode, RegCode, String^>(OpCode::Mov, RegCode::A, gcnew String("test2"));
	for (int i = 0; i < 10; ++i)
	{
		list1->Add(instr1);
		list1->Add(OpCode::PrintLn);
		list2->Add(instr2);
		list2->Add(OpCode::PrintLn);
	}
	SimulatedProgram^ program1 = gcnew SimulatedProgram(list1);
	SimulatedProgram^ program2 = gcnew SimulatedProgram(list2);
	Scheduler^ scheduler = gcnew RoundRobinScheduler(gcnew array<CPU^>{ cpu1 }, TimeSpan::FromMilliseconds(200), gcnew array<SimulatedProgram^>{ program1, program2 });
	scheduler->Run();

	std::cout << std::endl;
	std::cout << "Program ended. Press any key to close.";
	std::cout << std::endl;

	getchar();
}