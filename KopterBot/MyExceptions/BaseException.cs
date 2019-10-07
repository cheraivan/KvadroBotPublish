using System;

namespace KopterBot
{
    class BaseException :System.Exception
    {
        public BaseException(string message,System.Exception ex) :base(message,ex){ }
        public BaseException(string message) : base(message) { }
        public BaseException() { }
    }
}
