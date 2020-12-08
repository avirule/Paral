#region

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Iced.Intel;
using Paral.Compiler.Compiling;

#endregion


namespace Paral.Compiler
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Guid guid = new Guid(0x9576e91, 0x6d3f, 0x11d2, 0x8e, 0x39, 0x0, 0xa0, 0xc9, 0x69, 0x72, 0x3b);
            Console.WriteLine(guid.ToString());



            string temp = Path.GetTempFileName();

            await using (FileStream fileStream = new FileStream(temp, FileMode.Create))
            {
                InstructionBuilder instructionBuilder = new InstructionBuilder(fileStream);
                await instructionBuilder.AppendMOV(Registers.X86.EAX, Registers.X86.ECX);
            }

            string str = "asdadasd";
            var chasrs = str.AsMemory();

            string content = File.ReadAllText(temp);

            Process? process = Process.Start(@"./References/fasmg.exe", $"\"{temp}\" out.o");
            process.ErrorDataReceived += (_, eventArgs) => Console.Write(eventArgs.Data);


            // Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            // Stopwatch stopwatch = Stopwatch.StartNew();
            //
            // Parser parser = new Parser(File.OpenRead("TestFiles/Test_Functions.paral"));
            // Module module = await parser.Parse();
            //
            // Log.Information($"Total run time: {stopwatch.Elapsed.TotalMilliseconds:0.00}ms");
            //
            // if (module.Completed) return;
        }
    }
}
