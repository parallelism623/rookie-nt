﻿namespace mvc_todolist.Commons.Exceptions
{
    public abstract class BaseException : Exception
    {
        public BaseException(string message) : base(message)
        {
        }
    }
}
