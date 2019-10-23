using System;
using System.Runtime.Serialization;

namespace Application.Exceptions
{
    [Serializable]
    public class InvalidContact : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public InvalidContact()
        { }

        public InvalidContact(string message) : base(message)
        { }

        public InvalidContact(string message, Exception inner) : base(message, inner)
        { }

        protected InvalidContact(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        { }
    }
}