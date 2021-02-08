using System.Collections.Generic;

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

            J1 = (Jug)jug1.Clone();
            J2 = (Jug)jug2.Clone();
        }

        public Jug J1 { get; set; }
        public Jug J2 { get; set; }
        public List<int> AppliedRule { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is JugState)
            {
                var compare = (JugState)obj;
                return compare.J1.Current == J1.Current && compare.J2.Current == J2.Current;
            }
            return false;
        }
    }
}
