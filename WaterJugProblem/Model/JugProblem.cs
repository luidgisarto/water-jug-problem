using System;
using System.Collections.Generic;
using System.Linq;

namespace WaterJugProblem.Model
{
    public class JugProblem
    {
        readonly List<JugState> _visitedStates = new List<JugState>();
        readonly Stack<JugState> _path = new Stack<JugState>();
        readonly Dictionary<int, Func<Jug, Jug, bool>> _rules;

        JugState _currentState;

        public JugProblem(int m, int n)
        {
            j1 = new Jug(m);
            j2 = new Jug(n);

            _rules = new Dictionary<int, Func<Jug, Jug, bool>>
            {
                { 4, (current, destination) => destination.EmptyJug() },
                { 3, (current, destination) => current.EmptyJug() },
                { 6, (current, destination) => TransferWater(destination, current) },
                { 2, (current, destination) => destination.FillJug() },
                { 1, (current, destination) => current.FillJug() },
                { 5, (current, destination) => TransferWater(current, destination) },
            };

            EnqueuePath(0);
        }

        public void EnqueuePath(int ruleNumber)
        {
            _currentState = new JugState(ruleNumber, j1, j2);
            _path.Push(_currentState);
            _visitedStates.Add(_currentState);
            Console.WriteLine($"{ruleNumber} => ({j1.Current}, {j2.Current})");
        }

        public Jug j1 { get; set; }

        public Jug j2 { get; set; }

        public bool Solve()
        {
            while(j1.Current != 4)
            {
                if (!ApplyRule())
                    BacktrackRule();
            }
            return true;
        }

        private void BacktrackRule()
        {
            _currentState = _path.Pop();
        }

        public bool ApplyRule()
        {
            var hasSuccess = false;

            foreach(var rule in _rules)
            {

                var j1ant = (Jug)j1.Clone();
                var j2ant = (Jug)j2.Clone();

                var hasSucces = rule.Value(j1, j2);

                var a = !_visitedStates.Any(item => item.j1.Current == j1.Current
                    && item.j2.Current == j2.Current);

                //var b = !_currentState.AppliedRule.Contains(rule.Key);

                hasSuccess = a && hasSucces;

                if (hasSuccess)
                {
                    EnqueuePath(rule.Key);
                    break;   
                }
                else
                {
                    j1 = j1ant;
                    j2 = j2ant;
                }
            }

            return hasSuccess;
        }

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
