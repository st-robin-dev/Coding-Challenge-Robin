using Contracts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoldierSimulationModule.Interfaces
{
    /// <summary>
    /// This service allows to create trainings and soldiers and 
    /// simulates location changes on the soldiers.
    /// Trainings, soldiers and position updates are stored in a database.
    /// The location changes can be automated in a regular interval
    /// </summary>
    public interface ISoldierSimulationService
    {
        /// <summary>
        /// Initializes the given amount of trainings and soldiers
        /// </summary>
        /// <param name="trainingCount">The amount of trainings</param>
        /// <param name="soldierCount">The amount of soldiers</param>
        /// <param name="clearStorage">If the storage should be cleared first (optional)</param>
        void Initialize(int trainingCount, int soldierCount, bool clearStorage = false);

        /// <summary>
        /// Occurs when a soldiers gets updated
        /// </summary>
        event EventHandler<SoldierDTO> SoldierUpdated;

        /// <summary>
        /// The center point on the map and startpoint for location updates
        /// </summary>
        PositionDTO InitialPoint { get; }

        /// <summary>
        /// Gets all soldiers
        /// </summary>
        /// <returns>A list with the soldiers</returns>
        List<SoldierDTO> GetSoldiers();

        /// <summary>
        /// Gets all trainings
        /// </summary>
        /// <returns>A list with the trainings</returns>
        List<TrainingDTO> GetTrainings();

        /// <summary>
        /// Updates the location of every soldier available
        /// </summary>
        void UpdateLocation();

        /// <summary>
        /// Starts the automatic cyclic update of the locations of the soldiers
        /// </summary>
        void StartAutoUpdateLocations();

    }
}
