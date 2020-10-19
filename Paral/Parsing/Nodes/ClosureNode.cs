#region

using Paral.Lexing;
using Paral.Lexing.Tokens;

#endregion

namespace Paral.Parsing.Nodes
{
    /// <summary>
    ///     This node is used to signify a statement closure; it effectively a 'cap' to a node branch.
    /// </summary>
    public class ClosureNode : Node
    {
        protected override bool ConsumeTokenInternal(Token token) => true;
    }
}
