using System;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace Paral.Compiler.Compiling
{
    public class InstructionBuilder : IDisposable
    {
        private const string _ASSEMBLY_INSTRUCTION_TWO_ARGUMENTS = "{0} {1}, {2}\r\n";

        private readonly PipeWriter _Writer;

        public InstructionBuilder(Stream stream) => _Writer = PipeWriter.Create(stream);

        public async ValueTask AppendMOV(string destination, string source)
        {
            string line = string.Format(_ASSEMBLY_INSTRUCTION_TWO_ARGUMENTS, Instructions.MOV, destination, source);
            Memory<byte> stringMemory = new ByteStringMemoryManager(line).Memory;
            await _Writer.WriteAsync(stringMemory);
        }

        public void AppendADD(string destination, string source)
        {

        }


        #region IDisposable

        public void Dispose()
        {
            _Writer.Complete();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
