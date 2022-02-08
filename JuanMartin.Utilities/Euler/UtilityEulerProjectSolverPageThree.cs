using JuanMartin.Kernel.Utilities;
using JuanMartin.Kernel.Utilities.DataStructures;
using JuanMartin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace JuanMartin.Utilities.Euler
{
    public partial class UtilityEulerProjectSolver
    {
        /// <summary>
        /// https://projecteuler.net/problem=102 
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result TriangleContainment(Problem arguments)
        {
            bool TriangleContainsOriginUsingArea(int[] vertices)
            {
                //  For the triangle ABC (verteices - [A_x,A_y,B_x,B_y,C_x,C_y]) contains the point P.
                //  The main concept of the solution is that the area of the triangle ABC is equal to the
                //  area of the triangles ABP +APC + PBC.
                bool contains = false;
                int[] a = { vertices[0], vertices[1] };
                int[] b = { vertices[2], vertices[3] };
                int[] c = { vertices[4], vertices[5] };
                int[] p = { 0, 0 }; // origin

                var abc = UtilityMath.GetAreaOfTriangleUsingCoordinates(a, b, c);
                var abp = UtilityMath.GetAreaOfTriangleUsingCoordinates(a, b, p);
                var apc = UtilityMath.GetAreaOfTriangleUsingCoordinates(a, p, c);
                var pbc = UtilityMath.GetAreaOfTriangleUsingCoordinates(p, b, c);

                if (abc == abp + apc + pbc)
                    contains = true;

                return contains;
            };

            //bool TriangleContainsOriginUsingDotProduct(int[] vertices)
            //{
            //    bool contains = false;
            //    int[] a = { vertices[0], vertices[1] };
            //    int[] b = { vertices[2], vertices[3] };
            //    int[] c = { vertices[4], vertices[5] };
            //    int[] p = { 0, 0 }; // origin

            //    // using dot product
            //    int scalar_pab = UtilityMath.DotProduct(p, a, b);
            //    int scalar_pbc = UtilityMath.DotProduct(p, b, c);
            //    int scalar_pca = UtilityMath.DotProduct(p, c, a);

            //    if (scalar_pab.Sign() == scalar_pbc.Sign() && scalar_pab.Sign() == scalar_pca.Sign())
            //        contains = true;

            //    return contains;
            //};

            var cvsinfo = arguments.Sequence.Split('|');
            var fileName = cvsinfo[0];
            var delimiter = Convert.ToChar(cvsinfo[1]);
            int[][] coordinates;
            int count = 0;

            coordinates = UtilityFile.ReadTextToTwoDimensionalNumericArray(fileName, delimiter);

            foreach (var v in coordinates)
            {
                if (TriangleContainsOriginUsingArea(v))
                    count++;
            }
            var answer = count.ToString();
            var message = string.Format("The number of triangles, in triangles.txt, a 27K text file containing the coordinates of one thousand 'random' triangles, for which the interior contains the origin is {0}.", answer);
            if (Answers[arguments.Id] != answer)
            {
                message += string.Format(" => INCORRECT ({0})", Answers[arguments.Id]);
            }
            var r = new Result(arguments.Id, message)
            {
                Answer = answer
            };

            return r;
        }

        /// <summary>
        /// https://projecteuler.net/problem=104 
        ///  Solution from <see cref="https://www.mathblog.dk/project-euler-104-fibonacci-pandigital/"/>
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result PandigitalFibonacciEnds(Problem arguments)
        {
            BigInteger fn2 = 1;
            BigInteger fn1 = 1;
            BigInteger fn = fn1 + fn2;

            BigInteger tailcut = 1000000000; // last 9 digits

            int n = 2;
            bool found = false;

            while (!found)
            {
                n++;

                fn = fn1 + fn2;
                long tail = (long)(fn % tailcut);
                if (UtilityMath.IsPandigital(tail))
                {
                    int digits = 1 + (int)BigInteger.Log10(fn); // fn.ToString().Length
                    if (digits > 9)
                    {
                        long head = (long)(fn / BigInteger.Pow(10, digits - 9)); // firt 9 digits
                        if (UtilityMath.IsPandigital(head))
                        {
                            found = true;
                        }
                    }
                }

                fn2 = fn1;
                fn1 = fn;
            }
            var answer = n.ToString();
            var message = string.Format("Given that Fk = ({0}), is the first Fibonacci number for which the first nine digits AND the last nine digits are 1-9 pandigital, k is {1}.", fn, answer);
            if (Answers[arguments.Id] != answer)
            {
                message += string.Format(" => INCORRECT ({0})", Answers[arguments.Id]);
            }
            var r = new Result(arguments.Id, message)
            {
                Answer = answer
            };

            return r;
        }

        /// <summary>
        /// https://projecteuler.net/problem=105 
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result SpecialSubsetSumsTesting(Problem arguments)
        {
            bool IsSpecialSum(int[] A)
            {

                for (var i = 1; i <= A.Length; i++)
                {
                    var subsetsB = UtilityMath.GetCombinationsOfK<int>(A, i).ToArray();

                    foreach (var B in subsetsB)
                    {
                        var disjoint = A.Except(B).ToArray();

                        for (var j = 1; j <= disjoint.Length; j++)
                        {
                            var subsetsC = UtilityMath.GetCombinationsOfK<int>(disjoint, j).ToArray();

                            foreach (var C in subsetsC)
                            {
                                //if (B.Count() <= C.Count())
                                //    continue;

                                var sumB = B.Sum();
                                var sumC = C.Sum();

                                // rule 1
                                if (sumB == sumC)
                                    return false;

                                // rule 2
                                if (B.Count() > C.Count())
                                    if (sumB <= sumC)
                                        return false;

                            }
                        }
                    }
                }

                return true;
            };
            var cvsinfo = arguments.Sequence.Split('|');
            var fileName = cvsinfo[0];
            var delimiter = Convert.ToChar(cvsinfo[1]);
            int[][] sets;
            int sum = 0;

            sets = UtilityFile.ReadTextToTwoDimensionalNumericArray(fileName, delimiter);

            foreach (var A in sets)
            {
                if (IsSpecialSum(A))
                    sum += A.Sum();
            }


            var answer = sum.ToString();
            var message = string.Format("Using  a 4K text file with one-hundred sets containing seven to twelve elements (the two examples given above are the first two sets in the file), identify all the special sum sets, A1, A2, ..., Ak, and the value of S(A1) + S(A2) + ... + S(Ak). is {0}.", answer);
            if (Answers[arguments.Id] != answer)
            {
                message += string.Format(" => INCORRECT ({0})", Answers[arguments.Id]);
            }
            var r = new Result(arguments.Id, message)
            {
                Answer = answer
            };

            return r;
        }

        /// <summary>
        /// https://projecteuler.net/problem=107 
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result MinimalNetwork(Problem arguments)
        {
            var cvsinfo = arguments.Sequence.Split('|');
            var fileName = cvsinfo[0];
            var delimiter = Convert.ToChar(cvsinfo[1]);
            var network = LoadNetwork(fileName,delimiter);
            double sum = network.GetOutgoingEdges().Sum(e => e.Weight);

            // get minimum possible edge weight 
            UndirectedGraph<int> subset = network.GetMinimumSpanningTreeWithKruskalAlgorithm();
            sum -= subset.GetOutgoingEdges().Sum(e => e.Weight);

            
            var answer = sum.ToString();
            var message = string.Format("Using a given text file containing a network with forty vertices, and given in matrix form, the maximum saving which can be achieved by removing redundant edges whilst ensuring that the network remains connected is {0}.", answer);
            if (Answers[arguments.Id] != answer)
            {
                message += string.Format(" => INCORRECT ({0})", Answers[arguments.Id]);
            }
            var r = new Result(arguments.Id, message)
            {
                Answer = answer
            };

            return r;
        }


        #region Support Methods
        private static UndirectedGraph<int> LoadNetwork(string fileName, char delimiter = ',')
        {
            int[][] matrix = UtilityFile.ReadTextToTwoDimensionalNumericArrayWithNullElements(fileName, delimiter);
            UndirectedGraph<int> network = new UndirectedGraph<int>();

            int dimension = matrix.Length;

            for (int i = 0; i < dimension; i++)
            {
                network.AddVertex(new Vertex<int>( value: i, name: UtilityString.GetColumnName(i)));
            }

            var edges = new List<string>();

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    double w = (double)matrix[i][j];
                    if (w > 0)
                    {
                        var from = network.GetVertex(UtilityString.GetColumnName(i));
                        var to = network.GetVertex(UtilityString.GetColumnName(j));
                        // there should only one edge between two nodes
                        string n = $"{from.Name}-{to.Name}";
                        if (!edges.Contains(UtilityString.ReverseString(n, '-')))
                        {
                            edges.Add(n);
                            network.AddEdge(from, to,name:n, weight: w);
                        }
                    }
                }
            }

            return network;
        }
        #endregion
    }
}