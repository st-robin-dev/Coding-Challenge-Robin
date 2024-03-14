using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Model;

namespace StorageModule.Model
{
    internal class SoldierDataModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 0)]
        public int SoldierId { get; set; }

        public string Name { get; set; }

        public string Rank { get; set; }

        public string Country { get; set; }

        [ForeignKey("TrainingDataModel")]
        public int TrainingId { get; set; }

        public TrainingDataModel TrainingDataModel { get; set; }

        public SoldierDataModel() { }

        public SoldierDataModel(SoldierDTO soldier)
        {
            SoldierId = soldier.Id;
            Name = soldier.Name;
            Rank = soldier.Rank;
            Country = soldier.Country;
            TrainingId = soldier.Training.Id;

        }
    }
}
