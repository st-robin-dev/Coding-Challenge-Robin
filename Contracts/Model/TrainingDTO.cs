using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Model
{
    /// <summary>
    /// The data transfer object of the training
    /// </summary>
    public class TrainingDTO
    {
        /// <summary>
        /// The id of the training
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the training
        /// </summary>
        public string Name { get; set; }
    }
}
