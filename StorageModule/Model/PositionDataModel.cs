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
    internal class PositionDataModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }

        [ForeignKey("SoldierDataModel")]
        public int SoldierId { get; set; }
        public SoldierDataModel SoldierDataModel { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }

        public PositionDataModel() { }

        public PositionDataModel(PositionDTO position)
        {
            SoldierId = position.Soldier.Id;
            Latitude = position.Latitude;
            Longitude = position.Longitude;
            Timestamp = position.Timestamp;
        }
    }
}
