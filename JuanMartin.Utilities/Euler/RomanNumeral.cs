using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuanMartin.Utilities.Euler
{
    public class RomanNumeral
    {
        private readonly SortedDictionary<char, int> Denominations = new SortedDictionary<char, int>() { { 'I', 1 }, { 'V', 2 }, { 'X', 10 }, { 'L', 50 }, { 'C', 100 }, { 'D', 500 }, { 'M', 1000 } };
        private string _numeral;
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
            // check rules:
            // 1. Numerals must be arranged in descending order of size.
            // 2. M, C, and X cannot be equalled or exceeded by smaller denominations.
            // 3. D, L, and V can each only appear once.
            // from https://projecteuler.net/about=roman_numerals
            
            var valid_charachters = Value.All(c => Denominations.ContainsKey(c));
            var descending_numerals = true;

            var keys = Denominations.Keys.ToList();
            foreach(var c in Value)
            {
                var current_index = keys.IndexOf(c);
                char next_c = (current_index <= Value.Length - 1) ? keys[current_index + 1] : '\0';

                // is a pair
                if(next_c != '\0')
                {
                    var value1 = Denominations[c];
                    var value2 = Denominations[next_c];

                    // is subtractive
                    if(value2 < value1)
                    {
                        // 1. Only one I, X, and C can be used as the leading numeral in part of a subtractive pair.
                        // 2. I can only be placed before V and X.
                        // 3. X can only be placed before L and C.
                        // 4. C can only be placed before D and M.
                        if (!"IXC".Contains(c) || (c == 'I' && !"VX".Contains(next_c)) || (c == 'X' && !"LC".Contains(next_c)) || (c == 'C' && !"DM".Contains(next_c)))
                        {
                            descending_numerals = false;
                            break;
                        }
                    }
                    else if(value1 == value2 && !"IVDL".Contains(c))
                    {
                        descending_numerals = false;
                        break;
                    }
                }
            }
            bool dlv_once = Value.Count(c => c == 'D') > 1 && Value.Count(c => c == 'L') > 1 && Value.Count(c => c == 'V') > 1;

            return valid_charachters && descending_numerals && dlv_once; 
        }

        public int ToArabic()
        {
            throw new NotImplementedException();
        }
    }
}
