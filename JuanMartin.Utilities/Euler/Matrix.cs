using JuanMartin.Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JuanMartin.Utilities.Euler
{
    public class Matrix
    {
        private int _dimension;
        private Number[][] _matrix;

        public Matrix(string fileName = "", char delimiter = '\0', int dimension = 0)
        {
            if (fileName == "")
            {
                _dimension = dimension;
                _matrix = new Number[dimension][];
                for (int i = 0; i < dimension; i++)
                    _matrix[i] = new Number[dimension];
            }
            else
            {
                LoadMatrix(fileName, delimiter);
            }
        }

        private void LoadMatrix(string file_name, char delimiter)
        {
            using (var reader = new StreamReader(file_name, Encoding.UTF8))
            {
                var lines = (IEnumerable<string>)reader.ReadToEnd().Split(new char[] { UtilityFile.CarriageReturn, UtilityFile.LineFeed }, StringSplitOptions.RemoveEmptyEntries);

                _matrix = lines.Select(line => line.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries).Select(v => new Number(Convert.ToInt32(v))).ToArray()).ToArray();
                _dimension = _matrix.Length;
            }
        }

        public void Transpose()
        {
            UnVisitAll();

            for (int i = 0; i < _dimension; i++)
            {
                for (int j = 0; j < _dimension; j++)
                {
                    if (i == j)
                        continue;
                    if (_matrix[i][j].Visited || _matrix[j][i].Visited)
                        continue;

                    var temp = _matrix[i][j];
                    _matrix[i][j] = _matrix[j][i];
                    _matrix[j][i] = temp;
                    _matrix[i][j].Visited = true;
                    _matrix[j][i].Visited = true;
                }
            }

            UnVisitAll();
        }

        public void UnVisitAll()
        {
            for (int i = 0; i < _dimension; i++)
            {
                for (int j = 0; j < _dimension; j++)
                {
                    _matrix[i][j].Visited = false;
                }
            }
        }
        public void PopulateNeighbors()
        {
            for (int i = 0; i < _dimension; i++)
            {
                for (int j = 0; j < _dimension; j++)
                {
                    if (i > 0)
                        _matrix[i][j].Neighbors.Add(new Tuple<int, int>(i - 1, j), _matrix[i - 1][j]);
                    if (i < _dimension - 1)
                        _matrix[i][j].Neighbors.Add(new Tuple<int, int>(i + 1, j), _matrix[i + 1][j]);

                    if (j > 0)
                        _matrix[i][j].Neighbors.Add(new Tuple<int, int>(i, j - 1), _matrix[i][j - 1]);
                    if (j < _dimension - 1)
                        _matrix[i][j].Neighbors.Add(new Tuple<int, int>(i, j + 1), _matrix[i][j + 1]);

                }
            }
        }

        public List<int> GetMinimalPath()
        {
            var path = new List<int>();
            int previous_x = 0;
            int previous_y = 0;
            int current_x = 0;
            int current_y = 0;

            path.Add(_matrix[current_x][current_y].Value);
            while (previous_x < _dimension && previous_y < _dimension)
            {
                if (current_x == _dimension - 1 && current_y == _dimension - 1)
                    break;

                var numeric_cell = _matrix[current_x][current_y];
                
                if (numeric_cell.Visited)
                    throw new ApplicationException($"Loop triggered when adding {numeric_cell.Value} to path [{string.Join(",",path.ToArray())}].");

                Tuple<int, int> coordinates = null;
                var min = Int32.MaxValue;

                foreach (var cell in numeric_cell.Neighbors)
                {
                    var neighbor = cell.Value;
                    if (neighbor.Value < min && !neighbor.Visited) //cell.Key != Tuple.Create(previous_x, previous_y))
                    {
                        coordinates = cell.Key;
                        min = neighbor.Value;
                    }
                }
                if (min == Int32.MaxValue)
                    throw new ApplicationException($"Error finding minimum value in [{string.Join(",", numeric_cell.Neighbors.Values.Select(n=>n.Value).ToArray())}].");

                path.Add(min);

                previous_x = current_x;
                previous_y = current_y;
                current_x = coordinates.Item1;
                current_y = coordinates.Item2;
                numeric_cell.Visited = true;
            }
            return path;
        }
    }
}
