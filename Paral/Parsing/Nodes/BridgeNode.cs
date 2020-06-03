#region

using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public abstract class BridgeNode : Node
    {
        public Node? Child { get; protected set; }

        public BridgeNode() => Child = null;

        protected override bool ConsumeTokenInternal(Token token)
        {
            if (Child == null)
            {
                AttemptInitializeChild(token);
            }
            else if (!Child.Complete)
            {
                return Child.ConsumeToken(token);
            }
            else
            {
                ExceptionHelper.Error(token, ExceptionHelper.INVALID_COMPILER_STATE);
            }

            return Child?.Complete ?? false;
        }

        protected abstract void AttemptInitializeChild(Token token);
    }
}
