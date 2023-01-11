using System;

namespace AndreasReitberger.API.Moonraker.Models.Exceptions
{
    public class ServerNotReachableException : Exception
    {
        public ServerNotReachableException()
        {
        }

        public ServerNotReachableException(string message)
            : base(message)
        {
        }

        public ServerNotReachableException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
