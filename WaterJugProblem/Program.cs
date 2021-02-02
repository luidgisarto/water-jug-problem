using WaterJugProblem.Model;

namespace WaterJugProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            var m = 7;
            var n = 5;

            var problem = new JugProblem(m, n);

            problem.TransferWater(problem.j1, problem.j2);
        }
    }
}
