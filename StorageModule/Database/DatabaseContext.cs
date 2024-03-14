using StorageModule.Interfaces;
using StorageModule.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageModule.Database
{
    internal class DatabaseContext : DbContext, IDatabaseContext
    {
        public DbSet<SoldierDataModel> Soldiers { get; set; }
        public DbSet<TrainingDataModel> Trainings { get; set; }
        public DbSet<PositionDataModel> Positions { get; set; }
        public DatabaseContext(DbConnection connection, bool skipValidation) : base(connection, true)
        {
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, DbConfiguration>(useSuppliedContext: true));
            if (skipValidation)
            {
                Configuration.AutoDetectChangesEnabled = false;
                Configuration.ValidateOnSaveEnabled = false;
            }
        }

        /// <inheritdoc/>
        public void AddTrainings(List<TrainingDataModel> trainings)
        {
            Trainings.AddRange(trainings);
            SaveChanges();
        }

        /// <inheritdoc/>
        public List<TrainingDataModel> GetTrainings()
        {
            return Trainings.ToList();
        }

        /// <inheritdoc/>
        public List<SoldierDataModel> GetSoldiers()
        {
            return Soldiers.Include(x => x.TrainingDataModel).ToList();
        }

        /// <inheritdoc/>
        public List<PositionDataModel> GetPositions()
        {
            return Positions
                .GroupBy(x => x.SoldierId)
                .Select(g => g.OrderByDescending(y => y.Timestamp).FirstOrDefault())
                .Include(x => x.SoldierDataModel)
                .Include(x => x.SoldierDataModel.TrainingDataModel)
                .ToList();
        }

        /// <inheritdoc/>
        public void AddSoldiers(List<SoldierDataModel> soldiers)
        {
            Soldiers.AddRange(soldiers);
            SaveChanges();
        }

        /// <inheritdoc/>
        public void AddPositions(List<PositionDataModel> positions)
        {
            Positions.AddRange(positions);
            SaveChanges();
        }

        /// <inheritdoc/>
        public bool HasSoldiers()
        {
            return Soldiers.Any();
        }

        /// <inheritdoc/>
        public void ClearDB()
        {
            Database.ExecuteSqlCommand("TRUNCATE TABLE PositionDataModels");
            foreach(var soldier in Soldiers.ToList())
            {
                Soldiers.Remove(soldier);
            }
            foreach(var training in Trainings.ToList())
            {
                Trainings.Remove(training);
            }
            SaveChanges();
        }
    }
}
