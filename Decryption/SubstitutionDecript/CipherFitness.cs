using System;
using System.Collections.Generic;
using System.Linq;
using Decryption.Proto4Task;

namespace Decryption.SubstitutionDecript
{
    class CipherFitness
    {
        private static readonly int[] NGrams = { 1, 2, 3, 4, 5, 6 };

        public static double NewNew(string decryptedText)
        {
            double score = 0.0;
            double length = decryptedText.Length;
            var helper = DictionaryNGrams.GetDictionaryNGrams();
            for (int i = 2; i < 5; i++)
            {
                for (int j = i; j < decryptedText.Length; j++)
                {
                    string word = decryptedText.Substring(j - i, i);
                    helper.Dictionary.TryGetValue(word, out double value);
                    score += value;

                }
            }

            return 200 * score / length;
        }
        public static double New(string decryptedText)
        {
            double bi = 0.0;
            double tri = 0.0;
            double quad = 0.0;

            var newDict = NewDictionary.GetNewDictionary();
            var biDict = newDict.BiDict;
            var triDict = newDict.TriDict;
            var quadDict = newDict.QuadDict;

            decryptedText = decryptedText.ToLower();
            List<string> words = WordNinja.WordNinja.Split(decryptedText);

            foreach (var word in words)
            {
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
            }

            return (bi + tri + quad) / decryptedText.Length * 100000;
        }
        public static double NewEvaluate(string decryptedText)
        {
            decryptedText = decryptedText.ToLower();

            var statisticHelper = LanguageStatisticsHelper.GetLanguageStatistics();
            var uniDict = statisticHelper.UniGramDict;
            var biDict = statisticHelper.BiGramDict;
            var triDict = statisticHelper.TriGramDict;
            var quadDict = statisticHelper.QuadGramDict;
            var pentaDict = statisticHelper.PentaGramDict;

            double uni = 0.0;
            double bi = 0.0;
            double tri = 0.0;
            double quad = 0.0;
            double penta = 0.0;

            foreach (char c in decryptedText)
            {
                uniDict.TryGetValue(c, out double prob);
                uni += prob;
            }

            for (int j = 0; j < (decryptedText.Length - 1); j++)
            {
                string key = decryptedText.Substring(j, 2);
                biDict.TryGetValue(key, out double prob);
                bi += prob * 4;
            }

            for (int j = 0; j < (decryptedText.Length - 2); j++)
            {
                string key = decryptedText.Substring(j, 3);
                triDict.TryGetValue(key, out double prob);
                tri += prob * 9;
            }

            for (int j = 0; j < (decryptedText.Length - 3); j++)
            {
                string key = decryptedText.Substring(j, 4);
                quadDict.TryGetValue(key, out double prob);
                quad += prob * 16;
            }
            for (int j = 0; j < (decryptedText.Length - 4); j++)
            {
                string key = decryptedText.Substring(j, 5);
                pentaDict.TryGetValue(key, out double prob);
                penta += prob * 25;
            }

            return (uni + bi + tri + quad + penta) / decryptedText.Length * 1000;
        }

        public static void NewEvaluateShow(string decryptedText)
        {
            decryptedText = decryptedText.ToLower();

            var statisticHelper = LanguageStatisticsHelper.GetLanguageStatistics();
            var uniDict = statisticHelper.UniGramDict;
            var biDict = statisticHelper.BiGramDict;
            var triDict = statisticHelper.TriGramDict;
            var quadDict = statisticHelper.QuadGramDict;
            var pentaDict = statisticHelper.PentaGramDict;

            double uni = 0.0;
            double bi = 0.0;
            double tri = 0.0;
            double quad = 0.0;
            double penta = 0.0;

            foreach (char c in decryptedText)
            {
                uniDict.TryGetValue(c, out double prob);
                uni += prob;
            }

            for (int j = 0; j < (decryptedText.Length - 1); j++)
            {
                string key = decryptedText.Substring(j, 2);
                biDict.TryGetValue(key, out double prob);
                bi += prob * 4;
            }

            for (int j = 0; j < (decryptedText.Length - 2); j++)
            {
                string key = decryptedText.Substring(j, 3);
                triDict.TryGetValue(key, out double prob);
                tri += prob * 9;
            }

            for (int j = 0; j < (decryptedText.Length - 3); j++)
            {
                string key = decryptedText.Substring(j, 4);
                quadDict.TryGetValue(key, out double prob);
                quad += prob * 16;
            }
            for (int j = 0; j < (decryptedText.Length - 4); j++)
            {
                string key = decryptedText.Substring(j, 5);
                pentaDict.TryGetValue(key, out double prob);
                penta += prob * 25;
            }

            Console.WriteLine("all: " + ((uni + bi + tri + quad + penta) / decryptedText.Length * 1000 /*+ dictStat / 100*/) + " uni: " + 
                (uni / decryptedText.Length * 1000).ToString("F3") + " bi: " + (bi / decryptedText.Length * 1000).ToString("F3") +
                " tri: " + (tri / decryptedText.Length * 1000).ToString("F3") + " quad: " + 
                (quad / decryptedText.Length * 1000).ToString("F3") + " penta: " + 
                (penta / decryptedText.Length * 1000).ToString("F3"));
        }

