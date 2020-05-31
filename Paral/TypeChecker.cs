#region

using System.Collections.Generic;
using Paral.Exceptions;

#endregion

namespace Paral
{
    public static class TypeChecker
    {
        public static readonly HashSet<RuntimeType> DeclaredTypes = new HashSet<RuntimeType>();
        public static readonly HashSet<RuntimeType> UsedTypes = new HashSet<RuntimeType>();

        public static bool VerifyTypes()
        {
            foreach (RuntimeType usedType in UsedTypes)
            {
                if (!DeclaredTypes.Contains(usedType))
                {
                    ExceptionHelper.Error(default, $"Type \"{usedType}\" is invalid.");
                    return false;
                }
            }

            return true;
        }
    }
}
