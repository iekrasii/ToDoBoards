using System.Collections.Generic;

namespace ToDoBoards.Common.Models
{
    /// <summary>
    /// Board model
    /// </summary>
    public class Board : BaseEntity
    {
        /// <summary>
        /// The title of the Board
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// ToDos of the Board
        /// </summary>
        public List<ToDo> ToDos { get; set; }
    }
}