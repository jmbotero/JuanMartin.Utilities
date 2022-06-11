using JuanMartin.Kernel.Extesions;
using JuanMartin.Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JuanMartin.Utilities.Euler.Objects
{
    public class PrimeSet
    {
        public int Count;
        public HashSet<long>[] Pairs;
        public long[] Primes;
        public long[] Set;
        public int[] Minimums;

        public PrimeSet(int setSize, int primeUpperLimit)
        {                 
            Count = setSize;
            Primes = UtilityMath.ErathostenesSieve(primeUpperLimit, 3).ToArray();
            Minimums = Enumerable.Repeat(int.MaxValue, 5).ToArray();
            Pairs = new HashSet<long>[Primes.Length];
            Set = new long[Count];

            for (int a = 0; a < Primes.Length; a++)
            {
                Pairs[a] = new HashSet<long>();
                var x = Primes[a];

                for (int b = 0; b < Primes.Length; b++)
                {
                    if (a == b) continue;
                    var y = Primes[b];
                    if (UtilityMath.IsPrimeUsingSquares(UtilityMath.NumericConcat(x, y)) &&
                        UtilityMath.IsPrimeUsingSquares(UtilityMath.NumericConcat(y, x)))
                        Pairs[a].Add(y);
                }
            }
        }

        public bool IsPrimeSet()
        {
            bool isPrimeSet = true;

            var count = Set.InUse();
            for (var i = 0; i < count; i++)
            {
                for (var j = 0; j < count; j++)
                {
                    if (j == i) continue;
                    var index = Array.IndexOf(Primes, Set[i]);
                    isPrimeSet &= Pairs[index].Contains(Set[j]);
                    if (!isPrimeSet) break;
                }
                if (!isPrimeSet) break;
            }

            return isPrimeSet;
        }

        public void CalculatePrimeSetMinimumSum()
        {
            var minimumCalculated = 0; // Keep a count when a minium is calculated for a level
            int level; // from 1 to size, size-1 primes in set per level

            for (int a = 0; a < Primes.Length; a++)
            {
                level = 1;
                if (Count <= level - 1) break; // if got to size level stop processing
                if (minimumCalculated > level) break; // found minimum for this set size
                minimumCalculated = level - 1; // reset counter to search for minimum if set changed

                //set[level - 1] = Primes[a]; // add first
                //Set.ReSet(level - 1, default);
                Set.Assign(level - 1, Primes[a]);

                // calculate minimum for this level
                int sum = (int)Set.Sum();
                if (sum >= Minimums[4]) break; // if sum of set so far is more that target minimum no need to continue since we are looking for minimum

                if (sum < Minimums[0])
                {
                    Minimums[0] = sum;
                    minimumCalculated++;
                }

                for (int b = a + 1; b < Primes.Length; b++)
                {
                    level = 2;
                    if (Count <= level - 1) break; // if got to size level stop processing
                    if (minimumCalculated > level) break; // found minimum for this set size
                    minimumCalculated = level - 1; // reset counter to search for minimum if set changed

                    //set[level-1]= Primes[b]; // add second
                    //Set.ReSet(level-1, default);
                    Set.Assign(level - 1, Primes[b]);

                    if (Set.Sum() >= Minimums[4]) break; // if sum of set so far is more that target minimum no need to continue since we are looking for minimum

                    // find if set is prime

                    if (IsPrimeSet()) //if (IsPrimeSetQuery(Set))
                    {
                        // calculate minimum for this level
                        sum = (int)Set.Sum();
                        if (sum < Minimums[1])
                        {
                            Minimums[1] = sum;
                            minimumCalculated++;
                        }

                        if (Set.InUse() <= Count - 1) // should have more than two
                        {
                            for (int c = b + 1; c < Primes.Length; c++)
                            {
                                level = 3;
                                if (Count <= level - 1) break; // if got to size level stop processing
                                if (minimumCalculated > level) break; // found minimum for this set size
                                minimumCalculated = level - 1; // reset counter to search for minimum if set changed

                                //set[level - 1] = Primes[c]; // add third
                                //Set.ReSet(level - 1, default);
                                Set.Assign(level - 1, Primes[c]);

                                if (Set.Sum() >= Minimums[4]) break; // if sum of set so far is more that target minimum no need to continue since we are looking for minimum

                                if (IsPrimeSet()) //if (IsPrimeSetQuery(Set))
                                {
                                    // calculate minimum for this level
                                    sum = (int)Set.Sum();
                                    if (sum < Minimums[2])
                                    {
                                        Minimums[2] = sum;
                                        minimumCalculated++;
                                    }

                                    if (Set.InUse() <= Count - 1) // should have more than three
                                    {
                                        for (int d = c + 1; d < Primes.Length; d++)
                                        {
                                            level = 4;
                                            if (Count <= level - 1) break; // if got to size level stop processing
                                            if (minimumCalculated > level) break; // found minimum for this set size
                                            minimumCalculated = level - 1; // reset counter to search for minimum if set changed

                                            //set[level - 1] = Primes[d]; // add fourth
                                            //Set.ReSet(level - 1, default);
                                            Set.Assign(level - 1, Primes[d]);

                                            if (Set.Sum() >= Minimums[4]) break; // if sum of set so far is more that target minimum no need to continue since we are looking for minimum

                                            if (IsPrimeSet()) //if (IsPrimeSetQuery(Set))
                                            {
                                                // calculate minimum for previous level
                                                sum = (int)Set.Sum();
                                                if (sum < Minimums[3])
                                                {
                                                    Minimums[3] = sum;
                                                    minimumCalculated++;
                                                }

                                                if (Set.InUse() <= Count - 1) // should have more than four
                                                {
                                                    for (int e = d + 1; e < Primes.Length; e++)
                                                    {
                                                        level = 5;
                                                        if (Count <= level - 1) break; // if got to size level stop processing
                                                        if (minimumCalculated > level) break; // found minimum for this set size
                                                        minimumCalculated = level - 1; // reset counter to search for minimum if set changed

                                                        //set[level - 1] = Primes[e]; // add fifth
                                                        //Set.ReSet(level - 1, default);
                                                        Set.Assign(level - 1, Primes[e]);

                                                        if (Set.Sum() >= Minimums[4]) break; // if sum of set so far is more that target minimum no need to continue since we are looking for minimum

                                                        if (IsPrimeSet()) //if (IsPrimeSetQuery(Set))
                                                        {
                                                            // calculate minimum for this level
                                                            sum = (int)Set.Sum();

                                                            if (sum < Minimums[4])
                                                            {
                                                                Minimums[4] = sum;
                                                                minimumCalculated++;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                            }
                        }
                        else
                        {
                             break;
                        }
                    }
                }
            }

        }

        public long ExecuteMathBlogPrimePairSet()
        {
            long result = long.MaxValue;
            HashSet<long>[] pairs = new HashSet<long>[Primes.Length];

            for (int a = 1; a < Primes.Length; a++)
            {
                if (Primes[a] * 5 >= result) break;
                if (pairs[a] == null) pairs[a] = MakePairs(a);
                for (int b = a + 1; b < Primes.Length; b++)
                {
                    if (Primes[a] + Primes[b] * 4 >= result) break;
                    if (!pairs[a].Contains(Primes[b])) continue;
                    if (pairs[b] == null) pairs[b] = MakePairs(b);

                    for (int c = b + 1; c < Primes.Length; c++)
                    {
                        if (Primes[a] + Primes[b] + Primes[c] * 3 >= result) break;
                        if (!pairs[a].Contains(Primes[c]) ||
                            !pairs[b].Contains(Primes[c])) continue;
                        if (pairs[c] == null) pairs[c] = MakePairs(c);

                        for (int d = c + 1; d < Primes.Length; d++)
                        {
                            if (Primes[a] + Primes[b] + Primes[c] + Primes[d] * 2 >= result) break;
                            if (!pairs[a].Contains(Primes[d]) ||
                                !pairs[b].Contains(Primes[d]) ||
                                !pairs[c].Contains(Primes[d])) continue;
                            if (pairs[d] == null) pairs[d] = MakePairs(d);

                            for (int e = d + 1; e < Primes.Length; e++)
                            {
                                if (Primes[a] + Primes[b] + Primes[c] + Primes[d] + Primes[e] >= result) break;
                                if (!pairs[a].Contains(Primes[e]) ||
                                    !pairs[b].Contains(Primes[e]) ||
                                    !pairs[c].Contains(Primes[e]) ||
                                    !pairs[d].Contains(Primes[e])) continue;

                                if (result > Primes[a] + Primes[b] + Primes[c] + Primes[d] + Primes[e])
                                    result = Primes[a] + Primes[b] + Primes[c] + Primes[d] + Primes[e];

                                Console.WriteLine("{0} + {1} + {2} + {3} + {4} = {5}", Primes[a], Primes[b], Primes[c], Primes[d], Primes[e], result);
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        private HashSet<long> MakePairs(int a)
        {
            HashSet<long> pairs = new HashSet<long>();
            for (int b = a + 1; b < Primes.Length; b++)
            {
                if (UtilityMath.IsPrimeUsingSquares(UtilityMath.NumericConcat<long>(Primes[a], Primes[b])) &&
                    UtilityMath.IsPrimeUsingSquares(UtilityMath.NumericConcat<long>(Primes[b], Primes[a])))
                    pairs.Add(Primes[b]);
            }
            return pairs;
        }

        // Check if taking any two primes and concatenating them in any order the result is prime. 
        // For example, taking 7 and 109, both 7109 and 1097 are prime
        private readonly Func<int, int, bool> IsPrimePair = (x, y) =>
        {
            var IsPrime = UtilityMath.IsPrimeUsingSquares.Memoize();

            bool value1 = IsPrime(UtilityMath.NumericConcat(x, y));
            bool value2 = IsPrime(UtilityMath.NumericConcat(y, x));

            return value1 && value2;
        };

        // Check if every possible pair in a set of prime numbers are 'IsPrimePair'
#pragma warning disable IDE0051 // Remove unused private members
        private bool IsPrimeSetQuery(IEnumerable<int> s)
#pragma warning restore IDE0051 // Remove unused private members
        {
            //var isPrimePair = IsPrimePair.Memoize();
            var combinations = from item1 in s
                               from item2 in s
                               where item1 < item2 && (item1 * item2) != 0
                               select IsPrimePair(item1, item2);
            return (combinations.All(p => p));
        }

    }
}
