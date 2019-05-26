using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exhibition.Core
{
    public class FileUpoadException : Exception
    {
        public FileUpoadException(string message)
            : base(message)
        {

        }
    }
}