        public static double Evaluate(string decryptedText)
        {
            decryptedText = decryptedText.ToLower();
            double langStat = languageStatisticFitness(decryptedText);
            double dictStat = dictionaryStatisticFitness(decryptedText);
            if ((30 / langStat) < 1200.0)
            {
                return 30 / langStat;
            }
            return 30 / langStat + dictStat;
        }

        public static void Show(string decryptedText)
        {
            decryptedText = decryptedText.ToLower();
            double langStat = languageStatisticFitness(decryptedText);
            double dictStat = dictionaryStatisticFitness(decryptedText);

            double stat = 0.0;
            if ((30 / langStat) < 1200.0)
            {
                stat = 30 / langStat;
            }
            else
            {
                stat = 30 / langStat + dictStat;
            }
            Console.WriteLine("sum: " + stat + " lang: " + (30 / langStat) + " dict: " + dictStat);
        }
        
        private static double languageStatisticFitness(string decryptedText)
        {
            var statisticHelper = LanguageStatisticsHelper.GetLanguageStatistics();
            var cipherUD = statisticHelper.CreateUniGramStatistic(decryptedText);
            var cipherBD = statisticHelper.CreateBiGramStatistic(decryptedText);
            var cipherTD = statisticHelper.CreateTriGramStatistic(decryptedText);
            var nativUD = statisticHelper.UniGramDict;
            var nativBD = statisticHelper.BiGramDict;
            var nativTD = statisticHelper.TriGramDict;

            double uniGramProb = 0.0;
            double biGramProb = 0.0;
            double triGramProb = 0.0;

            for (char c = 'a'; c <= 'z'; c++)
            {
                double cNativ;
                double cCipher;
                nativUD.TryGetValue(c, out cNativ);
                cipherUD.TryGetValue(c, out cCipher);

                uniGramProb += Math.Pow(Math.Abs(cNativ - cCipher), 2);

                //bigram 
                for (char c1 = 'a'; c1 <= 'z'; c1++)
                {
                    double cNativA;
                    double cCipherA;
                    char[] blockC = { c, c1 };
                    string blockS = new string(blockC);
                    nativBD.TryGetValue(blockS, out cNativA);
                    cipherBD.TryGetValue(blockS, out cCipherA);

                    biGramProb += Math.Pow(Math.Abs(cNativA - cCipherA), 2);

                    //trigram
                    for (char c2 = 'a'; c2 <= 'z'; c2++)
                    {
                        double cNativB;
                        double cCipherB;
                        char[] blockD = { c, c1, c2 };
                        string blockF = new string(blockD);
                        nativTD.TryGetValue(blockF, out cNativB);
                        cipherTD.TryGetValue(blockF, out cCipherB);

                        triGramProb += Math.Pow(Math.Abs(cNativB - cCipherB), 2);
                    }
                }
            }
            
            return uniGramProb + biGramProb + triGramProb;
        }

        public static double dictionaryStatisticFitness(string decryptedText)
        {
            double score = 0.0;
            double length = decryptedText.Length;
            var helper = DictionaryStatisticsHelper.GetDictionaryStatistics();
            for (int i = 3; i < 10; i++)
            {
                for (int j = i; j < decryptedText.Length; j++)
                {
                    string word = decryptedText.Substring(j - i, i);
                    if (helper.Dictionary.Contains(word))
                    {
                        score += Math.Pow(i, 3);
                    }
                }
            }

            return 200 * score / length;
        }
    }
}
