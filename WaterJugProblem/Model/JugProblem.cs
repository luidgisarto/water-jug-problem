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

        /// <summary>
        /// Apply rule sequences to solve Jug water problem
        /// </summary>
        /// <returns>A boolean indicating wheter solution was achieved or not</returns>
        public bool Solve()
        {
            while(_currentState.j1.Current != 4)
            {
                if (!ApplyRules())
                {
                    if(!BacktrackRule())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Get the size of tree generated in solution excluding root at the count
        /// </summary>
        /// <returns>An <see cref="int"/> representing the depth</returns>
        public int GetSolutionDepth()
        {
            return _path.Count - 1;
        }

        /// <summary>
        /// Get the number of visited nodes in solution process
        /// </summary>
        /// <returns>An <see cref="int"/> representing the number</returns>
        public int GetVisitedNodesNumber()
        {
            return _visitedStates.Count - 1;
        }
        
        /// <summary>
        /// Apply rules
        /// </summary>
        /// <returns>A <see cref="bool"/> denoting if any solution was applied in current state</returns>
        bool ApplyRules()
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
        
        /// <summary>
        /// Backtrack for last state to try apply another rule.
        /// </summary>
        /// <returns>A <see cref="bool"/> denoting if backtracking was sucedeed</returns>
        bool BacktrackRule()
        {
            if (_path.Count == 0)
                return false;

            _visitedStates.Add(_currentState);
            _currentState = _path.Pop();
            J1 = (Jug)_currentState.j1.Clone();
            J2 = (Jug)_currentState.j2.Clone();
            Console.WriteLine($"BT => ({_currentState.j1.Current}, {_currentState.j2.Current})");

            return true;
        }

        /// <summary>
        /// Transfer water from current <see cref="Jug"/> to destination <see cref="Jug"/>
        /// </summary>
        /// <param name="current">The current jug</param>
        /// <param name="destination">The destination jug</param>
        /// <returns>A <see cref="bool"/> denoting if this rule was applied</returns>
        bool TransferWater(Jug current, Jug destination)
        {
            if (current.IsEmpty() || destination.IsFull())
                return false;

            var amount = Math.Min(current.Current, destination.EmptySpace);

            current.UpdateContent(-amount);
            destination.UpdateContent(amount);

            return true;
        }

        /// <summary>
        /// Enqueue current solution path
        /// </summary>
        /// <param name="ruleNumber">The number of applied rule</param>
        void EnqueuePath(int ruleNumber)
        {
            _path.Push(_currentState);
            _visitedStates.Add(_currentState);
            _currentState = new JugState(ruleNumber, J1, J2);
            Console.WriteLine($"{ruleNumber} => ({J1.Current}, {J2.Current})");
        }
        
        /// <summary>
        /// Checks if this state already happened
        /// </summary>
        /// <returns>A <see cref="bool"/> denoting if this state already happened</returns>
        bool CheckAlreadyVisited()
        {
            return _visitedStates.Any(item => item.j1.Current == J1.Current
                   && item.j2.Current == J2.Current);
        }
    }
}
