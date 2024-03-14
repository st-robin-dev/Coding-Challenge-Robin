using CodingChallenge.Interfaces;
using CodingChallenge.Services;
using Contracts.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoldierSimulationModule.Interfaces;
using SoldierSimulationModule.Services;
using StorageModule.Interfaces;
using StorageModule.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegrationTest
{
    /// <summary>
    /// Executes a integration test for updating soldier positions
    /// The test goes from initializing soldiers, update the location of them
    /// write the updated location in the db and send it to the UI update buffer
    /// A separate database is used for doing the integration test
    /// </summary>
    [TestClass]
    public class IntegrationTest
    {
        [TestMethod]
        public void IntegrationTest_UpdatePosition()
        {
            #region arrange
            IStorageService storageService = new StorageService();
            ISoldierSimulationService soldierSimulationService = new SoldierSimulationService(storageService);
            ISoldierUpdateService soldierUpdateService = new SoldierUpdateService();

            soldierSimulationService.SoldierUpdated += delegate (object sender, SoldierDTO soldier)
            {
                soldierUpdateService.Post(soldier.Id, soldier);
            };

            var targetTrainingCount = 2;
            var targetSoldierCount = 3;
            #endregion

            #region act
            //Make sure db is empty
            storageService.ClearStorage();

            //Initialize trainings and soldiers with the specified amount
            soldierSimulationService.Initialize(targetTrainingCount, targetSoldierCount);

            //Update the location of the soldiers
            soldierSimulationService.UpdateLocation();

            //Getting the current trainings and the one from db
            var currentTrainings = soldierSimulationService.GetTrainings();
            var dbTrainings = storageService.GetTrainings();

            //Getting the current soldiers and the one from db
            var currentSolders = soldierSimulationService.GetSoldiers();
            var dbSoldiers = storageService.GetSoldiers();

            //Get the count of the soldiers waiting in the buffer for ui update
            var updateBufferCount = soldierUpdateService.GetOrder().Count;

            //Update the location a second time
            soldierSimulationService.UpdateLocation();

            //Get the buffer size again to compare witgh previous one
            var doubleUpdateBufferCount = soldierUpdateService.GetOrder().Count;

            //Get the latest position from db and from current soldiers
            var latestPositions = storageService.GetPositions();
            var latestCurrentSoldiers = soldierSimulationService.GetSoldiers();

            //Find the same soldier in this position for comparison
            var latestDbSoldierPosition = latestPositions.First();
            var currentLatestSoldierPosition = latestCurrentSoldiers.First(x => x.Id == latestDbSoldierPosition.Soldier.Id).Position;
            soldierUpdateService.Receive(latestDbSoldierPosition.Soldier.Id, out var bufferedSoldier);
            #endregion

            #region assert
            //Compare the amount of soldiers waiting in the buffer to the created one
            Assert.AreEqual(targetSoldierCount, updateBufferCount);

            //Even after a second location update they shoud not increase, because the buffer replaces them
            Assert.AreEqual(targetSoldierCount, doubleUpdateBufferCount);

            //Comapre the counts of the current trainings and the one in db
            Assert.AreEqual(targetTrainingCount, dbTrainings.Count);
            Assert.AreEqual(targetTrainingCount, currentTrainings.Count);

            //Compare the counts of current soldiers and the once in db
            Assert.AreEqual(targetSoldierCount, dbSoldiers.Count);
            Assert.AreEqual(targetSoldierCount, currentSolders.Count);

            //Compare the location from current soldier, the latest db position and the one waiting in the buffer
            Assert.AreEqual(currentLatestSoldierPosition.Longitude, latestDbSoldierPosition.Longitude);
            Assert.AreEqual(currentLatestSoldierPosition.Latitude, latestDbSoldierPosition.Latitude);

            Assert.AreEqual(currentLatestSoldierPosition.Longitude, bufferedSoldier.Soldier.Position.Longitude);
            Assert.AreEqual(currentLatestSoldierPosition.Latitude, bufferedSoldier.Soldier.Position.Latitude);
            #endregion
        }
    }
}
