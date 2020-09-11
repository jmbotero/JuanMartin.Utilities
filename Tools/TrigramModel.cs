using System;
using System.Collections;
using System.Collections.Generic;

namespace JuanMartin.Utilities.Tools
{
    public class TrigramModel
    {
        Trigram[] trigrams;
        public Trigram[] Trigrams
        {
            get { return trigrams; }
        }

        public TrigramModel(Hashtable trigramsAndCounts)
        {
            var keys2 = new List<string>();
            var scores2 = new List<int>();
            //convert hashtable to arrays
            foreach (string key in trigramsAndCounts.Keys)
            {
                keys2.Add(key);
                scores2.Add((int)trigramsAndCounts[key]);
            }

            var keys = keys2.ToArray();
            var scores = scores2.ToArray();

            // sort array results
            Array.Sort(scores, keys);
            Array.Reverse(keys);
            Array.Reverse(scores);

            //build final array
            var result = new List<Trigram>();
            for (int x = 0; x < keys.Length; x++)
            {
                result.Add(new Trigram(keys[x], scores[x]));
            }

            trigrams = result.ToArray();

        }

        public TrigramModel(string[] tgrams)
        {
            var keys2 = new List<string>();
            var scores2 = new List<int>();
            //convert hashtable to arrays
            int score = 0;
            foreach (string key in tgrams)
            {
                keys2.Add(key);
                scores2.Add(score++);
            }

            var keys = keys2.ToArray();
            var scores = scores2.ToArray();

            // sort array results
            Array.Sort(scores, keys);
            Array.Reverse(keys);
            Array.Reverse(scores);

            //build final array
            var result = new List<Trigram>();
            for (int x = 0; x < keys.Length; x++)
            {
                result.Add(new Trigram(keys[x], scores[x]));
            }

            trigrams = result.ToArray();

        }

        public bool HasTrigram(string trigram)
        {
            foreach (Trigram t in trigrams)
            {
                if (t.Token == trigram) return true;
            }
            return false;
        }

        public int GetScore(string trigram)
        {
            foreach (Trigram t in trigrams)
            {
                if (t.Token == trigram) return t.Score;
            }
            throw new Exception("No score found for '" + trigram + "'");
        }
    }

    public class Trigram
    {
        public string _token = null;
        public int _score = 0;

        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }

        public int Score
        {
            get { return _score; }
            private set { _score = value; }
        }

        public Trigram()
        {
            _token = null;
            _score = 0;
        }
        public Trigram(string t, int s)
        {
            Token = t;
            Score = s;
        }
    }
}
