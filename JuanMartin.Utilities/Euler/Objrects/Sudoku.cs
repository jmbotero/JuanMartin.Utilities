using JuanMartin.Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JuanMartin.Utilities.Euler
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "}")]
    public class Sudoku
    {
        public string Boardname;
        public int Changes;
        public int EmptyCellCount;

        private Dictionary<Tuple<int, int>, Tuple<int, int>> Coordinates;

        public int GridHeight;
        public int BlockHeight;
        public List<List<int>> Rows;
        public List<List<int>> Columns;
        public List<List<int>> Blocks;

        public Sudoku(string name, List<List<int>> rows = null)
        {

            if (rows != null)
            {
                GridHeight = rows.Count;
                var x = Convert.ToInt32(Math.Sqrt(GridHeight));
                if (x * x == GridHeight)
                    BlockHeight = x;
                else
                    BlockHeight = GridHeight;
            }
            else
            {
                GridHeight = 9;
                BlockHeight = 3;
            }
            Boardname = name;
            Rows = rows;

            //transpose
            Columns = new List<List<int>>();
            for (int i = 0; i < GridHeight; i++)
            {
                var col = new List<int>();
                col = UtilityList.ListRepeatedElements<int>(0, GridHeight);
                Columns.Add(col);
            }

            for (int i = 0; i < GridHeight; i++)
            {
                for (int j = 0; j < GridHeight; j++)
                {
                    Columns[j][i] = Rows[i][j];
                }
            }
            //calculate coordinates
            SetBlockToCellCordinateMapping();

            //set blocks
            SetBlocks();

        }

        public bool Solved => GetEmptyCells().Count == 0;

        public int CheckSum
        {
            get
            {
                var sum = 0;

                if (Solved)
                {
                    for (int i = 2; i >= 0; i--)
                        sum += Rows[0][i] * (int)Math.Pow(10, 2 - i);
                }
                else
                    return -1;
                return sum;

            }
        }

        public void WriteLineBoard()
        {
            var color = ConsoleColor.White;
            var matrix = "";
            var builder = new System.Text.StringBuilder();

            for (int i = 0; i < GridHeight; i++)
            {
                builder.Append("\t");
                for (int j = 0; j < GridHeight; j++)
                {
                    builder.Append(Rows[i][j] + " ");
                    if ((j + 1) % BlockHeight == 0)
                        builder.Append(" ");
                }
                builder.Append("\n");
                if ((i + 1) % BlockHeight == 0)
                    builder.Append("\n");
            }
            matrix = builder.ToString();

            if (Solved)
                color = ConsoleColor.Green;
            else
                color = ConsoleColor.Red;

            Console.ForegroundColor = color;
            Console.WriteLine(Boardname);
            Console.WriteLine(matrix);

            Console.ForegroundColor = ConsoleColor.White;
        }

        public bool Solve(bool verbose=false)
        {
            var found_soulion = BackTrack(GetEmptyCells());
            
            if (verbose) WriteLineBoard(); 
            
            return found_soulion;
        }

        public bool SetGridValue(int i, int j, int value, bool overwrite = false, bool block_based = false)
        {
            var update = false;
            var block_i = -1;
            var block_j = -1;

            var cell_coordinates = new Tuple<int, int>(i, j);
            var block_coordinates = Coordinates[cell_coordinates];

            if (block_based)
            {
                block_i = i;
                block_j = j;
                i = block_coordinates.Item1;
                j = block_coordinates.Item2;
            }
            else
            {
                block_i = block_coordinates.Item1;
                block_j = block_coordinates.Item2;
            }

            if ((Rows[i][j] == 0 & Rows[i][j] != value) | overwrite)
            {
                Rows[i][j] = value;
                Columns[j][i] = value;
                Blocks[block_i][block_j] = value;
                update = true;
                Changes += 1;
            }

            return update;
        }
        public bool CheckGridValue(int i, int j, int value)
        {
            var row_contains = Rows[i].Contains(value);
            var column_contains = Columns[j].Contains(value);

            if (BlockHeight != GridHeight)
            {
                var cell_coordinates = new Tuple<int, int>(i, j);

                if (Coordinates.ContainsKey(cell_coordinates))
                {
                    var block_coordinates = Coordinates[cell_coordinates];
                    var block_contains = Blocks[block_coordinates.Item1].Contains(value);
                    return row_contains | column_contains | block_contains;
                }
                else
                    return false;
            }
            else
            {
                return row_contains | column_contains;
            }


        }

        private List<Tuple<int, int>> GetEmptyCells()
        {
            var spots = new List<Tuple<int, int>>();

            for (int i = 0; i < GridHeight; i++)
            {
                for (int j = 0; j < GridHeight; j++)
                {
                    if (Rows[i][j] == 0)
                        spots.Add(new Tuple<int, int>(i, j));
                }
            }

            return spots;
        }

        private void SetBlocks()
        {
            var count = -1;
            // create empty block lists first
            Blocks = new List<List<int>>();

            // first check if there are blocks
            if (BlockHeight != GridHeight)
            {
                count = GridHeight;
            }
            else
            {
                count = GridHeight * GridHeight;
            }
            if (count != -1)
            {
                for (int i = 0; i < GridHeight; i++)
                {
                    var block = new List<int>();
                    block = UtilityList.ListRepeatedElements<int>(0, count);
                    Blocks.Add(block);
                }
            }
            // second populate block list
            foreach (KeyValuePair<Tuple<int, int>, Tuple<int, int>> entry in Coordinates)
            {
                var cellCoordinates = entry.Key;
                var blockCoordinates = entry.Value;

                var value = Rows[cellCoordinates.Item1][cellCoordinates.Item2];

                Blocks[blockCoordinates.Item1][blockCoordinates.Item2] = value;
            }
        }

        private void SetBlockToCellCordinateMapping()
        {
            Coordinates = new Dictionary<Tuple<int, int>, Tuple<int, int>>();

            // first check if there are blocks
            if (BlockHeight != GridHeight)
            {
                // cellcoordinates(i=up/down,j=left/right)
                for (int i = 0; i < GridHeight; i++)
                {

                    for (int j = 0; j < GridHeight; j++)
                    {
                        var block_i = GetHorizontalShift(i) * BlockHeight + GetHorizontalShift(j);
                        var block_j = GetVerticalShift(i) * BlockHeight + GetVerticalShift(j);
                        var cellcoordinates = new Tuple<int, int>(i, j);
                        var blockcoordinates = new Tuple<int, int>(block_i, block_j);
                        Coordinates.Add(cellcoordinates, blockcoordinates);
                    }
                }
            }
            else
            {
                for (int i = 0; i < GridHeight; i++)
                {
                    for (int j = 0; j < GridHeight; j++)
                    {
                        var block_i = 0;
                        var block_j = (i * GridHeight) + j;
                        var cellcoordinates = new Tuple<int, int>(i, j);
                        var blockcoordinates = new Tuple<int, int>(block_i, block_j);
                        Coordinates.Add(cellcoordinates, blockcoordinates);
                    }
                }
            }
        }

        private List<List<int>> GetHorizontalShiftRanges()
        {
            var indexranges = new List<List<int>>();

            for (int i = 0; i < BlockHeight; i++)
            {
                var zero = i * BlockHeight;
                var rng = new List<int>();

                for (int j = zero; j < zero + BlockHeight; j++)
                    rng.Add(j);

                indexranges.Add(rng);
            }
            return indexranges;
        }

        private List<List<int>> GetVerticalShiftRanges()
        {
            var indexranges = new List<List<int>>();

            for (int i = 0; i < BlockHeight; i++)
            {
                var upper = i + 2 * BlockHeight + 1;
                var rng = new List<int>();

                for (int j = i; j < upper; j += BlockHeight)
                    rng.Add(j);

                indexranges.Add(rng);
            }
            return indexranges;
        }

        private int GetHorizontalShift(int k)
        {
            var indexranges = GetHorizontalShiftRanges();
            var shift = 0;

            foreach (var indexrange in indexranges)
            {
                if (shift >= indexranges.Count)
                    return -1;

                if (indexrange.Contains(k))
                    return shift;
                else
                    shift++;
            }

            return -1;
        }

        private int GetVerticalShift(int k)
        {
            var indexranges = GetVerticalShiftRanges();
            var shift = 0;

            foreach (var indexrange in indexranges)
            {
                if (shift >= indexranges.Count)
                    return -1;

                if (indexrange.Contains(k))
                    return shift;
                else
                    shift++;
            }

            return -1;
        }

        private bool BackTrack(List<Tuple<int, int>> empty_cells)
        {
            //  if all cells are assigned then the sudoku is already solved


            if (empty_cells.Count == 0)
                return true;

            var empty_cell = empty_cells[0];
            var i = empty_cell.Item1;
            var j = empty_cell.Item2;

            for (int value = 1; value < GridHeight + 1; value++)
            {
                // if we can assign i to the cell or not the cell is matrix[row][col]
                var check = !CheckGridValue(i, j, value);

                if (check)
                {
                    SetGridValue(i, j, value);

                    // backtracking
                    if (BackTrack(empty_cells.GetRange(1, empty_cells.Count - 1)))
                        return true;

                    // if we can't proceed with this solution reassign the cell
                    SetGridValue(i, j, 0, overwrite: true);
                }
            }
            return false;
        }

        private string GetDebuggerDisplay()
        {
            return Boardname;
        }
    }
}

