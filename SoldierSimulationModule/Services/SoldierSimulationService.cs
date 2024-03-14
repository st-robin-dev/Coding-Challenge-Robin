using Contracts.Model;
using SoldierSimulationModule.Interfaces;
using StorageModule.Interfaces;
using StorageModule.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SoldierSimulationModule.Services
{
    /// <inheritdoc/>
    public class SoldierSimulationService : ISoldierSimulationService
    {
        private List<SoldierDTO> _soldiers = new List<SoldierDTO>();
        private List<TrainingDTO> _trainings = new List<TrainingDTO>();

        private static Random rnd = new Random();

        private IStorageService _storageService;

        /// <inheritdoc/>
        public PositionDTO InitialPoint => new PositionDTO(47.74643, 7.34734);

        public SoldierSimulationService() : this(new StorageService())
        { }

        public SoldierSimulationService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        /// <inheritdoc/>
        public event EventHandler<SoldierDTO> SoldierUpdated;

        /// <inheritdoc/>
        public List<SoldierDTO> GetSoldiers()
        {
            return _soldiers;
        }

        /// <inheritdoc/>
        public List<TrainingDTO> GetTrainings()
        {
            return _trainings;
        }

        private void UpdateLocation(object sender, ElapsedEventArgs e)
        {
            UpdateLocation();
        }

        /// <inheritdoc/>
        public void UpdateLocation()
        {
            float offset = 0.013f;
            if(rnd.NextDouble() >= 0.5)
            {
                offset = offset * -1;
            }
            foreach (var soldier in _soldiers)
            {
                soldier.Position = new PositionDTO(soldier, soldier.Position.Latitude, soldier.Position.Longitude + offset, DateTime.UtcNow);
                SoldierUpdated?.Invoke(this, soldier);
            }
            _storageService.AddPositions(_soldiers.Select(x => x.Position).ToList());
        }

        private void AddInitialLocation()
        {
            int count = 0;
            double offset = 0.0100;
            foreach (var soldier in _soldiers)
            {
                count++;
                if(count >= 25)
                {
                    count = 0;
                    offset += 0.0100;
                }
                double latitudeOffset = (count * 0.0050);

                soldier.Position = new PositionDTO(soldier, InitialPoint.Latitude + latitudeOffset, InitialPoint.Longitude + offset, DateTime.UtcNow);
            }
        }

        private void ReadInitialData()
        {
            _trainings = _storageService.GetTrainings();
            _soldiers = _storageService.GetSoldiers();

            var positions = _storageService.GetPositions();
            foreach(var position in positions)
            {
                var foundSoldier = _soldiers.FirstOrDefault(x => x.Id == position.Soldier.Id);
                if(foundSoldier != null)
                {
                    foundSoldier.Position = position;
                }
            }
        }

        /// <inheritdoc/>
        public void Initialize(int trainingCount, int soldierCount, bool clearStorage = false)
        {
            if (clearStorage)
            {
                _storageService.ClearStorage();
            }

            if (!HasInitialData())
            {
                InitializeTestTraining(trainingCount);
                InitializeTestSoldiers(soldierCount);
                AddInitialLocation();
            }
            else
            {
                ReadInitialData();
            }

        }

        /// <inheritdoc/>
        public void StartAutoUpdateLocations()
        {
            Timer timer = new Timer();
            timer.Elapsed += UpdateLocation;
            timer.Interval = 1000;
            timer.AutoReset = true;
            timer.Start();
        }

        private bool HasInitialData()
        {
            return _storageService.HasSoldiers();
        }

        private void InitializeTestSoldiers(int soldierCount)
        {
            List<SoldierDTO> soldiersToAdd = new List<SoldierDTO>();

            List<TrainingDTO> availableTrainings = _trainings;

            for (int i = 0; i < soldierCount; i++)
            {
                var soldier = new SoldierDTO()
                {
                    Id = i,
                    Name = $"Soldier {i}",
                    Rank = "Sdt",
                    Country = "CH",
                    Training = availableTrainings[rnd.Next(availableTrainings.Count)]
                };
                soldiersToAdd.Add(soldier);
            }

            _soldiers.AddRange(soldiersToAdd);
            _storageService.AddSoldiers(soldiersToAdd);
        }

        private void InitializeTestTraining(int trainingCount)
        {
            List<TrainingDTO> trainingsToAdd = new List<TrainingDTO>();

            for (int i = 0; i < trainingCount; i++)
            {
                trainingsToAdd.Add(new TrainingDTO() 
                { 
                    Id = i,
                    Name = $"Training {i}" 
                });
            }

            _trainings.AddRange(trainingsToAdd);
            _storageService.AddTrainings(trainingsToAdd);
        }

    }
}
