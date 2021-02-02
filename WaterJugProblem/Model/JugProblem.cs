using System;
using System.Collections.Generic;
using System.Linq;

namespace WaterJugProblem.Model
{
    public class JugProblem
    {
        Jug J2;
        Jug J1;
        JugState _currentState;
     
        readonly List<JugState> _visitedStates = new List<JugState>();
        readonly Stack<JugState> _path = new Stack<JugState>();
        readonly Dictionary<int, Func<Jug, Jug, bool>> _rules;

        public JugProblem(int m, int n)
        {
            J1 = new Jug(m);
            J2 = new Jug(n);

            _rules = new Dictionary<int, Func<Jug, Jug, bool>>
            {
                { 4, (current, destination) => destination.EmptyJug() },
                { 3, (current, destination) => current.EmptyJug() },
                { 6, (current, destination) => TransferWater(destination, current) },
                { 2, (current, destination) => destination.FillJug() },
                { 1, (current, destination) => current.FillJug() },
                { 5, (current, destination) => TransferWater(current, destination) },
            };

            _currentState = new JugState(0, J1, J2);

            EnqueuePath(0);
        }

        public void EnqueuePath(int ruleNumber)
        {
            _path.Push(_currentState);
            _visitedStates.Add(_currentState);
            _currentState = new JugState(ruleNumber, J1, J2);
            Console.WriteLine($"{ruleNumber} => ({J1.Current}, {J2.Current})");
        }

        public bool Solve()
        {
            while(_currentState.j1.Current != 4)
            {
                if (!ApplyRule())
                    BacktrackRule();
            }
            return true;
        }

        private void BacktrackRule()
        {
            _visitedStates.Add(_currentState);
            _currentState = _path.Pop();
            J1 = (Jug)_currentState.j1.Clone();
            J2 = (Jug)_currentState.j2.Clone();
            Console.WriteLine($"BT => ({_currentState.j1.Current}, {_currentState.j2.Current})");
        }

        public bool ApplyRule()
        {
            var hasSuccess = false;

            foreach(var rule in _rules)
            {
                var ruleWasApplied = rule.Value(J1, J2);

                var alreadyVisited = CheckAlreadyVisited();

                hasSuccess = !alreadyVisited && ruleWasApplied;

                if (hasSuccess)
                {
                    EnqueuePath(rule.Key);
                    break;   
                }
                else
                {
                    J1 = (Jug)_currentState.j1.Clone();
                    J2 = (Jug)_currentState.j2.Clone();
                }
            }

            return hasSuccess;
        }

        public bool CheckAlreadyVisited()
        {
            return _visitedStates.Any(item => item.j1.Current == J1.Current
                   && item.j2.Current == J2.Current);
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
