// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace Paral.Compiler.Compiling
{
    public static class Registers
    {
        public static class X64
        {
            public const string RAX = "eax";
            public const string RBX = "ebx";
            public const string RCX = "ecx";
            public const string RDX = "edx";
            public const string RSI = "esi";
            public const string RDI = "edi";
            public const string RBP = "ebp";
            public const string RSP = "esp";
            public const string R8 = "r8";
            public const string R9 = "r9";
            public const string R10 = "r10";
            public const string R11 = "r11";
            public const string R12 = "r12";
            public const string R13 = "r13";
            public const string R14 = "r14";
            public const string R15 = "r15";
        }

        public static class X86
        {
            public const string EAX = "eax";
            public const string EBX = "ebx";
            public const string ECX = "ecx";
            public const string EDX = "edx";
            public const string ESI = "esi";
            public const string EDI = "edi";
            public const string EBP = "ebp";
            public const string ESP = "esp";
            public const string R8D = "r8d";
            public const string R9D = "r9d";
            public const string R10D = "r10d";
            public const string R11D = "r11d";
            public const string R12D = "r12d";
            public const string R13D = "r13d";
            public const string R14D = "r14d";
            public const string R15D = "r15d";
        }

        public static class X16 { }

        public static class X8 { }
    }
}
