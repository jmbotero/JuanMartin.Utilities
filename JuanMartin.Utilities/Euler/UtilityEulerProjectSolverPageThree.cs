using JuanMartin.Kernel.Extesions;
using JuanMartin.Kernel.Utilities;
using JuanMartin.Kernel.Utilities.DataStructures;
using JuanMartin.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

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

            bool TriangleContainsOriginUsingDotProduct(int[] vertices)
            {
                bool contains = false;
                int[] a = { vertices[0], vertices[1] };
                int[] b = { vertices[2], vertices[3] };
                int[] c = { vertices[4], vertices[5] };
                int[] p = { 0, 0 }; // origin

                // using dot product
                int scalar_pab = UtilityMath.DotProduct(p, a, b);
                int scalar_pbc = UtilityMath.DotProduct(p, b, c);
                int scalar_pca = UtilityMath.DotProduct(p, c, a);

                if (scalar_pab.Sign() == scalar_pbc.Sign() && scalar_pab.Sign() == scalar_pca.Sign())
                    contains = true;

                return contains;
            };

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
            var message = string.Format("Given that Fk = ({0}), is the first Fibonacci number for which the first nine digits AND the last nine digits are 1-9 pandigital, k is {1}.",fn, answer);
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
    }
}