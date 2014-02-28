namespace Wolfenstein.Common
{
    public class GameException : System.ApplicationException
    {
        private const string DefaultMessage = "Game exception:";

        public GameException(string message)
            : base(string.Format("{0} {1}", DefaultMessage, message))
        {
        }
    }
}