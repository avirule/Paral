#region

using System.Collections.Generic;

#endregion


namespace Paral.Parsing
{
    public static class ParsingHelper
    {
        public static readonly HashSet<string> LocalityIdentifiers = new HashSet<string>
        {
            "public",
            "private"
        };
    }
}
