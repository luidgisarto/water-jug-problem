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
        int[] _rulesOrder = new int[6] { 1, 2, 3, 4, 5, 6 };

        readonly List<JugState> _visitedStates = new List<JugState>();
        readonly Stack<JugState> _path = new Stack<JugState>();
        readonly Dictionary<int, Func<Jug, Jug, bool>> _rules;
        static readonly Dictionary<int, string> _rulesInfo = new Dictionary<int, string>
        {
            { 0, "R0 - Estado Inicial"},
            { 1, "R1 - Encher Jarro Maior"},
            { 2, "R2 - Encher Jarro Menor"},
            { 3, "R3 - Esvaziar Jarro Maior"},
            { 4, "R4 - Esvaziar Jarro Menor"},
            { 5, "R5 - Transferir água do Jarro Maior para o Jarro Menor"},
            { 6, "R6 - Transferir água do Jarro Menor para o Jarro Maior"}
        };

        public JugProblem(int m, int n)
        {
            J1 = new Jug(m);
            J2 = new Jug(n);

            SetSort(SortType.Asc);

            _rules = Jug.GetTransferRules();

            _currentState = new JugState(0, J1, J2);
        }

        public void SetSort(SortType sortType = SortType.Asc, int[] sortOrder = null)
        {
            if (sortType == SortType.Asc)
            {
                _rulesOrder = _rulesOrder.OrderBy(x => x).ToArray();
            }
            else if (sortType == SortType.Desc)
            {
                _rulesOrder = _rulesOrder.OrderByDescending(x => x).ToArray();
            }
            else if (sortType == SortType.Random)
            {
                var random = new Random();
                random.Shuffle(_rulesOrder);
            }
            else if(sortType == SortType.User)
            {
                _rulesOrder = sortOrder;
            }
        }

        public static List<string> GetRules()
        {
            return _rulesInfo
                .Skip(1)
                .Select(x => x.Value).ToList();
        }

        /// <summary>
        /// Apply rule sequences to solve Jug water problem
        /// </summary>
        /// <returns>A boolean indicating wheter solution was achieved or not</returns>
        public bool Solve()
        {
            Console.WriteLine($"\nOrdem das Regras: [R{string.Join(", R", _rulesOrder.Select(x => x))}]\n");
            
            EnqueuePath(0);

            while (_currentState.J1.Current != 4)
            {
                if (!ApplyRules())
                {
                    if (!BacktrackRule())
                    {
                        Console.WriteLine("\n[RESULTADO]: Não foi encontrada solução para o problema");
                        return false;
                    }
                }
            }

            Console.WriteLine($"\n[RESULTADO]: A solução atingiu nível {GetSolutionDepth()} e visitou {GetVisitedNodesNumber()} nós");
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
            return _visitedStates.Distinct().Count() - 1;
        }

        /// <summary>
        /// Apply rules
        /// </summary>
        /// <returns>A <see cref="bool"/> denoting if any solution was applied in current state</returns>
        bool ApplyRules()
        {
            var hasSuccess = false;

            foreach (var ruleNumber in _rulesOrder)
            {
                var rule = _rules[ruleNumber];

                var ruleWasApplied = rule(J1, J2);

                var alreadyVisited = CheckAlreadyVisited();

                hasSuccess = !alreadyVisited && ruleWasApplied;

                if (hasSuccess)
                {
                    EnqueuePath(ruleNumber);
                    break;
                }
                else
                {
                    J1 = (Jug)_currentState.J1.Clone();
                    J2 = (Jug)_currentState.J2.Clone();
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
            J1 = (Jug)_currentState.J1.Clone();
            J2 = (Jug)_currentState.J2.Clone();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"({_currentState.J1.Current}, {_currentState.J2.Current}) => BT - Backtracking");
            Console.ResetColor();

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
            Console.WriteLine($"({J1.Current}, {J2.Current}) => {_rulesInfo[ruleNumber]}");
        }

        /// <summary>
        /// Checks if this state already happened
        /// </summary>
        /// <returns>A <see cref="bool"/> denoting if this state already happened</returns>
        bool CheckAlreadyVisited()
        {
            return _visitedStates.Any(item => item.J1.Current == J1.Current
                   && item.J2.Current == J2.Current);
        }
    }
}
