using System;

namespace ToDoBoards.Common.Models
{
    public class BaseEntity
    {
        /// <summary>
        /// ID of the entity
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Creation timestamp
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Update timestamp
        /// </summary>
        public DateTime Updated { get; set; }
    }
}