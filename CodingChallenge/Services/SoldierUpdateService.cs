using CodingChallenge.Interfaces;
using CodingChallenge.ViewModels;
using Contracts.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge.Services
{
    /// <inheritdoc/>
    public class SoldierUpdateService : ISoldierUpdateService
    {
        private ConcurrentDictionary<int, SoldierViewModel> _soldiers = new ConcurrentDictionary<int, SoldierViewModel>();

        /// <inheritdoc/>
        public void Post(int id, SoldierDTO soldier)
        {
            _soldiers.AddOrUpdate(id, new SoldierViewModel(soldier), (key, oldValue) =>
            {
                oldValue.Soldier = soldier;
                return oldValue;
            });
        }

        /// <inheritdoc/>
        public List<int> GetOrder()
        {
            return _soldiers.ToArray().OrderBy(x => x.Value.UpdateTime).Select(x => x.Key).ToList();
        }

        /// <inheritdoc/>
        public bool Receive(int key, out SoldierViewModel soldier)
        {
            soldier = null;
            if (_soldiers.Count != 0)
            {
                return _soldiers.TryRemove(key, out soldier);
            }
            return false;
        }
    }
}
