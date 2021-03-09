
        //*********************************************************************************************************
        // © 2013 jakemdrew.com. All rights reserved. 
        // This source code is licensed under The GNU General Public License (GPLv3):  
        // http://opensource.org/licenses/gpl-3.0.html
        //*********************************************************************************************************

        //*********************************************************************************************************
        //makeNgrams - Example n-gram creator.
        //Created By - Jake Drew 
        //Version -    1.0, 04/22/2013
        //*********************************************************************************************************
        public static IEnumerable<string> MakeNgrams(string text, int nGramSize)
        {
            if (nGramSize == 0) throw new Exception("nGram size was not set");

            var nGram = new StringBuilder();
            var wordLengths = new System.Collections.Generic.Queue<int>();

            var wordCount = 0;
            var lastWordLen = 0;

            //append the first character, if valid.
            //avoids if statement for each for loop to check i==0 for before and after vars.
            if (text != "" && char.IsLetterOrDigit(text[0]))
            {
                nGram.Append(text[0]);
                lastWordLen++;
            }

            //generate ngrams
            for (int i = 1; i < text.Length - 1; i++)
            {
                var before = text[i - 1];
                var after = text[i + 1];

                if (char.IsLetterOrDigit(text[i])
                        ||
                        //keep all punctuation that is surrounded by letters or numbers on both sides.
                        (text[i] != ' '
                        && (char.IsSeparator(text[i]) || char.IsPunctuation(text[i]))
                        && (char.IsLetterOrDigit(before) && char.IsLetterOrDigit(after))
                        )
                    )
                {
                    nGram.Append(text[i]);
                    lastWordLen++;
                }
                else
                {
                    if (lastWordLen > 0)
                    {
                        wordLengths.Enqueue(lastWordLen);
                        lastWordLen = 0;
                        wordCount++;

                        if (wordCount >= nGramSize)
                        {
                            yield return nGram.ToString();
                            nGram.Remove(0, wordLengths.Dequeue() + 1);
                            wordCount -= 1;
                        }

                        nGram.Append(" ");
                    }
                }
            }
            nGram.Append(text.Last());
            yield return nGram.ToString();
        }