using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using WaterJugProblem.Model.Structs;

namespace WaterJugProblem.Model
{
    public class JugProblemGuloso
    {
        Jug _j1;
        Jug _j2;
        JugPath _currentPath;

        List<JugPath> _open;
        readonly List<JugPath> _close;

        public JugProblemGuloso(int capacity1, int capacity2)
        {
            _j1 = new Jug(capacity1);
            _j2 = new Jug(capacity2);
            _open = new List<JugPath>();
            _close = new List<JugPath>();

            CreateNewJugPath(_j1, _j2, 0);
        }

        public void CreateNewJugPath(Jug j1, Jug j2, int count)
        {
            _currentPath = new JugPath(j1, j2, count);
          
            _close.Add(_currentPath);

            Console.WriteLine("Fechados: {0}",
                string.Join(",",
                    _close.Select(item => $"({item.J1.Current}, {item.J2.Current} [{item.Order}])")
                )
            );
        }

        public void PopulateOpenList()
        {
            foreach(var rule in Jug.GetTransferRules())
            {
                JugPath jugPath;
                var jugClone1 = (Jug)_currentPath.J1.Clone();
                var jugClone2 = (Jug)_currentPath.J2.Clone();

                if(rule.Value(jugClone1, jugClone2))
                {
                    jugPath = new JugPath(jugClone1, jugClone2, _open.Count)
                    {
                        Anterior = _currentPath
                    };

                    if (!jugPath.Paths.Any(item => item.J1.Current == jugClone1.Current &&
                         item.J2.Current == jugClone2.Current)
                    )
                    {
                        _open.Add(jugPath);
                    }
                }
            }

            OrderOpenList();
        }

        public void OrderOpenList()
        {
            _open = _open.OrderBy(item => Math.Abs(item.J1.Current - 4)).ToList();
        }

        public string Solve()
        {
            while (_currentPath.J1.Current != 4)
            {
                PopulateOpenList();
                ApplyRule();
            }

            return GetJson();
        }

        private string GetJson()
        {
            var dictionaryJson = new Dictionary<int, JsonNode>();

            _open.ForEach(item =>
            {
                if (!dictionaryJson.ContainsKey(item.GetHashCode()))
                {
                    dictionaryJson.Add(item.GetHashCode(), new JsonNode()
                    {
                        HashCodeNode = item.GetHashCode(),
                        Color = "yellow"
                    });
                }
            });

            _close.ForEach(item =>
            {
                if (!dictionaryJson.ContainsKey(item.GetHashCode()))
                {
                    dictionaryJson.Add(item.GetHashCode(), new JsonNode()
                    {
                        HashCodeNode = item.GetHashCode(),
                        Color = "blue"
                    });
                }
            });

            _close.ForEach(item =>
            {
                var pai = item?.Paths.FirstOrDefault();
                var filho = item;

                if (pai != null)
                {
                    dictionaryJson[pai.GetHashCode()].HashCodeChildren.Add(dictionaryJson[filho.GetHashCode()]);
                }
            });

            _open.ForEach(item =>
            {
                var pai = item?.Paths.FirstOrDefault();
                var filho = item;

                if (pai != null)
                {
                    dictionaryJson[pai.GetHashCode()].HashCodeChildren.Add(dictionaryJson[filho.GetHashCode()]);
                }
            });

            _currentPath.Paths.ForEach(item =>
            {
                dictionaryJson[item.GetHashCode()].Color = "green";
            });

            dictionaryJson[_currentPath.GetHashCode()].Color = "red";

            var rootObj = _close.Single(item => item.Paths.Count == 0);
            var root = dictionaryJson[rootObj.GetHashCode()];

            return JsonConvert.SerializeObject(root, Formatting.Indented);
        }

        private void ApplyRule()
        {
            var firstElement = _open.First(item => !item.Paths.Any(
                item2 => item2.J1.Current == item.J1.Current && item2.J2.Current == item.J2.Current)
            );

            _open.Remove(firstElement);
            
            _currentPath = firstElement;

            _close.Add(_currentPath);

            Console.WriteLine("Abertos gerados ordenados: {0}",
                string.Join(",",
                    _open.Select(item => $"({item.J1.Current}, {item.J2.Current} [{item.Order}])")
                )
            );

            Console.WriteLine("Fechados: {0}",
                string.Join(",",
                    _close.Select(item => $"({item.J1.Current}, {item.J2.Current} [{item.Order}])")
                )
            );

            Console.WriteLine("=======================================");
            Console.WriteLine("=> Caminho: {1}, {0}",
                string.Join(",",
                   _currentPath.Paths.Select(item => $"({item.J1.Current}, {item.J2.Current} [{item.Order}])")
                ), 
                $"({_currentPath.J1.Current}, {_currentPath.J2.Current})" 
            );
            Console.WriteLine("======================================={0}", Environment.NewLine);
        }
    }

    public class JsonNode
    {
        public int HashCodeNode { get; set; }
        public string Color { get; set; }
        public List<JsonNode> HashCodeChildren { get; set; }

        public JsonNode()
        {
            HashCodeChildren = new List<JsonNode>();
        }
    }
}
