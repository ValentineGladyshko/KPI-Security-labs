using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Decryption.SubstitutionDecript
{
    public class WordDictionary
    {
        private WordDictionary()
        {
            LoadDictionary();
        }
        private static readonly WordDictionary _wordDictionary = new WordDictionary();

        public static WordDictionary GetWordDictionary()
        {
            return _wordDictionary;
        }

        public Dictionary<string, double> Dictionary { get; set; }

        private void LoadDictionary()
        {
            Dictionary = File.ReadLines("../../../Decryption/SubstitutionDecript/dictionary/WordDictionary.txt")
                .Select(line => line.Split(' '))
                .ToDictionary(line => line[0].ToLower(), line => Convert.ToDouble(line[1]));
        }
        private void CreateDictionary()
        {
            List<string> words = File.ReadLines("../../../Decryption/SubstitutionDecript/dictionary/Words.txt")
               .Select(line => line.Split(' '))
               .Select(line => line[0].ToLower()).ToList();

            int iw = 0;

            Dictionary = new Dictionary<string, double>();

            SubstitutionDecrypt sb = new SubstitutionDecrypt("abcdefghijklmnopqrstuvwxyz");
            foreach (var wordToChange in words)
            {
                var word = sb.DecryptText(wordToChange);
                iw++;
                Math.DivRem(iw, 1000, out int result);
                if (result == 0)
                {
                    Console.WriteLine(iw);
                }

                double bi = 0.0;
                double tri = 0.0;
                double quad = 0.0;

                var newDict = NewDictionary.GetNewDictionary();
                var biDict = newDict.BiDict;
                var triDict = newDict.TriDict;
                var quadDict = newDict.QuadDict;

                for (int j = 0; j < (word.Length - 1); j++)
                {
                    string key = word.Substring(j, 2);

                    if (biDict.TryGetValue(key, out Dictionary<int, double> dict))
                    {
                        dict.TryGetValue(word.Length, out double value);
                        bi += value;
                    }
                }

                for (int j = 0; j < (word.Length - 2); j++)
                {
                    string key = word.Substring(j, 3);

                    if (triDict.TryGetValue(key, out Dictionary<int, double> dict))
                    {
                        dict.TryGetValue(word.Length, out double value);
                        tri += value;
                    }
                }

                for (int j = 0; j < (word.Length - 3); j++)
                {
                    string key = word.Substring(j, 4);

                    if (quadDict.TryGetValue(key, out Dictionary<int, double> dict))
                    {
                        dict.TryGetValue(word.Length, out double value);
                        quad += value;
                    }
                }

                double sum = bi + tri + quad;
                if (sum != 0.0)
                {
                    Dictionary[word] = sum * Math.Pow(word.Length, 2);
                }

            }

            using (StreamWriter sw = new StreamWriter("../../../Decryption/SubstitutionDecript/dictionary/WordDictionary.txt"))
            {
                foreach (var pair in Dictionary.OrderByDescending(pair => pair.Value))
                {
                    sw.WriteLine(pair.Key + " " + pair.Value);
                }
            }
        }
    }
}
