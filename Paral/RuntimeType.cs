#region

using System;

#endregion

namespace Paral
{
    public class RuntimeType
    {
        public Guid ID { get; }
        public string Name { get; }

        public RuntimeType(string name)
        {
            // todo get typeid from global table
            ID = Guid.NewGuid();
            Name = name;

            TypeChecker.UsedTypes.Add(this);
        }

        public override int GetHashCode() => ID.GetHashCode();
    }
}
