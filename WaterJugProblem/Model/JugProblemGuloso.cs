using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterJugProblem.Model.Structs;

namespace WaterJugProblem.Model
{
    public class JugProblemGuloso
    {
        Jug _j1;
        Jug _j2;

        List<JugPath> _open;
        readonly List<JugPath> _close;
        readonly Stack<JugPath> _path;

        public JugProblemGuloso(int capacity1, int capacity2)
        {
            _j1 = new Jug(capacity1);
            _j2 = new Jug(capacity2);
            _open = new List<JugPath>();
            _close = new List<JugPath>();
            _path = new Stack<JugPath>();

            CreateNewJugPath(_j1, _j2, 0, 0);
        }

        public void CreateNewJugPath(Jug j1, Jug j2, int depth, int count)
        {
            var step1 = new JugPath(j1, j2, depth, count);

            _close.Add(step1);

            Console.WriteLine("Fechados: {0}",
                string.Join(",",
                    _close.Select(item => $"({item.J1.Current}, {item.J2.Current} [{item.Order}])")
                )
            );

            _path.Push(step1);

            Console.WriteLine("Caminho: {0}{1}",
                string.Join(",",
                    _path.OrderBy(item => item.Depth).Select(item => $"({item.J1.Current}, {item.J2.Current} [{item.Order}])")
                ),
                Environment.NewLine
            );
        }

        public void PopulateOpenList()
        {
            foreach(var rule in Jug.GetTransferRules())
            {
                var jugClone1 = (Jug)_j1.Clone();
                var jugClone2 = (Jug)_j2.Clone();

                if(rule.Value(jugClone1, jugClone2))
                {
                    if (!_path.Any(item => item.J1.Current == jugClone1.Current &&
                         item.J2.Current == jugClone2.Current)
                    )
                    {
                        _open.Add(new JugPath(jugClone1, jugClone2, _path.Count - 1, _open.Count));
                    }
                }
            }

            OrderOpenList();

            Console.WriteLine("Abertos gerados ordenados: {0}",
                string.Join(",",
                    _open.Select(item => $"({item.J1.Current}, {item.J2.Current} [{item.Order}])")
                )
            );
        }

        public void OrderOpenList()
        {
            _open = _open.OrderBy(item => Math.Abs(item.J1.Current - 4)).ToList();
        }

        public void Solve()
        {
            while(_j1.Current != 4)
            {
                PopulateOpenList();
                ApplyRule();
            }
        }

        private void ApplyRule()
        {
            var firstElement = _open.First(item => !_path.Any(
                item2 => item2.J1.Current == item.J1.Current && item2.J2.Current == item.J2.Current)
            );

            _open.Remove(firstElement);
            
            _j1 = firstElement.J1;
            _j2 = firstElement.J2;

            VerifyPath(firstElement);

            CreateNewJugPath(firstElement.J1, firstElement.J2, _path.Count, firstElement.Order);
        }

        private void VerifyPath(JugPath path)
        {
            while (path.Depth < _path.Count - 1)
            {
                _path.Pop();
            }
        }
    }
}
