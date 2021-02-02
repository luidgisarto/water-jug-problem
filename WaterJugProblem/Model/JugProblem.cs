using System;
using System.Collections.Generic;

namespace WaterJugProblem.Model
{
    public class JugProblem
    {
        readonly Dictionary<int, Func<bool>> _rules;

        public JugProblem(int m, int n)
        {
            j1 = new Jug(m);
            j2 = new Jug(n);

            _rules = new Dictionary<int, Func<bool>>()
            {
                { 1, j1.FillJug },
                { 2, j2.FillJug },
                { 3, j1.EmptyJug },
                { 4, j2.EmptyJug },
                { 5, () => TransferWater(j1, j2) },
                { 6, () => TransferWater(j2, j1) }
            };
        }

        public Jug j1 { get; set; }

        public Jug j2 { get; set; }

        public bool TransferWater(Jug current, Jug destination)
        {
            if (current.IsEmpty() || destination.IsFull())
                return false;

            var amount = Math.Min(current.Current, destination.EmptySpace);

            current.UpdateContent(-amount);
            destination.UpdateContent(amount);

            return true;
        }
    }
}
