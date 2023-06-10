using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Exceptions
{
    public class InvalidKeywordIndexException : Exception
    {

        public int KeywordIndex { get; }

        public InvalidKeywordIndexException(int index)
        {
            KeywordIndex = index;
        }

        public InvalidKeywordIndexException(string? message, int index) : base(message) 
        {
            KeywordIndex = index;
        }

        public InvalidKeywordIndexException(string? message, Exception? innerException, int index) : base(message, innerException)
        {
            KeywordIndex = index;
        }


    }
}
