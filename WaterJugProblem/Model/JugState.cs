using System;
using System.Collections.Generic;
using System.Text;

namespace WaterJugProblem.Model
{
    class JugState
    {
        public JugState(int ruleNumber, Jug jug1, Jug jug2)
        {
            AppliedRule = new List<int>()
            {
                ruleNumber
            };

            j1 = (Jug)jug1.Clone();
            j2 = (Jug)jug2.Clone();
        }

        public Jug j1 { get; set; }
        public Jug j2 { get; set; }
        public List<int> AppliedRule { get; set; }
    }
}
