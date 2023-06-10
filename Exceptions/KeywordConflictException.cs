using LOGrasper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Exceptions
{
    public class KeywordConflictException : Exception
    {
        private string keyword;

        public string ExistingKeyword { get; }
        public string IncomingKeyword { get; }

        public KeywordConflictException(string existingKeyword, string incomingKeyword)
        {
            ExistingKeyword = existingKeyword;
            IncomingKeyword = incomingKeyword;
        }

        public KeywordConflictException(string? message, string existingKeyword, string incomingKeyword) : base(message)
        {
            ExistingKeyword = existingKeyword;
            IncomingKeyword = incomingKeyword;
        }

        public KeywordConflictException(string? message, Exception? innerException, string existingKeyword, string incomingKeyword) : base(message, innerException)
        {
            ExistingKeyword = existingKeyword;
            IncomingKeyword = incomingKeyword;
        }

    }
}
