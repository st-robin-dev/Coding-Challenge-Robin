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
    internal class TrainingDataModel
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TrainingId { get; set; }

        public string Name { get; set; }

        public TrainingDataModel() { }

        public TrainingDataModel(TrainingDTO training)
        {
            TrainingId = training.Id;
            Name = training.Name;
        }
    }
}
