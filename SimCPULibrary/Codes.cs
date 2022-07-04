using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCPULibrary
{
    public enum OpCode { Print, PrintLn, Add, Jmp, Jz, Jnz, Mov, Call, Exit }
    public enum RegCode { A, B, C };
}
