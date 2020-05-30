#region

using System;

#endregion

namespace Paral
{
    public class RuntimeType
    {
        public Guid TypeID { get; }
        public string FriendlyName { get; }

        public RuntimeType(string friendlyName)
        {
            TypeID = Guid.NewGuid();
            FriendlyName = friendlyName;
        }

        public override int GetHashCode() => TypeID.GetHashCode();
    }
}
