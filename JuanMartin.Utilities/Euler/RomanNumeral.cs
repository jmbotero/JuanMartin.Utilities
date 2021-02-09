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
        private readonly SortedDictionary<char, int> Denominations = new SortedDictionary<char, int>() { { 'I', 1 }, { 'V', 5 }, { 'X', 10 }, { 'L', 50 }, { 'C', 100 }, { 'D', 500 }, { 'M', 1000 } };
        private string _numeral;
        private List<int> _draft; // contain first traslation with values evaluating substractions
        public string Value
        {
            get { return _numeral; }
            set { this._numeral = value.ToUpper(); }
        }
        public RomanNumeral(string value = "")
        {
            Value = value;
            _draft = new List<int>();
        }

        /// <summary>
        /// Check if roman numeral specified is valid: follows rules layout in https://projecteuler.net/about=roman_numerals
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            int substraction_counter = 1;
            string[] xcm_reductions = new string[]
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

            _draft.Clear();

            // check rules:
            // rule_1. (valid_charachters): Roman numerals contain only I, V, X, L, C, D, M
            // rule_2. (descending_numerals): Numerals must be arranged in descending order of size.
            // rule_3. (mcx_reduction): M, C, and X cannot be equalled or exceeded by smaller denominations.
            // rule_4. (dlv_once): D, L, and V can each only appear once.
            var valid_charachters = Value.All(c => Denominations.ContainsKey(c));

            if (valid_charachters)
            {
                bool mcx_reduction = !xcm_reductions.Any(v => Value.Contains(v));

                if (mcx_reduction)
                {
                    bool dlv_once = Value.Count(c => c == 'D') <= 1 && Value.Count(c => c == 'L') <= 1 && Value.Count(c => c == 'V') <= 1;

                    if (dlv_once)
                    {
                        var descending_numerals = true;

                        for (var i = 0; i < Value.Length; i += 2)
                        {
                            var c = Value[i];
                            char next_c = (i == Value.Length - 1) ? '\0' : Value[i + 1];

                            // is a pair
                            if (next_c != '\0')
                            {
                                var value1 = Denominations[c];
                                var value2 = Denominations[next_c];

                                // is subtractive
                                if (value1 < value2)
                                {
                                    // a. Only one I, X, and C can be used as the leading numeral in part of a subtractive pair.
                                    // b. I can only be placed before V and X.
                                    // c. X can only be placed before L and C.
                                    // d. C can only be placed before D and M.
                                    var rule_a = "IXC".Contains(c);
                                    var rule_b = c == 'I' && "VX".Contains(next_c);
                                    var rule_c = c == 'X' && "LC".Contains(next_c);
                                    var rule_d = c == 'C' && "DM".Contains(next_c);
                                    bool substraction_rules = (rule_a || rule_b || rule_c || !rule_d);

                                    if (!substraction_rules)
                                    {
                                        descending_numerals = false;
                                        break;
                                    }

                                    _draft.Add(value2 - value1);
                                    substraction_counter++;
                                }
                                else
                                {
                                    _draft.Add(Denominations[c]);
                                    _draft.Add(Denominations[next_c]);
                                }
                            }
                            else
                            {
                                _draft.Add(Denominations[c]);
                            }
                        }

                        if (_draft.First() < _draft.Last())
                        {
                            descending_numerals = false;
                        }

                        if (descending_numerals)
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
            return (IsValid() && _draft.Count() > 1) ? _draft.Sum() : -1;
        }

        public string GetMinimalForm(int number = -1)
        {
            string roman = string.Empty;
            
            if  (number == -1)
                number = ToArabic();

            if (number == -1)
                return "";
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

            string[][] roman_numerals = new string[][]
            {
                new string[] {"","I","II","III","IV","V","VI","VII","VIII","IX"},
                new string[] {"","X","XX","XXX","XL","L","LX","LXX","LXXX","XC"},
                new string[] {"","C","CC","CCC","CD","D","DC","DCC","DCCC","CM" },
                new string[] {"","M","MM","MMM","|IV|","|V|","|VI|","|VII|","|VIII|","|IX|" }
            };

            var roman= roman_numerals[((power > 3) ? power-3 : power)][number];
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
