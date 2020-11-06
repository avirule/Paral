#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public interface IGrouping { }

    public interface IIntent { }

    public class Parenthetic : IGrouping { }

    public class Bracket : IGrouping { }

    public class Brace : IGrouping { }

    public class Open : IIntent { }

    public class Close : IIntent { }

    public class GroupToken<TGrouping, TIntent> : Token where TGrouping : IGrouping where TIntent : IIntent
    {
        public GroupToken(Point location) : base(location) { }
    }
}
