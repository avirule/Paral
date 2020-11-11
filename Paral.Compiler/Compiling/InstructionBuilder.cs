using System.Collections.Generic;

namespace Paral.Compiler.Compiling
{
    public class InstructionBuilder
    {
        private const string _ASSEMBLY_INSTRUCTION_TWO_ARGUMENTS = "{0} {1}, {2}\r\n";

        public List<string> AssemblyLines { get; }

        public void AppendMOV(string destination, string source)
        {
                AssemblyLines.Add(string.Format(_ASSEMBLY_INSTRUCTION_TWO_ARGUMENTS, Instructions.MOV, destination, source));
        }

        public void AppendADD(string destination, string source)
        {

        }
    }
}
