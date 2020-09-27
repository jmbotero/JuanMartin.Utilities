using JuanMartin.Kernel.Extesions;
using JuanMartin.Kernel.Utilities;
using JuanMartin.Kernel.Utilities.DataStructures;
using JuanMartin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace JuanMartin.Utilities.Euler
{
    public partial class UtilityEulerProjectSolver
    {
        private enum CardSuits
        {
            Diamonds = 'D',
            Hearts = 'H',
            Clovers = 'C',
            Spades = 'S'
        }

        private enum HandType
        {
            None = 0,
            OnePair = 100000,
            TwoPairs = 200000,
            ThreeOfaKind = 300000,
            Straight = 400000,
            Flush = 500000,
            FullHouse = 600000,
            FourOfaKind = 700000,
            StraightFlush = 800000,
            RoyalFlush = 900000

        }

        private enum CardValues
        {
            Ten = 10,
            Jack = 11,
            Queen = 12,
            King = 13,
            Ace = 14
        }


        /// <summary>
        /// https://projecteuler.net/problem=51 
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result PrimeDigitReplacements(Problem arguments)
        {
            /// <summary>
            /// Return true if all digits in a numbers are the same, except those that repeat the
            /// same digit and it just increases/decreases from one to the other. For example
            /// CompareNumbers(124353,124757)=true, CompareNumbers(124353,124151)=true,
            /// CompareNumbers(121313,121757)=false. Also if numbers are the same return false.
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            bool NumbersBelongToSameDigitFamily(int left, int right, int repeating_sequences = 1)
            {
                var repeating = new Dictionary<char, List<int>>();
                var sleft = left.ToString();
                var sright = right.ToString();

                if (left == right)
                    return false;
                if (sleft.Length != sright.Length)
                    return false;

                for (var i = 0; i < sleft.Length; i++)
                {
                    var rdigit = sright[i];
                    var ldigit = sleft[i];

                    if ((repeating.Count < repeating_sequences) && !repeating.ContainsKey(ldigit) && (sleft.Count(c => c == ldigit) >= 2))
                    {
                        if (repeating.ContainsKey(ldigit) && repeating[ldigit].Contains(i))
                            continue;
                        else
                        {
                            repeating.Add(ldigit, new List<int>());

                            foreach (var (digit, j) in sleft.Enumerate())
                            {
                                if (digit == ldigit)
                                    repeating[ldigit].Add(j);
                            }
                            bool match = true;
                            foreach (var k in repeating[ldigit])
                            {
                                match = match && sright[k] == rdigit;

                                if (!match) break;
                            }
                            if (!match) return false;
                        }
                    }
                    else if (repeating.ContainsKey(ldigit) && repeating[ldigit].Contains(i))
                        continue;
                    else if (rdigit == ldigit)
                        continue;
                    else if (ldigit != rdigit)
                        return false;
                }

                return true;
            }


            // get all six digit primes
            var primes = UtilityMath.GeneratePrimes(100000, 1000000).ToArray();
            var repeats = new List<int>();

            // get primes with repeated digits (not necessarily adjacent digits)
            for (int i = 0; i < primes.Length; i++)
            {
                var counts = UtilityMath.GetRepeatedDigitCounts(primes[i]);
                if (counts.Count(c => c > 1) >= 1) // choose only primes with at least one repeatig digit
                    repeats.Add(primes[i]);
            }

            // get first eight number prime-family
            HashSet<int> family=null;

            foreach (var prime in repeats)
            {
                family = new HashSet<int>
                {
                    prime
                };

                foreach (var n in repeats)
                {
                    if (NumbersBelongToSameDigitFamily(prime, n))
                    {
                        family.Add(n);
                    }

                    if (family.Count == 8)
                        break;
                }
                if (family.Count == 8)
                    break;
            }
            primes = null;
            repeats = null;

            var answer = family?.First().ToString();
            var message = string.Format("The smallest prime which, by replacing part of the number (not necessarily adjacent digits) with the same digit, is part of an eight prime value family is {0}.", answer);
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
        /// https://projecteuler.net/problem=52
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result PermutedMultiples(Problem arguments)
        {
            const int limit = 6;
            var found = 1;
            int number;

            for (number = 5; number < int.MaxValue; number++)
            {
                var t = 2;
                while (found < limit && t <= limit)
                {
                    var multiple = number * t;

                    if (multiple.ToString().IsAnagram(number.ToString()))
                        found++;

                    t++;
                }
                if (found == limit - 1)
                    break;
                else
                    found = 0;
            }
            var answer = number.ToString();
            var message = string.Format("The smallest positive integer, x, such that 2x, 3x, 4x, 5x, and 6x, contain the same digits is {0}.", answer);
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
        /// https://projecteuler.net/problem=53
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result CombinatoricSelections(Problem arguments)
        {
            var limit = arguments.IntNumber;
            var count = 0;


            //todo: use factaorial in loop and cache it in array

            for (int n = 1; n <= 100; n++)
            {
                for (int r = 1; r < n; r++)
                {
                    if (UtilityMath.BinomialCoefficients(n, r) >= limit)
                        count++;
                }
            }
            var answer = count.ToString();
            var message = string.Format("{0} not necessarily distinct, values of combinatorics(n, r), for 1 ≤ n ≤ 100, are greater than {1}.", answer, limit);
            if (Answers[arguments.Id] != answer)
            {
                message += string.Format(" => INCORRECT ({0})", Answers[arguments.Id]);
            }
            var result = new Result(arguments.Id, message)
            {
                Answer = answer
            };

            return result;
        }

        /// <summary>
        /// https://projecteuler.net/problem=54
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result PokerHands(Problem arguments)
        {
            //var a = EvaluatePokerHand(new string[] { "5H", "5C", "6S", "7S", "KD" });
            //var b = EvaluatePokerHand(new string[] { "2C", "3S", "8S", "8D", "TD" });
            //Console.WriteLine("Winner {0}.", (a > b) ? "1" : "2");
            //a = EvaluatePokerHand(new string[] { "5D", "8C", "9S", "JS", "AC" });
            //b = EvaluatePokerHand(new string[] { "2C", "5C", "7D", "8S", "QH" });
            //Console.WriteLine("Winner {0}.", (a > b) ? "1" : "2");
            //a = EvaluatePokerHand(new string[] { "2D", "9C", "AS", "AH", "AC" });
            //b = EvaluatePokerHand(new string[] { "3D", "6D", "7D", "TD", "QD" });
            //Console.WriteLine("Winner {0}.", (a > b) ? "1" : "2");
            //a = EvaluatePokerHand(new string[] { "4D", "6S", "9H", "QH", "QC" });
            //b = EvaluatePokerHand(new string[] { "3D", "6D", "7H", "QD", "QS" });
            //Console.WriteLine("Winner {0}.", (a > b) ? "1" : "2");
            //a = EvaluatePokerHand(new string[] { "2H", "2D", "4C", "4D", "4S" });
            //b = EvaluatePokerHand(new string[] { "3C", "3D", "3S", "9S", "9D" });
            //Console.WriteLine("Winner {0}.", (a > b) ? "1" : "2");

            var count = 0;
            var cvsinfo = arguments.Sequence.Split('|');
            var fileName = cvsinfo[0];
            var delimiter = Convert.ToChar(cvsinfo[1]);
            var cards = UtilityFile.ReadTextToTwoDimensionalArray(fileName, delimiter);

            for (int i = 0; i < cards.Length; i++)
            {
                var player1 = cards[i].Take(cards[i].Length / 2).ToArray();
                var player2 = cards[i].Skip(cards[i].Length / 2).ToArray();
                cards[i] = null;

                var p1 = EvaluatePokerHand(player1);
                var p2 = EvaluatePokerHand(player2);

                if (p1 > p2)
                    count++;
            }

            var answer = count.ToString();
            var message = string.Format("Player 1 wins {0} hands.", answer);
            if (Answers[arguments.Id] != answer)
            {
                message += string.Format(" => INCORRECT ({0})", Answers[arguments.Id]);
            }
            var result = new Result(arguments.Id, message)
            {
                Answer = answer
            };

            return result;
        }

        /// <summary>
        /// https://projecteuler.net/problem=55
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result LychrelNumbers(Problem arguments)
        {
            var limit = arguments.IntNumber;
            var count = 0;

            for (BigInteger number = 11; number < limit; number++)
            {
                if (IsLychrel(number))
                    count++;
            }

            var answer = count.ToString();
            var message = string.Format("There are {0} Lychrel numbers below {1}.", answer, limit);
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
        /// https://projecteuler.net/problem=56
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result PowerfulDigitSum(Problem arguments)
        {
            var limit = arguments.IntNumber;
            long max = 100; // include all 100 * 1^b to begin with

            for (int a = 2; a < limit; a++)
            {
                for (int b = 1; b < limit; b++)
                {
                    var ab = BigInteger.Pow(a, b);
                    var sum = UtilityMath.DigitsSum(ab.ToString());
                    if (sum > max)
                        max = sum;
                }
            }

            var answer = max.ToString();
            var message = string.Format("The maximum digital sum of natural numbers of the form, a^b, where a, b < {0}, is {1}.", limit, answer);
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
        /// https://projecteuler.net/problem=57
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result SquareRootConvergents(Problem arguments)
        {
            var limit = arguments.IntNumber;
            var count = 0;
            BigInteger p = 1;
            BigInteger q = 1;

            for (int i = 1; i < limit; i++)
            {
                var numerator = p + 2 * q;
                var denomintor = p + q;

                var countN = numerator.ToString().Length;
                var countD = denomintor.ToString().Length;

                //we could add reundaant check: if (UtilityMath.IsNaturalNumber(_numerator.ToString()) && UtilityMath.IsNaturalNumber(denomintor.ToString()) && countN > countD), but slows down loop 
                if (countN > countD)
                    count++;

                p = numerator;
                q = denomintor;
            }

            var answer = count.ToString();
            var message = string.Format("In the first {0} expansions, {1} fractions contain a _numerator with more digits than denominator.", limit, answer);
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
        /// https://projecteuler.net/problem=58
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result SpiralPrimes(Problem arguments)
        {
            bool IsPrime(int n)
            {
                return n != 1 && (n != 3 || n % 3 == 0) && (n != 2 || n % 2 == 0) && UtilityMath.IsPrimeUsingSquares(n);
            }

            double ratio = 1;
            var step = 0;
            var length = 1;
            var corner = 1;
            var count = 0;

            while (ratio > 0.10)
            {
                length += 2;
                step += 2;
                for (int i = 1; i < 4; i++)
                // only check primality for three corners, so just skip the southwest corner numbers
                {
                    corner += step;
                    if (IsPrime(corner))
                        count++;
                }
                corner += step;

                ratio = (double)count / (2 * length - 1);
            }
            var answer = length.ToString();
            var message = string.Format("The side length of the square spiral for which the ratio of primes along both diagonals first falls below {0}% is {1}.", ratio * 100, answer);
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
        /// https://projecteuler.net/problem=59
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result XorDecryption(Problem arguments)
        {
            const char FirstChar = 'a';
            const char LastChar = 'z';

            var sum = 0;
            var key = string.Empty;
            var text = string.Empty;

            var cvsinfo = arguments.Sequence.Split('|');
            var fileName = cvsinfo[0];
            var delimiter = Convert.ToChar(cvsinfo[1]);
            var qualifier = Convert.ToChar(cvsinfo[2]);
            var cyphers = UtilityFile.ReadCsvToArray(fileName, delimiter, qualifier);
            var ascii = cyphers.Select(X => Convert.ToInt32(X)).ToArray();

            var allPossiblePasswords = (from a in FirstChar.To(LastChar)
                                        from b in FirstChar.To(LastChar)
                                        from c in FirstChar.To(LastChar)
                                        select new[] { Convert.ToInt32(a), Convert.ToInt32(b), Convert.ToInt32(c) }).ToArray();

            foreach (var password in allPossiblePasswords)
            {
                var decrypted = UtilityMath.Encrypt(ascii, password);
                text = new string(decrypted.Select(x => Convert.ToChar(x)).ToArray());

                if (text.Contains(" the ")) //perform simple check: if Text contains single word 'the' it is in english
                {
                    key = new string(password.Select(x => Convert.ToChar(x)).ToArray());
                    break;
                }
            }
            sum = text.Aggregate(0, (runningTotal, next) => runningTotal += next);

            var answer = sum.ToString();
            var message = string.Format("The sum of the ASCII values in the original English text is {0}.", answer);
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
        /// https://projecteuler.net/problem=60
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result PrimePairSets(Problem arguments)
        {
            var set = new PrimeSet(arguments.IntNumber);

            set.CalculatePrimeSetMinimumSum();

            var answer = set.Minimums[set.Count - 1].ToString();
            var message = string.Format("The lowest sum for of {0} primes for which any two primes concatenate to produce another prime is {1}.", set.Count, answer);
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
        /// https://projecteuler.net/problem=61
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result CyclicalFigurateNumbers(Problem arguments)
        {
            var numbers = UtilityMath.GetPolygonalNumbers<int>(1009, 9999).ToArray();

            var count = numbers.Length;

            var types = Enumerable.Range(0, count).ToArray<int>(); // array of polygonal number types used
            var setNumberOrder = UtilityString.GeneratePermutations<int>(types); // permutation of the types indicat1ing all combinations of these in a sequence of numbers

            var s = UtilityMath.GetNumericalCyclicalSet(setNumberOrder, numbers);

            var answer = s.set.Sum().ToString();

            var message = string.Format("The sum of the only ordered set of six cyclic 4-digit numbers for which each polygonal type: triangle, square, pentagonal, hexagonal, heptagonal, and octagonal, is represented by a different number in the set is {0}.", answer);
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
        /// https://projecteuler.net/problem=62
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result CubicPermutations(Problem arguments)
        {
            long minCube = int.MaxValue;
            var cubes = new Dictionary<string, Cube>();

            for (long i = 3; i < int.MaxValue; i++)
            {
                var cubePow = i * i * i;

                //var key = GetSmallestPermutation(cubePow).ToString();
                var key = cubePow.GetSmallestPermutation();

                if (!cubes.ContainsKey(key))
                {
                    var c = new Cube(i);
                    cubes.Add(key, c);
                }
                var cube = cubes[key];
                cube.PermutationsCount++;
                if (i < cube.Value)
                {
                    cube.Value = i;
                }
                cubes[key] = cube;

                if (cube.PermutationsCount == 5)
                {
                    minCube = cube.Value;
                    break;
                }
            }
            var answer = (minCube * minCube * minCube).ToString();

            var message = string.Format("The smallest cube for which exactly five permutations of its digits are cube is {0}.", answer);
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
        /// https://projecteuler.net/problem=63
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result PowerfulDigitCounts(Problem arguments)
        {
            const int max = 9; // maximum base
            var count = 0;
            var maxExponent = (int)(1 / (1 - Math.Log10(max))); //max(e) = 21 => the max base, 9, has a Power of 22 caracters

            //var s = 0;

            //for (int number1 = 1; number1 <= max; number1++)
            //{
            //    s += UtilityMath.GetCountOfLengthPower(number1, max, maxExponent);
            //}

            for (int n = 1; n <= maxExponent; n++)
            {
                // a cannot be greater than 9: Having f(n) = n^e = e => log10(n) < 1 => n < 10
                for (int number = 1; number <= max; number++)
                {
                    var x = UtilityMath.PowerUsingLoop(number, n);
                    if (("" + x).ToCharArray().Length == n)
                    {
                        count++;
                        //Console.WriteLine(a + "^" + n + " = " + b);
                    }
                }
            }
            var answer = count.ToString();

            var message = string.Format("The number of n-digit positive integers exist which are also an nth Power is {0}.", answer);
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
        /// https://projecteuler.net/problem=64
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result OddPeriodSquareRoots(Problem arguments)
        {
            var count = 0;
            var upperBound = arguments.IntNumber;

            for (int n = 2; n <= upperBound; n++)
            {
                if (UtilityMath.IsPerferctSquare(n)) continue;

                var p = UtilityMath.GetContinuedFractionExpansionPeriodicLengthForNonPerfectSquare(n);
                if (p % 2 == 1) //only if odd
                    count++;
            }
            var answer = count.ToString();

            var message = string.Format("The number of continued fractions for N ≤ {0} have an odd period is {1}.", upperBound, answer);
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
        /// https://projecteuler.net/problem=68
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result Magic5GonRing(Problem arguments)
        {
            var n = arguments.ListOfNumbers;
            var ringSize = arguments.IntNumber;
            var sideLength = 3;

            var ring = UtilityMath.GetPentagonalRing(n, sideLength, ringSize);

            // traverse ring in circular index from smaller outer node
            // if iendex go: n, 0, 1,..., n-2, n-1

            var index = new int[ringSize];
            var v = 0;

            index[0] = ringSize - 1;
            for (var i = 1; i < ringSize; i++)
            {
                index[i] = v;
                v++;
            }

            var builder = new StringBuilder();
            foreach (var i in index)
            {
                var side = ring[i];
                foreach (var d in side)
                    builder.Append(d.ToString());
            }
            string answer = builder.ToString();
            var message = string.Format("The maximum 16-digit string for a \"magic\" 5-gon ring is {0}.", answer);
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
        /// https://projecteuler.net/problem=71
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result OrderedFractions(Problem arguments)
        {
            var integers = arguments.ListOfNumbers;
            var key = new Fraction(integers[0], integers[1]);
            var d = arguments.IntNumber;

            var sort_list = UtilityMath.GetReducedProperFractions(d, key, true).ToList();
            string answer = sort_list[0].Numerator.ToString();

            var message = string.Format("The numerator of the fraction immediately to the left of {0} is {1}.", key.ToString(), answer);
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
        /// https://projecteuler.net/problem=73
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result CountingFractionsInRange(Problem arguments)
        {
            var d = arguments.IntNumber;
            var bounds = arguments.ListOfNumbers;
            var begin = new Fraction(bounds[0], bounds[1]);
            var end = new Fraction(bounds[2], bounds[3]);

            var list = UtilityMath.GenerateFareySequence(d);
            var count = list.Count(fraction => fraction > begin && fraction < end);
            var answer = count.ToString();

            var message = string.Format("The number of fractions lie between {0} and {1} in the sorted set of reduced proper fractions for d ≤ {2} is {3}.", begin.ToString(), end.ToString(), d, answer);

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
        /// https://projecteuler.net/problem=96
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result Sudoku(Problem arguments)
        {
            var s = 0;
            foreach (var sudoku in LoadBoards(arguments))
            {
                sudoku.Solve();

                s += sudoku.CheckSum;
            }
            string answer = s.ToString();

            var message = string.Format("The sum of the 3-digit numbers found in the top left corner of each solution grid is {0}.", answer);
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
        /// https://projecteuler.net/problem=75
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result SingularIntegerRightTriangles(Problem arguments)
        {
            var perimeter = arguments.IntNumber;
            var values = 0;

            var a = UtilityMath.GetPythagoreanTriples_Pitagoras(120, false);
            for (int L = 12; L < perimeter; L++)
            {
                if (GetSingularPythagoreanTriples(L) == 1)
                    values++;
            }

            var answer = values.ToString();

            var message = string.Format("Given that L is the length of the wire, {0} values of L ≤ {1} can form exactly one integer sided right angle triangle.", answer, perimeter);
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
        /// https://projecteuler.net/problem=76
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result CountingSummations(Problem arguments)
        {
            var sum = arguments.IntNumber;
            var answer = string.Empty;

            var partitions = UtilityMath.Partitions(sum);

            //oeis a(n) is the number of partitions of n (the partition numbers).
            //int[] A000041 = { 1, 1, 2, 3, 5, 7, 11, 15, 22, 30, 42, 56, 77, 101, 135, 176, 231, 297, 385, 490, 627, 792, 1002, 1255, 1575, 1958, 2436, 3010, 3718, // (28) -> {00:08:19.7112021} 
            //                  4565, 5604, 6842, 8349, 10143, 12310, 14883, 17977, 21637, 26015, 31185, 37338, 44583, 53174, 63261, 75175, 89134, 105558, 124754, 147273, 173525 };
            //var sw = new Stopwatch();

            //foreach(var (count,i) in A000041.Enumerate())
            //{
            //    sw.Start();
            //    var partitions = UtilityMath.Partitions(i);
            //    sw.Stop();
            //    var elapsed = sw.Elapsed;

            //    if (partitions.Count + 1 != count)
            //    {
            //        answer = "-1";
            //    }
            //}
            answer = partitions.Count.ToString();
            string message = string.Format("There are {0} different ways can {1} be written as a sum of at least two positive integers.", answer, sum);


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
        /// https://projecteuler.net/problem=79
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result PasscodeDerivation(Problem arguments)
        {
            var fileinfo = arguments.Sequence.Split('|');
            var fileName = fileinfo[0];


            var p = LoadLoginAttempts(fileName);

            var answer = p.Code.ToString(); // 73162890

            var message = string.Format("Given that the three characters are always asked for in order, after analysing the given file ({0}), the shortest possible secret passcode of unknown length is {1}.", fileName, answer);
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
        /// https://projecteuler.net/problem=80
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result SquareRootDigitalExpansion(Problem arguments)
        {
            var count = (int)arguments.Numbers[0];
            var digits = (int)arguments.Numbers[1];
            long sum = 0;

            #region Sqrt Test
            for (double expected_number = 0.001; expected_number < 100; expected_number += 0.5d)
            {
                double actual_number = UtilityMath.Sqrt_By_Substraction(expected_number);

                if ((expected_number - (actual_number * actual_number)) > .0005)
                    throw new ArithmeticException(string.Format("Sqrt by substraction differed with margin of error for {0}", expected_number));
            }
            #endregion

            for (var number = 1; number <= count; number++)
            {
                if (!UtilityMath.IsPerferctSquare((double)number))
                {
                    var sqrt = UtilityMath.SqrtDigitExpansion(number, digits);

                    sum += UtilityMath.DigitsSum(sqrt.ToString());
                }
            }
            var answer = sum.ToString();

            var message = string.Format("The total of the digital sums of the first {0} decimal digits for all the irrational square roots of the first {1} natural numbers is {2}.", digits, count, answer);
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
        /// https://projecteuler.net/problem=83
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result PathSumFourWays(Problem arguments)
        {
            var cvsinfo = arguments.Sequence.Split('|');
            var fileName = cvsinfo[0];
            var delimiter = Convert.ToChar(cvsinfo[1]);
            var matrix = new Matrix(fileName,delimiter);
            var path = new List<int>();

            matrix.Transpose(); 
            matrix.PopulateNeighbors();
            path =  matrix.GetMinimalPath();
             
            var answer = path.Sum().ToString();

            var message = string.Format("The minimal path sum from the top left to the bottom right, by moving left, right, up, and down, is equal to {0}.", answer);
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

        
        /// <summary>
        /// A common security method used for online banking is to ask the user for three random characters from a passcode. 
        /// This method reads from a textfile some of these thre digits login attempts
        /// </summary>
        /// <param name="fiename"></param>
        /// <returns></returns>
        private static Passcode LoadLoginAttempts(string filename)
        {
            var p = new Passcode();
            HashSet<string> attempts = new HashSet<string>();

            using (var reader = new StreamReader(filename, Encoding.UTF8))
            {
                var line = "";

                while ((line = reader.ReadLine()) != null)
                {
                    attempts.Add(line);
                }

                foreach (var key in attempts)
                {
                    var digits = key.SplitIntoNumericParts(1).ToArray();
                    p.Insert(digits);
                }
            }
            return p;
        }

        /// <summary>
        /// For right triangles, using Euclid formula, modified function for problem75
        /// </summary>
        /// <param name="p">Triangle perimeter</param>
        /// <returns></returns>
        private static int GetSingularPythagoreanTriples(long p)
        {
            var count = 0;

            var bound = (long)Math.Sqrt(p / 2);
            long m;
            for (m = 2; m <= bound; m++)
            {
                if ((p / 2) % m == 0)
                { // m found
                    long k;
                    if (m % 2 == 0)
                    { // ensure that we find an odd number for k
                        k = m + 1;
                    }
                    else
                    {
                        k = m + 2;
                    }
                    while (k < 2 * m && k <= p / (2 * m))
                    {
                        if (p / (2 * m) % k == 0 && UtilityMath.EuclidianGCD(k, m) == 1)
                        {
                            long d = p / 2 / (k * m);
                            long n = k - m;
                            long a = d * (m * m - n * n);
                            long b = 2 * d * n * m;
                            long c = d * (m * m + n * n);

                            if (a + b + c == p)
                                count++;

                            // only consider those which only have one combination
                            if (count > 1)
                                return 0;
                        }
                        k += 2;
                    }
                }
            }
            return count;
        }


        //private static long GetSmallesstPermutation(long number)
        //{
        //    var k = number;
        //    var digits = new int[10];
        //    long retVal = 0;

        //    while (k > 0)
        //    {
        //        digits[k % 10]++;
        //        k /= 10;
        //    }

        //    for (int i = 9; i >= 0; i--)
        //    {
        //        for (int j = 0; j < digits[i]; j++)
        //        {
        //            retVal = retVal * 10 + i;
        //        }
        //    }

        //    return retVal;
        //}
        private static double ConergentFraction(int n)
        {
            if (n == 0)
                return 2;
            else
                return 2 + 1 / ConergentFraction(n - 1);
        }

        //private static double SquareWithConvergence(int n)
        //{
        //    return 1 + 1 / ConergentFraction(n);
        //    //1 + 1/2
        //    //1 + 1/(2 + 1/2))
        //    //1 + 1/(2 + 1/(2 + 1/2)))
        //}

        private static bool IsLychrel(BigInteger number)
        {
            var lychrel = number;
            for (int i = 0; i < 50; i++)
            {
                lychrel += lychrel.Reverse();
                if (UtilityString.IsPalindrome(lychrel.ToString()))
                    return false;
            }
            return true;
        }
        private static int EvaluatePokerHand(string[] hand)
        {
            // determine card frequencies
            var values = new int[15];
            var suits = new char[15];
            foreach (var card in hand)
            {
                var s = card[1];
                var v = card.Remove(1);
                var i = 0;

                switch (v)
                {
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        {
                            i = Convert.ToInt32(v);
                            break;
                        }
                    case "T":
                        {
                            i = (int)CardValues.Ten;
                            break;
                        }
                    case "A":
                        {
                            i = (int)CardValues.Ace;
                            break;
                        }
                    case "J":
                        {
                            i = (int)CardValues.Jack;
                            break;
                        }
                    case "Q":
                        {
                            i = (int)CardValues.Queen;
                            break;
                        }
                    case "K":
                        {
                            i = (int)CardValues.King;
                            break;
                        }

                    default:
                        throw new Exception("Unexpected Card Value");
                }
                values[i]++;
                suits[i] = s;
            }

            var handType = 0; // 100000-900000
            var typeCardNumber = 0; // 1000-14000
            if (values.Contains(2))
            {
                var first = Array.IndexOf(values, 2);
                var second = Array.IndexOf(values, 2, first + 1);
                var third = Array.IndexOf(values, 3, first + 1);
                if (first != -1 && second == -1 && third == -1)
                {
                    handType = (int)HandType.OnePair;
                    typeCardNumber = first;
                }
                if (first != -1 && second != -1 && third == -1)
                {
                    handType = (int)HandType.TwoPairs;
                    typeCardNumber = (first * second);
                }
                if (first != -1 && second == -1 && third != -1)
                {
                    handType = (int)HandType.FullHouse;
                    typeCardNumber = third;
                }

            }
            else if (values.Contains(3))
            {
                handType = (int)HandType.ThreeOfaKind;
                typeCardNumber = Array.IndexOf(values, 3);
            }
            else if (values.Contains(4))
            {
                handType = (int)HandType.FourOfaKind;
                typeCardNumber = Array.IndexOf(values, 4);
            }
            else
            {
                var allCardsHaveConsecutiveValues = AreConsecutivesSequential(values);
                var allCardsHaveSameSuit = AreAllSuitsSame(suits);

                if (allCardsHaveConsecutiveValues && allCardsHaveSameSuit)
                {
                    handType = (int)HandType.StraightFlush;
                }
                if (allCardsHaveConsecutiveValues && !allCardsHaveSameSuit)
                {
                    handType = (int)HandType.Straight;
                }
                if (!allCardsHaveConsecutiveValues && allCardsHaveSameSuit)
                {
                    handType = (int)HandType.Flush;
                }
            }
            int highValueCard = GetHighValueCard(values);
            // determine hand types annd rank
            int rank = handType + 1000 * typeCardNumber + highValueCard;

            return rank;
        }

        private static bool AreConsecutivesSequential(int[] array)
        {
            var first = Array.IndexOf(array, 1);
            var consecutives = array.Skip(first).Take(5).ToArray();

            var selected = consecutives.All(x => x.Equals(1));

            return selected;
        }

        private static bool AreAllSuitsSame(char[] array)
        {
            var first = '\0';
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] != '\0')
                {
                    first = array[i];
                    break;
                }
            }

            var count = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] == first)
                {
                    count++;
                }
            }

            return count == 5;
        }

        private static int GetHighValueCard(int[] values)
        {
            var value = 0;

            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] == 1)
                    value = i;
            }
            return value;
        }

        private static List<Sudoku> LoadBoards(Problem arguments)
        {
            var cvsinfo = arguments.Sequence.Split('|');
            var fileName = cvsinfo[0];
            var count = Convert.ToInt32(cvsinfo[1]);

            return CreateBoardFromText(fileName, count);
        }

        private static List<Sudoku> CreateBoardFromText(string fileName, int count, string boardName = "")
        {
            var boards = new List<Sudoku>();

            using (var reader = new StreamReader(fileName, Encoding.UTF8))
            {
                var name = "";
                var rows = new List<List<int>>();
                var line = "";
                var processPuzzle = boardName == "";

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.All(char.IsDigit))
                    {
                        if (processPuzzle)
                        {
                            var row = line.SplitIntoNumericParts(count).ToList<int>();
                            rows.Add(row);
                        }
                    }
                    else
                    {
                        Sudoku board = null;

                        if (boardName != "" && rows.Count > 0)
                        {
                            board = new Sudoku(boardName, rows);
                            rows = new List<List<int>>();
                        }
                        else if (name != "" && rows.Count > 0)
                        {
                            board = new Sudoku(name, rows);
                            rows = new List<List<int>>();
                        }

                        name = line;
                        if (board != null)
                        {
                            boards.Add(board);
                        }
                        if (boardName != "")
                            processPuzzle = boardName == name;
                    }
                }
                // add the last one
                if (name != "" && rows.Count > 0)
                    boards.Add(new Sudoku(name, rows));
            }

            return boards;
        }


        #endregion
    }
}