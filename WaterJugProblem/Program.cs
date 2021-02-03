using System;
using System.Linq;
using WaterJugProblem.Model;

namespace WaterJugProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            var m = 7;
            var n = 5;

            var key = -1;

            while (key == -1 || key > 4 || key < 1)
            {
                Console.WriteLine("Informe o critério de escolha das regras\n");

                Console.WriteLine("1 - Crescente");
                Console.WriteLine("2 - Decrescente");
                Console.WriteLine("3 - Randômico");
                Console.WriteLine("4 - Informado pelo usuário");

                var inputKey = Console.ReadLine();

                key = Convert.ToInt32(inputKey);
            }

            var sortType = SortType.Asc;
            int[] userSort = null;

            switch (key)
            {
                case 1:
                {
                    sortType = SortType.Asc;
                } break;
                case 2:
                {
                    sortType = SortType.Desc;
                } break;
                case 3:
                {
                    sortType = SortType.Random;
                } break;
                case 4:
                {
                    sortType = SortType.User;
                    var rules = JugProblem.GetRules();

                    Console.WriteLine($"\nRegras:\n{string.Join("\n", rules)}");
                    Console.WriteLine("\nInforme a ordem de aplicação das regras separadas por vírgula");

                    var rulesUser = Console.ReadLine();

                    userSort = rulesUser.Split(",").Select(x => Convert.ToInt32(x.Replace("R", "").Trim())).ToArray();
                } break;
            }

            if(key >= 1 && key <= 3)
            {
                for (int i = 0; i < 50; i++)
                {
                    var problem = new JugProblem(m, n);

                    problem.SetSort(sortType);

                    problem.Solve();
                }
            }
            else
            {
                var problem = new JugProblem(m, n);

                problem.SetSort(sortType, userSort);

                problem.Solve();
            }
        } 
    }

    public enum SortType
    {
        Asc = 1,
        Desc = 2,
        Random = 3,
        User = 4
    }
}
