using System;
using System.Collections.Generic;
using System.Text;

namespace WaterJugProblem.Model.Structs
{
    public class JugPath
    {
        int _order;
        Jug _j1;
        Jug _j2;

        public JugPath Anterior { get; set; }
        public int Order => _order;
        public Jug J1 => _j1;
        public Jug J2 => _j2;
        public List<JugPath> Paths
        {
            get
            {
                var paths = new List<JugPath>();

                var aux = Anterior;

                while (aux != null)
                {
                    paths.Add(aux);
                    aux = aux.Anterior;
                }

                return paths;
            }
        }

        public JugPath(Jug j1, Jug j2, int order)
        {
            _j1 = (Jug)j1.Clone();
            _j2 = (Jug)j2.Clone();
            _order = order;
        }
    }
}
