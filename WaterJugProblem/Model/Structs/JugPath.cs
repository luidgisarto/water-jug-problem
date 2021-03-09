using System;
using System.Collections.Generic;
using System.Text;

namespace WaterJugProblem.Model.Structs
{
    public class JugPath
    {
        int _depth;
        int _order;
        Jug _j1;
        Jug _j2;

        public int Depth => _depth;
        public int Order => _order;
        public Jug J1 => _j1;
        public Jug J2 => _j2;

        public JugPath(Jug j1, Jug j2, int depth, int order)
        {
            _j1 = (Jug)j1.Clone();
            _j2 = (Jug)j2.Clone();
            _depth = depth;
            _order = order;
        }
    }
}
