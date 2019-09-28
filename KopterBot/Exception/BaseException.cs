using System;

namespace KopterBot
{
    class BaseException :Exception
    {
        public BaseException(string message,Exception ex) :base(message,ex){ }
        public BaseException(string message) : base(message) { }
        public BaseException() { }
    }
}
