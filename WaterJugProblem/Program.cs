using System;

namespace WaterJugProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            var m = 7;
            var n = 5;

            Console.WriteLine($"Estado Inicial (0, 0)");
            Console.WriteLine($"Capacidade dos Jarros (7, 5)");
            Console.WriteLine($"Estado final (4, ?)");

            Console.WriteLine("\nRegras do Problema");

            Console.WriteLine("R1 - Encher totalmente um jarro com água do poço");
            Console.WriteLine("R2 - Esvaziar totalmente um jarro jogando a água na grama");
            Console.WriteLine("R3 - Passar água de um jarro pro outro\n\n");

            var problem = new JugProblem(m, n);

            Console.WriteLine($"1º Estado: ({problem.j1.Current}, {problem.j2.Current})");
            Console.WriteLine($"\nR1 em J1");
            problem.j1.FillJug();
            Console.WriteLine($"\n2º Estado: ({problem.j1.Current}, {problem.j2.Current})");
            Console.WriteLine("\nR3 em J1");
            problem.TransferWater(problem.j1, problem.j2);
            Console.WriteLine($"\n3º Estado: ({problem.j1.Current}, {problem.j2.Current})");
            Console.WriteLine("\nR3 em J1");
            problem.j1.EmptyJug();
            Console.WriteLine($"\n4º Estado: ({problem.j1.Current}, {problem.j2.Current})");
            Console.WriteLine("\nR3 em J1");
            problem.TransferWater(problem.j1, problem.j2);
            Console.WriteLine($"\n5º Estado: ({problem.j1.Current}, {problem.j2.Current})");

        }
    }

    public class JugProblem
    {
        public JugProblem(int m, int n)
        {
            j1 = new Jug(m);
            j2 = new Jug(n);
        }

        public Jug j1 { get; set; }

        public Jug j2 { get; set; }

        public void TransferWater(Jug x, Jug y)
        {
            if(y.Free >= x.Current)
            {
                y.Current += x.Current;
                y.Free = y.Capacity - y.Current;
            }
        }
    }

    public class Jug
    {
        public Jug(int capacity)
        {
            Capacity = capacity;
            Free = Capacity;
        }

        public int Free { get; set; }

        public int Capacity { get; set; }

        public int Current { get; set; }

        public void FillJug()
        {
            Current = Capacity - Current;
            Free = 0;
        }

        public void EmptyJug()
        {
            Current = 0;
            Free = Capacity;
        }
    }
}
