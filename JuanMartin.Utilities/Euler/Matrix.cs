using JuanMartin.Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private void LoadMatrix(string fileName, char delimiter)
        {
            using (var reader = new StreamReader(fileName, Encoding.UTF8))
            {
                var lines = (IEnumerable<string>)reader.ReadToEnd().Split(new char[] { UtilityFile.CarriageReturn, UtilityFile.LineFeed }, StringSplitOptions.RemoveEmptyEntries);

                _matrix = lines.Select(line => line.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries).Select(v => new Number(Convert.ToInt32(v))).ToArray()).ToArray();
                _dimension = _matrix.Length;
            }
        }

        public void Transpose()
        {
            for (int i = 0; i < _dimension; i++)
            {
                for (int j = 0; j < _dimension; j++)
                {
                    if (i == j)
                        continue;

                    var temp = _matrix[i][j];
                    _matrix[i][j] = _matrix[j][i];
                    _matrix[j][i] = temp;
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
                        _matrix[i][j].Neighbors.Add(new Tuple<int, int>(i - 1, j), _matrix[i - 1][j].Value);
                    if (i < _dimension - 1)
                        _matrix[i][j].Neighbors.Add(new Tuple<int, int>(i + 1, j), _matrix[i + 1][j].Value);

                    if (j > 0)
                        _matrix[i][j].Neighbors.Add(new Tuple<int, int>(i, j - 1), _matrix[i][j - 1].Value);
                    if (j < _dimension - 1)
                        _matrix[i][j].Neighbors.Add(new Tuple<int, int>(i, j + 1), _matrix[i][j + 1].Value);

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

                if (_matrix[current_x][current_y].Visited)
                    throw new ApplicationException($"Loop triggered when adding {_matrix[current_x][current_y].Value} to path [{string.Join(",",path.ToArray())}].");

                Tuple<int, int> coordinates = null;
                var min = Int32.MaxValue;
                _matrix[current_x][current_y].Visited = true;
                foreach (var cell in _matrix[current_x][current_y].Neighbors                                                        )
                {
                    if (cell.Value < min && cell.Key != Tuple.Create(current_x,current_y))
                    {
                        coordinates = cell.Key;
                        min = cell.Value;
                    }
                }
                path.Add(min);

                previous_x = current_x;
                previous_y = current_y;
                current_x = coordinates.Item1;
                current_y = coordinates.Item2;
            }
            return path;
        }
    }
}
