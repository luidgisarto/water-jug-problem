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

            if(key == 3)
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
