using Contracts.Model;
using DevExpress.Mvvm;
using DevExpress.Xpf.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CodingChallenge.ViewModels
{
    public class SoldierViewModel : BindableBase
    {
        public SoldierViewModel(SoldierDTO soldier)
        {
            this.Soldier = soldier;
            UpdateTime = DateTime.UtcNow;
            Position = new GeoPoint(soldier.Position.Latitude, soldier.Position.Longitude);
            Brush = new SolidColorBrush(Colors.LightGreen);
        }

        public SoldierDTO Soldier { get; set; }
        public DateTime UpdateTime { get; set; }

        public SolidColorBrush Brush
        {
            get { return GetValue<SolidColorBrush>(nameof(Brush)); }
            set { SetValue(value, nameof(Brush)); }
        }

        public GeoPoint Position
        {
            get { return GetValue<GeoPoint>(nameof(Position)); }
            set { SetValue(value, nameof(Position)); }
        }
    }
}
