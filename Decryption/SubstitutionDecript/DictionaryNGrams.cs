using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Decryption.SubstitutionDecript
{
    class DictionaryNGrams
    {
        private readonly int[] NGrams = { 1, 2, 3, 4, 5, 6 };
        private readonly char[] alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private DictionaryNGrams()
        {
            LoadDictionary();
        }
        private static readonly DictionaryNGrams _dictionaryNGrams = new DictionaryNGrams();

        public static DictionaryNGrams GetDictionaryNGrams()
        {
            return _dictionaryNGrams;
        }

        public Dictionary<string, double> NGramDict { get; set; }

        private void LoadDictionary()
        {
            NGramDict = File.ReadLines("../../../Decryption/SubstitutionDecript/dictionary/DictionaryNGrams.txt")
                .Select(line => line.Split(' '))
                .ToDictionary(line => line[0], line => Convert.ToDouble(line[1]));
        }

        private void CreateDictionary()
        {
            NGramDict = new Dictionary<string, double>();

            double sum = File.ReadLines("../../../Decryption/SubstitutionDecript/dictionary/WordsFrequency.txt")
                .Select(line => line.Split(' ')).Sum(line => Convert.ToDouble(line[1]));

            Dictionary<string, double> wordFrequency =
                File.ReadLines("../../../Decryption/SubstitutionDecript/dictionary/WordsFrequency.txt")
                .Select(line => line.Split(' '))
                .ToDictionary(line => line[0], line => Convert.ToDouble(line[1]) / sum);
            
            Dictionary<string, double> gramsSums = new Dictionary<string, double>();
            Dictionary<string, int> gramsCounts = new Dictionary<string, int>();
            int iw = 0;
            double count = 0.0;
            foreach (var pair in wordFrequency)
            {
                iw++;
                Math.DivRem(iw, 1000, out int result);

                if (result == 0)
                {
                    Console.WriteLine(iw);
                }

                foreach (int i in NGrams)
                {
                    for (int j = 0; j < (pair.Key.Length - i + 1); j++)
                    {
                        string key = pair.Key.Substring(j, i);
                        if(!key.All(c => alphabet.Contains(c)))
                        {
                            continue;
                        }
                        //count += pair.Value;
                        if (gramsSums.ContainsKey(key))
                        {
                            gramsSums[key] += pair.Value;
                        }
                        else
                        {
                            gramsSums[key] = pair.Value;
                        }
                    }
                }
            }
            foreach (var pair in gramsSums)
            {
                NGramDict[pair.Key] = pair.Value;
            }

            using (StreamWriter sw = new StreamWriter("../../../Decryption/SubstitutionDecript/dictionary/DictionaryNGrams.txt"))
            {
                foreach (var pair in NGramDict)
                {
                    sw.WriteLine(pair.Key + " " + pair.Value);
                }
            }
        }
    }
}
