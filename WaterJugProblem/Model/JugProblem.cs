using System;
using System.Collections.Generic;
using System.Linq;

namespace WaterJugProblem.Model
{
    public class JugProblem
    {
        readonly Stack<JugState> _path = new Stack<JugState>();
        readonly Stack<Tuple<Jug, Jug>> _previousState = new Stack<Tuple<Jug, Jug>>();
        readonly Dictionary<int, Func<Jug, Jug, bool>> _rules;

        JugState _currentState;

        public JugProblem(int m, int n)
        {
            j1 = new Jug(m);
            j2 = new Jug(n);

            _rules = new Dictionary<int, Func<Jug, Jug, bool>>
            {
                { 1, (current, destination) => current.FillJug() },
                { 2, (current, destination) => destination.FillJug() },
                { 3, (current, destination) => current.EmptyJug() },
                { 4, (current, destination) => destination.EmptyJug() },
                { 5, (current, destination) => TransferWater(current, destination) },
                { 6, (current, destination) => TransferWater(destination, current) }
            };

            EnqueuePath(0);
        }

        public void EnqueuePath(int ruleNumber)
        {
            _currentState = new JugState(ruleNumber, j1, j2);
            _path.Push(_currentState);
            Console.WriteLine($"{ruleNumber} => ({j1.Current}, {j2.Current})");
        }

        public Jug j1 { get; set; }

        public Jug j2 { get; set; }

        public bool Solve()
        {
            while(j2.Current != 4)
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
                _previousState.Push(Tuple.Create((Jug)j1.Clone(), (Jug)j2.Clone()));

                var a = rule.Value(j1, j2);
                var b = StateAlreadyExists();
                hasSuccess = a && !b;
                
                if (hasSuccess)
                {
                    EnqueuePath(rule.Key);
                    break;
                }
                else
                {
                    var previous = _previousState.Pop();
                    j1 = previous.Item1;
                    j2 = previous.Item2;
                }
            }

            return hasSuccess;
        }

        private bool StateAlreadyExists()
        {
            return _path.Any(item => item.j1.Current == j1.Current 
                && item.j2.Current == j2.Current);
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
