using Contracts.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SoldierSimulationModule.Services;
using StorageModule.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SoldierSimulationModuleTest
{
    [TestClass]
    public class SoldierSimulationServiceTests
    {
        [TestMethod]
        public void Initialize_CreateSoldiers_SoldiersCreated()
        {
            #region arrange
            var storageService = new Mock<IStorageService>();
            storageService.Setup(x => x.HasSoldiers()).Returns(false);

            var soldierSimulationService = new SoldierSimulationService(storageService.Object);

            int targetTrainingCount = 5;
            int targetSoldierCount = 20;
            #endregion

            #region act
            //Initialize the trainings and soldiers
            soldierSimulationService.Initialize(targetTrainingCount, targetSoldierCount);
            #endregion

            #region assert
            int trainingCount = soldierSimulationService.GetTrainings().Count;
            int soldierCount = soldierSimulationService.GetSoldiers().Count;

            //Compare the target count with the actual created one
            Assert.AreEqual(targetTrainingCount, trainingCount);
            Assert.AreEqual(targetSoldierCount, soldierCount);

            //Check that the trainings and soldiers would have been added to the database
            storageService.Verify(x => x.AddTrainings(It.IsAny<List<TrainingDTO>>()), Times.Once);
            storageService.Verify(x => x.AddSoldiers(It.IsAny<List<SoldierDTO>>()), Times.Once);
            #endregion
        }

        [TestMethod]
        public void UpdateLocation_OneUpdate_PositionMoved()
        {
            #region arrange
            var storageService = new Mock<IStorageService>();
            storageService.Setup(x => x.HasSoldiers()).Returns(false);

            var soldierSimulationService = new SoldierSimulationService(storageService.Object);

            int targetTrainingCount = 1;
            int targetSoldierCount = 2;

            int eventRaiseCount = 0;
            soldierSimulationService.SoldierUpdated += delegate (object sender, SoldierDTO soldier)
            {
                eventRaiseCount++;
            };
            #endregion

            #region act
            soldierSimulationService.Initialize(targetTrainingCount, targetSoldierCount);

            //Get the position before the update
            var positionsBefore = soldierSimulationService.GetSoldiers().Select(x => x.Position).ToList();

            //Update the location of the soldiers
            soldierSimulationService.UpdateLocation();

            //Get the position after the update
            var positionsAfter = soldierSimulationService.GetSoldiers().Select(x => x.Position).ToList();
            #endregion

            #region assert
            var beforeCount = positionsBefore.Count;
            var afterCount = positionsAfter.Count;

            //To check that no soldier got lost
            Assert.AreEqual(beforeCount, afterCount);

            //To check that the soldiers are still the same
            Assert.AreEqual(positionsBefore[0].Soldier.Id, positionsAfter[0].Soldier.Id);
            Assert.AreEqual(positionsBefore[1].Soldier.Id, positionsAfter[1].Soldier.Id);

            //To check that the position actually updated (only the longitude gets updated from the service)
            Assert.AreNotEqual(positionsBefore[0].Longitude, positionsAfter[0].Longitude);
            Assert.AreNotEqual(positionsBefore[1].Longitude, positionsAfter[1].Longitude);

            //To check that the soldier updated event is acually called, when position is updated
            Assert.AreEqual(targetSoldierCount, eventRaiseCount);

            //To check that the new positions would have been added to the db
            storageService.Verify(x => x.AddPositions(It.IsAny<List<PositionDTO>>()), Times.Once);
            #endregion
        }
    }
}
