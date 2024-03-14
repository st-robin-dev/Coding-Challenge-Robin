using StorageModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageModule.Interfaces
{
    /// <summary>
    /// The database context for storing trainings, soldiers and positions in the database
    /// </summary>
    internal interface IDatabaseContext : IDisposable
    {
        /// <summary>
        /// Adds a list of trainings
        /// </summary>
        /// <param name="trainings">The list of the trainings</param>
        void AddTrainings(List<TrainingDataModel> trainings);

        /// <summary>
        /// Gets all trainings stored
        /// </summary>
        /// <returns>A list with training objects</returns>
        List<TrainingDataModel> GetTrainings();

        /// <summary>
        /// Gets all soldiers stored
        /// </summary>
        /// <returns>A list with soldier objects</returns>
        List<SoldierDataModel> GetSoldiers();

        /// <summary>
        /// Gets the latest positions stored grouped per soldier
        /// </summary>
        /// <returns>A list with the latest position per soldier</returns>
        List<PositionDataModel> GetPositions();

        /// <summary>
        /// Adds a list of soliders
        /// </summary>
        /// <param name="soldiers">The list of the soldiers</param>
        void AddSoldiers(List<SoldierDataModel> soldiers);

        /// <summary>
        /// Adds a list of positions
        /// </summary>
        /// <param name="positions">The list of the positions</param>
        void AddPositions(List<PositionDataModel> positions);

        /// <summary>
        /// Checks, if soldiers are available
        /// </summary>
        /// <returns>If soldiers are available or not</returns>
        bool HasSoldiers();

        /// <summary>
        /// Clears everything from the db
        /// </summary>
        void ClearDB();
    }
}
