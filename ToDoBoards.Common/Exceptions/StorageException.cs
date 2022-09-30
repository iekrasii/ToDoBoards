using System;

namespace ToDoBoards.Common.Exceptions
{
    /// <summary>
    /// Wraps exceptions occurred in Storage
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class StorageException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageException"/> class
        /// </summary>
        /// <param name="ex">The exception</param>
        public StorageException(Exception ex) : base("Problem occurred in storage", ex)
        {
        }
    }
}