using Contracts.Model;
using StorageModule.Database;
using StorageModule.Interfaces;
using StorageModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageModule.Services
{
    /// <inheritdoc/>
    public class StorageService : IStorageService
    {
        public StorageService() { }

        /// <inheritdoc/>
        public void AddPositions(List<PositionDTO> positions)
        {
            using (var context = new DatabaseFactory().Build(skipValidation: true))
            {
                context.AddPositions(positions.Select(x => new PositionDataModel(x)).ToList());
            }
        }

        /// <inheritdoc/>
        public void AddSoldiers(List<SoldierDTO> soldiers)
        {
            using (var context = new DatabaseFactory().Build())
            {
                context.AddSoldiers(soldiers.Select(x => new SoldierDataModel(x)).ToList());
            }
        }

        /// <inheritdoc/>
        public void AddTrainings(List<TrainingDTO> trainings)
        {
            using (var context = new DatabaseFactory().Build())
            {
                context.AddTrainings(trainings.Select(x => new TrainingDataModel(x)).ToList());
            }
        }

        /// <inheritdoc/>
        public List<SoldierDTO> GetSoldiers()
        {
            List<SoldierDTO> soldiers = new List<SoldierDTO>();
            using (var context = new DatabaseFactory().Build())
            {
                soldiers = context.GetSoldiers().Select(x => ConvertSoldier(x)).ToList();
            }
            return soldiers;
        }

        /// <inheritdoc/>
        public List<TrainingDTO> GetTrainings()
        {
            List<TrainingDTO> trainings = new List<TrainingDTO>();
            using (var context = new DatabaseFactory().Build())
            {
                trainings = context.GetTrainings().Select(x => ConvertTraining(x)).ToList();
            }
            return trainings;
        }

        /// <inheritdoc/>
        public List<PositionDTO> GetPositions()
        {
            List<PositionDTO> trainings = new List<PositionDTO>();
            using (var context = new DatabaseFactory().Build())
            {
                trainings = context.GetPositions().Select(x => ConvertPosition(x)).ToList();
            }
            return trainings;
        }


        /// <inheritdoc/>
        public bool HasSoldiers()
        {
            bool hasSoldiers = false;
            using (var context = new DatabaseFactory().Build())
            {
                hasSoldiers = context.HasSoldiers();
            }
            return hasSoldiers;
        }

        /// <inheritdoc/>
        public void ClearStorage()
        {
            using (var context = new DatabaseFactory().Build())
            {
                context.ClearDB();
            }
        }

        private SoldierDTO ConvertSoldier(SoldierDataModel dataModel)
        {
            return new SoldierDTO()
            {
                Id = dataModel.SoldierId,
                Name = dataModel.Name,
                Country = dataModel.Country,
                Rank = dataModel.Rank,
                Training = ConvertTraining(dataModel.TrainingDataModel)
            };
        }

        private TrainingDTO ConvertTraining(TrainingDataModel dataModel)
        {
            return new TrainingDTO()
            {
                Name = dataModel.Name,
                Id = dataModel.TrainingId
            };
        }

        private PositionDTO ConvertPosition(PositionDataModel dataModel)
        {
            return new PositionDTO(
                ConvertSoldier(dataModel.SoldierDataModel), 
                dataModel.Latitude, 
                dataModel.Longitude,
                dataModel.Timestamp
            );
        }
    }
}
