#region

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Paral.Lexing;
using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public abstract class Node
    {
        protected List<Token> _Tokens { get; }

        public bool Completed { get; private set; }

        public Node() => _Tokens = new List<Token>();

        public bool ConsumeToken(Token token) => Completed = ConsumeTokenInternal(token);

        protected abstract bool ConsumeTokenInternal(Token token);

        protected static void Expect<TExpected>(Token actual) where TExpected : Token
        {
            if (actual is not TExpected) throw new ExpectedTokenException(typeof(TExpected), actual);
        }

        protected static void Expect(Token actual, params Type[] required) => required.Any(type => actual.GetType().IsInstanceOfType(type));
    }

    public abstract class BranchNode : Node
    {
        public List<Node> Branches { get; }

        public BranchNode() => Branches = new List<Node>();

        protected NamespaceNode? FindOrCreateNamespace(Queue<string> identities)
        {
            BranchNode current = this;

            while (identities.TryDequeue(out string? identity))
            {
                if (current.TryFindNamespace(identity, out NamespaceNode? next)) current = next;
                else
                {
                    next = new NamespaceNode(identity);
                    current.Branches.Add(next);
                    current = next;
                }
            }

            return current as NamespaceNode;
        }

        private bool TryFindNamespace(string identity, [NotNullWhen(true)] out NamespaceNode? namespaceNode)
        {
            namespaceNode = Branches.SingleOrDefault(node => node is NamespaceNode current && current.Identity.Equals(identity)) as NamespaceNode;
            return namespaceNode is not null;
        }
    }
}
