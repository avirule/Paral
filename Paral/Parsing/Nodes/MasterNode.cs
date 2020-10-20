using System.Collections.Generic;
using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class MasterNode : Node
    {
        public override void ConsumeToken(Token token)
        {
            switch (token)
            {
                case KeywordToken keywordToken when keywordToken.Value == Keyword.Namespace:
                    
            }
        }
    }
}
