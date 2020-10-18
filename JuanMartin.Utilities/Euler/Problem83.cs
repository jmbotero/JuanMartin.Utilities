using JuanMartin.Kernel.Utilities;
using JuanMartin.Kernel.Utilities.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuanMartin.Utilities.Euler
{
    public class Problem83
    {
        private int _dimension;
        private Vertex<int>[][] _matrix;
        public DirectedAcyclicGraph<int> Graph { get; private set; }

        public Problem83(string file_name, char delimiter)
        {
            Graph = new DirectedAcyclicGraph<int>();
            LoadGraph(file_name, delimiter);
            PopulateRelationships();
        }

        private void LoadGraph(string file_name, char delimiter)
        {
            using (var reader = new StreamReader(file_name, Encoding.UTF8))
            {
                var lines = (IEnumerable<string>)reader.ReadToEnd().Split(new char[] { UtilityFile.CarriageReturn, UtilityFile.LineFeed }, StringSplitOptions.RemoveEmptyEntries);

                _matrix = lines.Select(line => line.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries).Select(v => new Vertex<int>(Convert.ToInt32(v))).ToArray()).ToArray();
                _dimension = _matrix.Length;
            }
            for (int i = 0; i < _dimension; i++)
            {
                for (int j = 0; j < _dimension; j++)
                {
                    Graph.AddVertex(_matrix[i][j]);
                }
            }
        }

        private void PopulateRelationships()
        {
            for (int i = 0; i < _dimension; i++)
            {
                for (int j = 0; j < _dimension; j++)
                {
                    var current = _matrix[i][j];

                    if (i > 0)
                        Graph.AddEdge(current, _matrix[i - 1][j], Edge<int>.EdgeDirection.bidirectional, "left", 1);
                    if (i < _dimension - 1)
                        Graph.AddEdge(current, _matrix[i + 1][j], Edge<int>.EdgeDirection.bidirectional, "right", 1);

                    if (j > 0)
                        Graph.AddEdge(current, _matrix[i][j - 1], Edge<int>.EdgeDirection.bidirectional, "top", 1);
                    if (j < _dimension - 1)
                        Graph.AddEdge(current, _matrix[i][j + 1], Edge<int>.EdgeDirection.bidirectional, "bottom", 1);

                }
            }
        }
    }
}