using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decryption.SubstitutionDecript
{
    public class NGramFrequency
    {
        public int length { set; get; }
        public String text { set;  get; }
        public Dictionary<string, int> occurrences;

        public NGramFrequency(int length, String text)
        {
            this.length = length;
            this.text = text;
        }

        public int FrequencyOf(String nGram)
        {
            int nGramOccurrence;
            if (this.occurrences.TryGetValue(nGram, out nGramOccurrence)) return nGramOccurrence;
            else return 0; 
        }
        

        public void Analyse()
        {
            LinkedList<char> currentNGram = new LinkedList<char>();
            foreach (char ch in text)
            {
                currentNGram.AddLast(ch);
                    if (currentNGram.Count == length)
                    {
                        StringBuilder nGram = new StringBuilder(length);
                        foreach (char value in currentNGram)
                        {
                            nGram.Append(value);
                        }
                        string nGramString = nGram.ToString();

                        int currentOccurrences;
                        if (occurrences.TryGetValue(nGramString, out currentOccurrences))
                        {
                            occurrences[nGramString] = currentOccurrences + 1;
                        }
                        else
                        {
                            occurrences[nGramString] = 1;
                        }
                        currentNGram.RemoveFirst();
                    }
            }
        }

    }
}
