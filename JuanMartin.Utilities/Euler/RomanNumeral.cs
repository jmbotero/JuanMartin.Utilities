using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuanMartin.Utilities.Euler
{
    public class RomanNumeral
    {
        private readonly SortedDictionary<char, int> Denominations = new SortedDictionary<char, int>() { { 'I', 1 }, { 'V', 5 }, { 'X', 10 }, { 'L', 50 }, { 'C', 100 }, { 'D', 500 }, { 'M', 1000 } };
        private string _numeral;
        private SortedDictionary<string,int> _draft = new SortedDictionary<string, int>(); //contain first traslation evaluating substractions
        public string Value
        {
            get { return _numeral; }
            set { this._numeral = value.ToUpper(); }
        }
        public RomanNumeral(string value = "")
        {
            Value = value;
        }

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

            // check rules:
            // rule_1. (valid_charachters): Roman numerals contain only I, V, X, L, C, D, M
            // rule_2. (descending_numerals): Numerals must be arranged in descending order of size.
            // rule_3. (mcx_reduction): M, C, and X cannot be equalled or exceeded by smaller denominations.
            // rule_4. (dlv_once): D, L, and V can each only appear once.
            // from https://projecteuler.net/about=roman_numerals
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

                                    _draft.Add($"s{substraction_counter}", value2 - value1);
                                    substraction_counter++;
                                }
                                else
                                {
                                    _draft.Add(c.ToString(), Denominations[c]);
                                    _draft.Add(next_c.ToString(), Denominations[next_c]);
                                }
                            }
                            else
                            {
                                _draft.Add(c.ToString(), Denominations[c]);
                            }
                        }

                        if (_draft.Values.First() < _draft.Values.Last())
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

        public int ToArabic()
        {
            return (IsValid()) ? _draft.Values.Sum() : -1;
        }
    }
}
