﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkExceptions
{
    public class WrongUserCredentialsException : ServerException
    {
        public WrongUserCredentialsException(string message) : base(message)
        {

        }
    }
}