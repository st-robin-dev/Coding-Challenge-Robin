using Contracts.Model;
using StorageModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageModule.Interfaces
{
    /// <summary>
    /// Service for persisting training, soldier and position data
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Adds a list of trainings
        /// </summary>
        /// <param name="trainings">The list of the trainings</param>
        void AddTrainings(List<TrainingDTO> trainings);

        /// <summary>
        /// Gets all trainings stored
        /// </summary>
        /// <returns>A list with training objects</returns>
        List<TrainingDTO> GetTrainings();

        /// <summary>
        /// Gets all soldiers stored
        /// </summary>
        /// <returns>A list with soldier objects</returns>
        List<SoldierDTO> GetSoldiers();

        /// <summary>
        /// Gets the latest positions stored grouped per soldier
        /// </summary>
        /// <returns>A list with the latest position per soldier</returns>
        List<PositionDTO> GetPositions();

        /// <summary>
        /// Adds a list of soliders
        /// </summary>
        /// <param name="soldiers">The list of the soldiers</param>
        void AddSoldiers(List<SoldierDTO> soldiers);

        /// <summary>
        /// Adds a list of positions
        /// </summary>
        /// <param name="positions">The list of the positions</param>
        void AddPositions(List<PositionDTO> positions);

        /// <summary>
        /// Checks, if soldiers are available
        /// </summary>
        /// <returns>If soldiers are available or not</returns>
        bool HasSoldiers();

        /// <summary>
        /// Clears everything from the persistent storage
        /// </summary>
        void ClearStorage();
    }
}
