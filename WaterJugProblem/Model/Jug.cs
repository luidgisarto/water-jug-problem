using System;
using System.Collections.Generic;

namespace WaterJugProblem.Model
{
    public class Jug : ICloneable
    {
        public Jug(int capacity)
        {
            Capacity = capacity;
            Current = 0;
        }

        public int Capacity { get; private set; }

        public int Current { get; private set; }

        public int EmptySpace
        {
            get
            {
                return Capacity - Current;
            }
        }

        public void UpdateContent(int amount)
        {
            Current += amount;
        }

        public bool IsEmpty()
        {
            return Current == 0;
        }

        public bool IsFull()
        {
            return Current == Capacity;
        }

        public bool FillJug()
        {
            if (EmptySpace > 0)
            {
                Current = Capacity;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Transfer water from current <see cref="Jug"/> to destination <see cref="Jug"/>
        /// </summary>
        /// <param name="current">The current jug</param>
        /// <param name="destination">The destination jug</param>
        /// <returns>A <see cref="bool"/> denoting if this rule was applied</returns>
        public bool EmptyJug()
        {
            if (Current > 0)
            {
                Current = 0;
                return true;
            }

            return false;
        }

        public static bool TransferWater(Jug current, Jug destination)
        {
            if (current.IsEmpty() || destination.IsFull())
                return false;

            var amount = Math.Min(current.Current, destination.EmptySpace);

            current.UpdateContent(-amount);
            destination.UpdateContent(amount);

            return true;
        }

        public static Dictionary<int, Func<Jug, Jug, bool>> GetTransferRules(){

            return new Dictionary<int, Func<Jug, Jug, bool>>
            {
                { 1, (current, destination) => current.FillJug() },
                { 2, (current, destination) => destination.FillJug() },
                { 3, (current, destination) => current.EmptyJug() },
                { 4, (current, destination) => destination.EmptyJug() },
                { 5, (current, destination) => TransferWater(current, destination) },
                { 6, (current, destination) => TransferWater(destination, current) },
            };
        }

        public object Clone()
        {
            var newJug = new Jug(Capacity);
            newJug.UpdateContent(Current);
            return newJug;
        }
    }
}
