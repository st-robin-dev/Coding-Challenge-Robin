using CodingChallenge.Interfaces;
using CodingChallenge.Services;
using Contracts.Model;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Xpf.Map;
using SoldierSimulationModule.Interfaces;
using SoldierSimulationModule.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;

namespace CodingChallenge.ViewModels
{
    public class MainViewModel
    {
        private const int TRAINING_AMOUNT = 5;
        private const int SOLDIERS_AMOUNT = 500;
        private const bool CLEAR_STORAGE = false;

        ISoldierSimulationService _soldierSimulationService;
        ISoldierUpdateService _soldierUpdateService;

        public MainViewModel() 
        {
            _soldierUpdateService = new SoldierUpdateService();

            _soldierSimulationService = new SoldierSimulationService();
            _soldierSimulationService.SoldierUpdated += Soldier_Updated;

            Center = new GeoPoint(_soldierSimulationService.InitialPoint.Latitude, _soldierSimulationService.InitialPoint.Longitude);

            InitializeSoldiers();

            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(300);
            dt.Tick += DispatcherTimer_Ticked;
            dt.Start();
        }

        private void InitializeSoldiers()
        {
            _soldierSimulationService.Initialize(TRAINING_AMOUNT, SOLDIERS_AMOUNT, CLEAR_STORAGE);

            foreach(var soldier in _soldierSimulationService.GetSoldiers())
            {
                Soldiers.Add(new SoldierViewModel(soldier));
            }

            _soldierSimulationService.StartAutoUpdateLocations();
        }

        public ObservableCollection<SoldierViewModel> Soldiers { get; set; } = new ObservableCollection<SoldierViewModel>();

        private SoldierViewModel _selectedSoldier;
        public SoldierViewModel SelectedSoldier
        {
            get => _selectedSoldier; 
            set
            {
                SetHighlightColor(value, _selectedSoldier);
                _selectedSoldier = value;

            }
        }

        private void SetHighlightColor(SoldierViewModel selected, SoldierViewModel previousSelected)
        {
            if(selected != null) selected.Brush = new SolidColorBrush(Colors.DarkBlue);
            if(previousSelected != null) previousSelected.Brush = new SolidColorBrush(Colors.LightGreen);
        }

        public GeoPoint Center { get; set; }


        private void Soldier_Updated(object sender, SoldierDTO soldier)
        {
            _soldierUpdateService.Post(soldier.Id, soldier);
        }

        private void DispatcherTimer_Ticked(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var keys = _soldierUpdateService.GetOrder();
            foreach (var key in keys)
            {
                if (stopwatch.ElapsedMilliseconds > 30) break;
                if (_soldierUpdateService.Receive(key, out var soldier))
                {
                    var found = Soldiers.FirstOrDefault(x => x.Soldier.Id == soldier.Soldier.Id);
                    if (found != null)
                    {
                        found.Position = soldier.Position;
                    }
                    else
                    {
                        Soldiers.Add(soldier);
                    }
                }
            }
            stopwatch.Stop();
        }
    }
}
