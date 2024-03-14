using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Model
{
    /// <summary>
    /// Data transfer object for the position
    /// </summary>
    public class PositionDTO : IEquatable<PositionDTO>
    {
        /// <summary>
        /// The latitude of a position
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// The longitude of a position
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// The timestamp of the location update
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The soldier, which the position belongs to
        /// </summary>
        public SoldierDTO Soldier { get; set; }

        public PositionDTO(SoldierDTO soldier, double latitude, double longitude, DateTime timestamp)
        {
            Soldier = soldier;
            Latitude = latitude;
            Longitude = longitude;
            Timestamp = timestamp;
        }

        public PositionDTO(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Compares to positions
        /// </summary>
        /// <param name="other">The other position</param>
        /// <returns>If the positions are equal</returns>
        public bool Equals(PositionDTO other)
        {
            return 
                this.Soldier == other.Soldier && 
                this.Latitude == other.Latitude &&
                this.Longitude == other.Longitude;
        }
    }
}
