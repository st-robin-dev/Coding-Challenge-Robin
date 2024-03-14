using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Model
{
    /// <summary>
    /// Data transfer object for the soldier
    /// </summary>
    public class SoldierDTO
    {
        /// <summary>
        /// The position of the soldier
        /// </summary>
        public PositionDTO Position { get; set; }

        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The rank
        /// </summary>
        public string Rank { get; set; }

        /// <summary>
        /// The country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// The reference to the training
        /// </summary>
        public TrainingDTO Training { get; set; }

    }
}
