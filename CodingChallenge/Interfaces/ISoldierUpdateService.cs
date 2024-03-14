using CodingChallenge.ViewModels;
using Contracts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge.Interfaces
{
    /// <summary>
    /// Service for decoupling the update of the soldiers from the ui
    /// This will not block the thread, which posts to this queue
    /// It allows to receive the soldiers in an order that the one added first
    /// is removed also first from the list, so that every soldier gets updated in an
    /// acceptable time, even on high load
    /// </summary>
    public interface ISoldierUpdateService
    {
        /// <summary>
        /// Post a new soldier in the queue
        /// </summary>
        /// <param name="id"></param>
        /// <param name="soldier"></param>
        void Post(int id, SoldierDTO soldier);

        /// <summary>
        /// Receive a soldier based it's id
        /// </summary>
        /// <param name="key">The id for the soldier</param>
        /// <param name="soldier">The soldier, which has this id</param>
        /// <returns>If the receive was successful or not</returns>
        bool Receive(int key, out SoldierViewModel soldier);

        /// <summary>
        /// Gets the order to receive the soldiers based on who is already the longest time in the queue
        /// </summary>
        /// <returns>A ordered list with the keys of the soldiers</returns>
        List<int> GetOrder();
    }
}
