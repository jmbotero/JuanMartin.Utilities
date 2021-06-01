using System;
using System.Collections.Generic;
using System.Linq;
using JuanMartin.Kernel.Extesions;
using System.Text;
using System.Threading.Tasks;
using JuanMartin.Kernel.Utilities;

namespace JuanMartin.Utilities.Euler
{
    public class RomanNumeral
    {
        private const int MAX_ROMAN_NUMERAL_POWER = 7;
        private readonly SortedDictionary<char, int> _denominations = new SortedDictionary<char, int>() { { 'I', 1 }, { 'V', 5 }, { 'X', 10 }, { 'L', 50 }, { 'C', 100 }, { 'D', 500 }, { 'M', 1000 } };
        private readonly string[] _baseSymbolReplacements = new string[]
        {   new String('I',10),
                new String('V', 2),
                new String('I', 100),
                new String('v', 20),
                new String('X', 10),
                new String('L', 2),
                new String('I', 1000),
                new String('V', 200),
                new String('X', 100),
                new String('L', 20),
                new String('C', 10),
                new String('D', 2)
        };
        private string _numeral;
        private readonly List<string> _parsedNumeral;
        private readonly List<int> _draft; // contain first traslation with values evaluating substractions
        public string Value
        {
            get { return _numeral; }
            set { this._numeral = value.ToUpper(); }
        }

        public int ArabicValue { get; private set; }
        public RomanNumeral(string value = "")
        {
            Value = value;
            ArabicValue = -1;
            _draft = new List<int>();
            _parsedNumeral = new List<string>();
        }

        private bool Parse()
        {
            _draft.Clear();
            _parsedNumeral.Clear();

            var invalidNumeral = false;

            if (Value != null && Value.Length > 0)
            {
                for (var i = 0; i < Value.Length; i++)
                {
                    bool added = false;
                    char n = Value[i];

                    int valueLeft;
                    try
                    {
                        valueLeft = _denominations[n];
                    }
                    catch
                    {
                        invalidNumeral = true;
                        break;
                    }

                    // there is a contiguous charhacter
                    if (i + 1 < Value.Length)
                    {
                        char nextN = Value[i + 1];
                        int valueRight;
                        try
                        {
                            valueRight = _denominations[nextN];
                        }
                        catch
                        {
                            invalidNumeral = true;
                            break;
                        }

                        // pair is a substraction
                        if (valueLeft < valueRight)
                        {
                            // a. Only one I, X, and C can be used as the leading numeral in part of a subtractive pair.
                            // b. I can only be placed before V and X.
                            // c. X can only be placed before L and C.
                            // d. C can only be placed before D and M.
                            var ruleA = "IXC".Contains(n);
                            var ruleB = n == 'I' && !"VX".Contains(nextN);
                            var ruleC = n == 'X' && !"LC".Contains(nextN);
                            var ruleD = n == 'C' && !"DM".Contains(nextN);

                            if (!ruleA)
                            {
                                invalidNumeral = true;
                                break;
                            }
                            else if (ruleB || ruleC || ruleD)
                            {
                                invalidNumeral = true;
                                break;
                            }

                            _parsedNumeral.Add(n.ToString() + nextN.ToString());
                            _draft.Add(valueRight - valueLeft);
                            added = true;
                            i++; // move to next numeral
                        }
                    }
                 
                    if (!added)
                    {
                        _parsedNumeral.Add(n.ToString());
                        _draft.Add(valueLeft);
                    }
                }
            }
            return !invalidNumeral;
        }

        /// <summary>
        /// Check if roman numeral specified is valid: follows rules layout in https://projecteuler.net/about=roman_numerals
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            // check rules:
            // rule_1. (validCharachters): Roman numerals contain only I, V, X, L, C, D, M
            // rule_2. (descendingNumerals): Numerals must be arranged in descending order of size.
            // rule_3. (mcxReduction): M, C, and X cannot be equalled or exceeded by smaller denominations.
            // rule_4. (dlvOnce): D, L, and V can each only appear once.
            var validCharachters = Value.All(c => _denominations.ContainsKey(c));

            if (validCharachters)
            {
                bool mcxReduction = !_baseSymbolReplacements.Any(v => Value.Contains(v));

                if (mcxReduction)
                {
                    bool dlvOnce = Value.Count(c => c == 'D') <= 1 && Value.Count(c => c == 'L') <= 1 && Value.Count(c => c == 'V') <= 1;

                    if (dlvOnce)
                    {
                        var descendingNumerals = Parse();

                        if (_draft.Count > 0 && (_draft.First() < _draft.Last()))
                        {
                            descendingNumerals = false;
                        }

                        if (descendingNumerals)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// If roman number specified in value is valid return its arabic number representation, if number is not valid return -1
        /// </summary>
        /// <returns></returns>
        public int ToArabic()
        {
            ArabicValue = (IsValid() && _draft.Count() > 0) ? _draft.Sum() : -1;

            return ArabicValue;
        }

        public string GetMinimalForm(int number = -1)
        {
            string roman = string.Empty;
            
            // if no number is specified as an argument  use the arabic conversion of 'this' roman numeral
            if  (number == -1)
                number = ToArabic();

            if (number < 0)
                throw new ArgumentException($"Cannot calculate roman representation for negative numbers.");
            else
            {
                if (UtilityMath.GetPowerOfTen(number) > MAX_ROMAN_NUMERAL_POWER)
                    throw new ArgumentException($"Cannot calculate roman representation for numbers in the  order of 10^{UtilityMath.GetPowerOfTen(number) + 1}.");

                var digits = ConvertToIntArray(number);

                for (var i = 0; i < ((digits.Length >= MAX_ROMAN_NUMERAL_POWER) ? MAX_ROMAN_NUMERAL_POWER + 1: digits.Length); i++)
                {
                    roman = IntToRoman(digits[i], i) + roman;
                }
                roman = roman.Replace("||", "");
            }

            return roman;
        }

        private string IntToRoman(int number, int power)
        {
            if (power > MAX_ROMAN_NUMERAL_POWER)
                throw new ArgumentException($"Cannot calculate roman representation for numbers in the  order of 10^{power + 1}.");

            string[][] romanNumerals = new string[][]
            {
                new string[] {"","I","II","III","IV","V","VI","VII","VIII","IX"},
                new string[] {"","X","XX","XXX","XL","L","LX","LXX","LXXX","XC"},
                new string[] {"","C","CC","CCC","CD","D","DC","DCC","DCCC","CM" },
                new string[] {"","M","MM","MMM","MMMM","|V|","|VI|","|VII|","|VIII|","|IX|" }
            };

            // surround with || to indicate x1000
            var roman= romanNumerals[((power > 3) ? power-3 : power)][number];
            return (power.Between(4, 7)) ? $"|{roman}|":roman;
        }

        private static int[] ConvertToIntArray(int num)
        {
            List<int> listOfInts = new List<int>();
            while (num > 0)
            {
                listOfInts.Add(num % 10);
                num /= 10;
            }
            //listOfInts.Reverse();
            return listOfInts.ToArray();
        }

    }
}
