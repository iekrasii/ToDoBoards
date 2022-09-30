using System;

namespace ToDoBoards.Common.Models
{
    /// <summary>
    /// ToDo model
    /// </summary>
    public class ToDo : BaseEntity
    {
        /// <summary>
        /// Indicates whether this <see cref="ToDo"/> is complete
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        /// The title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Board ID
        /// </summary>
        public Guid BoardId { get; set; }
        
        /// <summary>
        /// The Board ToDo belongs to
        /// </summary>
        public Board Board { get; set; }
    }
}