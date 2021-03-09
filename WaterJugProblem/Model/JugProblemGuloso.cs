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
            _path.Push(step1);
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
                OrderOpenList();

                Console.WriteLine("Abertos: {0}", 
                    string.Join(",", 
                        _open.Select(item => $"({item.J1.Current}, {item.J2.Current})")
                    )
                );

                ApplyRule();

                Console.WriteLine("Fechados: {0}",
                    string.Join(",",
                        _close.Select(item => $"({item.J1.Current}, {item.J2.Current})")
                    )
                );

                Console.WriteLine("Caminho: {0}{1}",
                    string.Join(",",
                        _path.Select(item => $"({item.J1.Current}, {item.J2.Current})")
                    ), 
                    Environment.NewLine
                );
            }
        }

        private void ApplyRule()
        {
            var firsElement = _open.ElementAt(0);
            _open.RemoveAt(0);

            _j1 = firsElement.J1;
            _j2 = firsElement.J2;

            VerifyPath(firsElement);

            CreateNewJugPath(firsElement.J1, firsElement.J2, _path.Count, 0);
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
