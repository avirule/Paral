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

        public override void Consume(Token token)
        {
            if (Child == null)
            {
                if (Complete)
                {
                    // this should never be hit, as a bridge node should only
                    // ever set Complete when the child is set
                    ExceptionHelper.Error(token, ExceptionHelper.INVALID_COMPILER_STATE);
                }

                AttemptInitializeChild(token);
                Complete = true;
            }
            else if (!Child.Complete)
            {
                Child.Consume(token);
            }
            else
            {
                ExceptionHelper.Error(token, ExceptionHelper.INVALID_COMPILER_STATE);
            }
        }

        protected abstract void AttemptInitializeChild(Token token);
    }
}
