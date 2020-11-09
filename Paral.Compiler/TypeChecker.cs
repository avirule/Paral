using System;
using System.Collections.Generic;
using Paral.Compiler.Parsing;

namespace Paral.Compiler
{
    public static class TypeChecker
    {
        public const string BYTECODE_INT8 = "BYTE";
        public const string BYTECODE_INT16 = "WORD";
        public const string BYTECODE_INT32 = "DWORD";
        public const string BYTECODE_INT64 = "QWORD";

        public static Scope Global { get; } = new Scope(Guid.NewGuid());

        public static RuntimeType Int8 = new RuntimeType(Global, "i8");
        public static RuntimeType Int16 = new RuntimeType(Global, "i16");
        public static RuntimeType Int32 = new RuntimeType(Global, "i32");
        public static RuntimeType Int64 = new RuntimeType(Global, "i64");

        public static RuntimeType UInt8 = new RuntimeType(Global, "u8");
        public static RuntimeType UInt16 = new RuntimeType(Global, "u16");
        public static RuntimeType UInt32 = new RuntimeType(Global, "u32");
        public static RuntimeType UInt64 = new RuntimeType(Global, "u64");

        public static HashSet<string> Primitives { get; } = new HashSet<string>
        {
            "i8",
            "i16",
            "i32",
            "i64",
            "u8",
            "u16",
            "u32",
            "u64"
        };

        public static HashSet<RuntimeType> Types { get; } = new HashSet<RuntimeType>
        {
            Int8,
            Int16,
            Int32,
            Int64,

            UInt8,
            UInt16,
            UInt32,
            UInt64
        };

        public static Dictionary<RuntimeType, string> PrimitiveBytecodes = new Dictionary<RuntimeType, string>
        {
            { Int8, BYTECODE_INT8 },
            { Int16, BYTECODE_INT16 },
            { Int32, BYTECODE_INT32 },
            { Int64, BYTECODE_INT64 },

            { UInt8, BYTECODE_INT8 },
            { UInt16, BYTECODE_INT16 },
            { UInt32, BYTECODE_INT32 },
            { UInt64, BYTECODE_INT64 }
        };
    }
}
