#region

using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public abstract class BranchNode : Node
    {
        public Node? Child { get; protected set; }

        public BranchNode() => Child = null;

        public override void Consume(Token token)
        {
            if (Child == null)
            {
                AttemptInitializeChild(token);
            }
            else if (!Child.Complete)
            {
                Child.Consume(token);
            }
            else
            {
                ExceptionHelper.Error(token, "Compiler has entered invalid control flow.");
            }
        }

        protected abstract void AttemptInitializeChild(Token token);
    }
}
