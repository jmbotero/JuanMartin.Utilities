using JuanMartin.Kernel.Utilities;
using JuanMartin.Kernel.Utilities.DataStructures;
using JuanMartin.Kernel.Extesions;
using JuanMartin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using JuanMartin.Models.Gallery;
using JuanMartin.Kernel.Messaging;
using JuanMartin.Kernel;
using System.IO;
using JuanMartin.Kernel.Adapters;

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
            var network = LoadNetwork(fileName, delimiter);
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

        /// <summary>
        /// https://projecteuler.net/problem=108 
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result DiophantineReciprocalsI(Problem arguments)
        {
            //List<Tuple<int, int>> FactorPairs(int number, bool addUniquePairs = true)
            //{
            //    var pairs = new List<Tuple<int, int>>();
            //    var unique = new List<int>();
            //    long[] factors = UtilityMath.GetFactors(number).ToArray();

            //    foreach (var f in factors)
            //    {
            //        int i = (int)f;
            //        int j = number / i;
            //        if (addUniquePairs && !unique.Contains(i + j))
            //        {
            //            unique.Add(i + j);
            //            pairs.Add(new Tuple<int, int>(i, j));
            //        }
            //        else  if(addUniquePairs)
            //        {
            //            pairs.Add(new Tuple<int, int>(i, j));
            //        }
            //    }

            //    return pairs;
            //};

            // See that both x and y needs to be greater than n since they both need to be non-negative,
            // therefore for non-negatve integers a and b, x and y can be expressed as
            // x =  n+a and y= n+b, and replacing these in the original equation we get n^2=a * b,
            // therefore the pairs of factors of n^2 lead to multiple values for x and y and
            // all are solutions to the reciprocals equality.
            var solutions = arguments.IntNumber;
            //int a, b;
            //int x, y;
            long n = 2;

            while (true)
            {
                //int count = 0;
                //var f = FactorPairs(n * n);

                //foreach (var p in f)
                //{
                //    a = p.Item1;
                //    b = p.Item2;
                //    x = n + a;
                //    y = n + b;

                //    double reciprocals = (double)(1)/ x;
                //    reciprocals += (double)(1)/ y;
                //    if (reciprocals == (double)(1) / n)
                //    {
                //        Console.WriteLine($"1/{x} + 1/{y} = 1/{n}");
                //        count++;
                //    }
                //}


                //if (count >= solutions)
                //    break;

                /// Hack, only use up to prime 17 from <see cref="https://www.mathblog.dk/project-euler-108-diophantine-equation/"/>
                var primes = new System.Collections.Generic.Queue<long>();
                foreach (var p in UtilityMath.ErathostenesSieve(17))
                    primes.Enqueue(p);

                var count = (UtilityMath.CountFactors(n * n, primes) + 1) / 2;

                if (count > solutions)
                    break;
                n++;
            }

            var answer = n.ToString();
            var message = string.Format(" The least value of n for which the number of distinct solutions exceeds {0} is {1}.", UtilityMath.NumberToLetters((long)solutions), answer);
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
        /// https://projecteuler.net/problem=111 , got it to work on test problem   
        /// but  not  for  10, for this took code from mathblog.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result PrimesWithRuns(Problem arguments)
        {
            IEnumerable<long> GenerateNumbers(long lowerLimit, long upperLimit)
            {
                for (long x = lowerLimit; x <= upperLimit; x++)
                {
                    if (CheckNumber(x))
                    {
                        if (UtilityMath.IsPrimeUsingSquares(x))
                            yield return x;
                    }
                }
            };

            /// Return true if number couuld be prime and has duplicate digits
            bool CheckNumber(long number)
            {
                var last = number % 10;
                bool isPossiblePrime = last != 0 && last != 5 && last % 2 != 0 && UtilityMath.DigitsSum(number) % 3 != 0;

                /// <see cref="https://stackoverflow.com/questions/1282271/how-to-test-a-prime-number-1000-digits-long/1282397#1282397"/>
                isPossiblePrime = isPossiblePrime && (number > 3 && (number - 1) % 6 == 0 || (number + 1) % 6 == 0);
                return isPossiblePrime && number.HasDuplicates();
            };

            long sum = 0;
            int n = arguments.IntNumber;

            if (arguments.TestMode)
            {
                var N = new Dictionary<int, List<long>> {
                { 0, new List<long>() },
                { 1, new List<long>() },
                { 2, new List<long>() },
                { 3, new List<long>() },
                { 4, new List<long>() },
                { 5, new List<long>() },
                { 6, new List<long>() },
                { 7, new List<long>() },
                { 8, new List<long>() },
                { 9, new List<long>() }
                    };


                long lowerBound = Convert.ToInt64("1" + "0".Repeat(n - 1));
                long upperBound = Convert.ToInt64("9".Repeat(n));
                var primes = GenerateNumbers(lowerBound, upperBound).ToArray(); //UtilityMath.ErathostenesSieve(upperBound, lowerBound, threadCount: 2);

                //       ulong sum = primes.Aggregate((n1, n2) => n1 + n2);

                var M = UtilityMath.MaximumNumericFrequency(primes);
                foreach (long p in primes)
                {
                    string number = p.ToString();

                    var repeats = number.DigitCounts(); //LongCount(x => (x - 48) == d);
                    for (int d = 0; d < 10; d++)
                    {
                        if (repeats[d] == M[d])
                            N[d].Add(p);
                    }
                }

                foreach (var primeSeries in N.Values)
                    sum += primeSeries.Sum();
            }
            else
            {
                var p = new Problem111();
                sum = p.Solve();
            }
            var answer = sum.ToString();
            var message = string.Format("The sum of all S({0} d). is {1}.", n, answer);
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
        /// https://projecteuler.net/problem=112
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result BouncyNumbers(Problem arguments)
        {
            IEnumerable<Photography> GetAllPhotographies(int userId, int pageId = 1)
            {
                AdapterMySql _dbAdapter = new AdapterMySql("localhost", "photogallery", "root", "yala");
                var photographies = new List<Photography>();
                if (_dbAdapter == null)
                    throw new ApplicationException("MySql connection nnnot set.");

                Message request = new Message("Command", System.Data.CommandType.StoredProcedure.ToString());

                request.AddData(new ValueHolder("PhotoGraphy", $"uspGetAllPhotographies({pageId},8,{userId})"));
                request.AddSender("AddPhotoGraphy", typeof(Photography).ToString());

                _dbAdapter.Send(request);
                IRecordSet reply = (IRecordSet)_dbAdapter.Receive();

                if (reply.Data != null && reply.Data.GetAnnotation("Record") != null)
                {
                    foreach (ValueHolder record in reply.Data.Annotations)
                    {
                        var id = (long)record.GetAnnotation("Id").Value;
                        var source = Convert.ToInt32(record.GetAnnotation("Source").Value);
                        var path = (string)record.GetAnnotation("Path").Value;
                        var fileName = (string)record.GetAnnotation("Filename").Value;
                        var title = (string)record.GetAnnotation("Title").Value;
                        var location = (string)record.GetAnnotation("Location").Value;
                        var rank = (long)record.GetAnnotation("Rank").Value;
                        var keywords = (string)record.GetAnnotation("Keywords").Value;

                        var photography = new Photography
                        {
                            UserId = userId,
                            Id = id,
                            FileName = fileName,
                            Path = path,
                            Source = (Photography.PhysicalSource)source,
                            Title = title,
                            Location = location,
                            Rank = rank
                        };

                        photography.AddKeywords(keywords);

                        photographies.Add(photography);
                    }
                }

                return photographies;
            };

            int GetGalleryPageCount(int pageSize)
            {
                AdapterMySql _dbAdapter = new AdapterMySql("localhost", "photogallery", "root", "yala");
                if (_dbAdapter == null)
                    throw new ApplicationException("MySql connection nnnot set.");

                Message request = new Message("Command", System.Data.CommandType.StoredProcedure.ToString());

                request.AddData(new ValueHolder("uspGetPageCount", $"uspGetPageCount({pageSize})"));
                request.AddSender("Gallery", typeof(Photography).ToString());

                _dbAdapter.Send(request);
                IRecordSet reply = (IRecordSet)_dbAdapter.Receive();

                var pageCount = (int)reply.Data.GetAnnotationByValue(1).GetAnnotation("pageCount").Value;

                return pageCount;
            };

            int UpdatePhotographyRanking(long id, int userId, int rank)
            {
                AdapterMySql _dbAdapter = new AdapterMySql("localhost", "photogallery", "root", "yala");
                if (_dbAdapter == null)
                    throw new ApplicationException("MySql connection nnnot set.");

                Message request = new Message("Command", System.Data.CommandType.StoredProcedure.ToString());

                request.AddData(new ValueHolder("uspUpdateRanking", $"uspUpdateRanking({id},{userId},{rank})"));
                request.AddSender("Photography", typeof(Photography).ToString());

                _dbAdapter.Send(request);
                IRecordSet reply = (IRecordSet)_dbAdapter.Receive();

                // be sure record.GetAnnotation("...") exists and is not null
                var rankingId = (int)reply.Data.GetAnnotationByValue(1).GetAnnotation("id").Value;

                return rankingId;
            };

            int percent =   arguments.IntNumber;
            int n = 99;
            int p = 0, bouncies = 0;

            var upr = UpdatePhotographyRanking(1, 9, 6);
            var pc = GetGalleryPageCount(8);
            var gph = GetAllPhotographies(1, 1).ToList();

            while (true)
            {
                
                if (n > 100 && UtilityMath.IsBouncyNumber(n))
                {
                    bouncies++;
                    p = 100 * bouncies / n;
                }
                if (p == percent)
                    break;
                else
                    n++;
            }

            var answer = n.ToString();
            var message = string.Format("The lea  st number for which the proportion of bouncy numbers is exactly {0}% is {1}.", percent, answer);
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