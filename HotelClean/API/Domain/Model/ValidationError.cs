using System;
namespace API.Domain.Model
{
    public class ValidationError : Exception
    {
        public ValidationError(string message) : base(message)
        {
        }
    }
}

