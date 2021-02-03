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

            Console.WriteLine("\n");

            Console.WriteLine("TRABALHO DE INTELIGÊNCIA ARTIFICIAL\n ");
            Console.WriteLine("SOLUÇÃO DO PROBLEMA DE JARROS DE ÁGUA\n");
            Console.WriteLine("201276007 / DOUGLAS BAUMGRATZ DE CARVALHO");
            Console.WriteLine("201376082 / JÚLIO CESAR ROSA TRINDADE");
            Console.WriteLine("201176023 / LUIDGI SARTO LACERDA");
            Console.WriteLine("201276030 / OTÁVIO AUGUSTO FERREIRA RODRIGUES\n");

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

            var problem = new JugProblem(m, n);

            SortType sortType = SortType.Asc;

            if (key == 4)
            {
                var rules = JugProblem.GetRules();
                Console.WriteLine($"\nRegras:\n{string.Join("\n", rules)}");

                Console.WriteLine("\nInforme a ordem de aplicação das regras separadas por vírgula");
                var rulesUser = Console.ReadLine();

                var sortOrder = rulesUser.Split(",").ToList().Select(x => Convert.ToInt32(x.Replace("R", "").Trim())).ToArray();

                problem.SetSort(SortType.User, sortOrder);

                problem.Solve();
            }
            else
            {
                if (key == 1)
                {
                    sortType = SortType.Asc;
                }
                else if (key == 2)
                {
                    sortType = SortType.Desc;
                }
                else if (key == 3)
                {
                    sortType = SortType.Random;
                }

                for (int i = 0; i < 1; i++)
                {
                    problem.SetSort(sortType);

                    problem.Solve();
                }
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
