using System;
using System.Collections.Generic;
using System.Linq;

namespace Decryption.SubstitutionDecript
{
    class CipherFitness4
    {
        private static readonly int[] NGrams = { 1, 2, 3, 4, 5, 6 };

        public static double Evaluate(string decryptedText)
        {
            decryptedText = decryptedText.ToLower();
            double uniStat = UniGramFitness(decryptedText);
            double biStat = BiGramFitness(decryptedText);
            double triStat = TriGramFitness(decryptedText);
            //double quadStat = QuadGramFitness(decryptedText);
            double dictStat = dictionaryStatisticFitness(decryptedText);

            return 30 / (uniStat + biStat + triStat) + dictStat;
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


            //double dictStat = dictionaryStatisticFitness(decryptedText);

            //Console.WriteLine("uni: " + uni.ToString("F3") + " bi: " + bi.ToString("F3") +
            //    " tri: " + tri.ToString("F3") + " quad: " + quad.ToString("F3") + " penta: "+ penta.ToString("F3") + " dict: " + dictStat.ToString("F3"));
            return (uni + bi + tri + quad + penta) / decryptedText.Length * 1000 /*+ dictStat*/;
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


            double dictStat = dictionaryStatisticFitness(decryptedText);
             
            Console.WriteLine("all: " + ((uni + bi + tri + quad + penta) / decryptedText.Length * 1000 + dictStat) + " uni: " + (uni / decryptedText.Length * 1000).ToString("F3") + " bi: " + (bi / decryptedText.Length * 1000).ToString("F3") +
                " tri: " + (tri / decryptedText.Length * 1000).ToString("F3") + " quad: " + (quad / decryptedText.Length * 1000).ToString("F3") + " penta: " + (penta / decryptedText.Length * 1000).ToString("F3") + " dict: " + dictStat.ToString("F3"));
            //return uni + bi + tri + quad + penta;
        }

        public static void Show(string decryptedText)
        {
            decryptedText = decryptedText.ToLower();
            double uniStat = UniGramFitness(decryptedText);
            double biStat = BiGramFitness(decryptedText);
            double triStat = TriGramFitness(decryptedText);
            //double quadStat = QuadGramFitness(decryptedText);
            double dictStat = dictionaryStatisticFitness(decryptedText);

            double stat = 0.0;
            double stat1 = 0.0;
            stat = 30 / (uniStat + biStat + triStat) + dictStat;
            stat1 = 30 / (uniStat + biStat + triStat);
            Console.WriteLine("sum: " + stat + " lang: " + stat1 + " dict: " + dictStat);
        }

        private static double UniGramFitness(string decryptedText)
        {
            var statisticHelper = LanguageStatisticsHelper.GetLanguageStatistics();
            var cipherUD = statisticHelper.CreateUniGramStatistic(decryptedText);
            var nativUD = statisticHelper.UniGramDict;

            double uniGramProb = 0.0;

            foreach(var pair in nativUD)
            {
                cipherUD.TryGetValue(pair.Key, out double probCipher);
                uniGramProb += Math.Pow(Math.Abs(pair.Value - probCipher), 2);
            }

            return uniGramProb;
        }

        private static double BiGramFitness(string decryptedText)
        {
            var statisticHelper = LanguageStatisticsHelper.GetLanguageStatistics();
            var cipherBD = statisticHelper.CreateBiGramStatistic(decryptedText);
            var nativBD = statisticHelper.BiGramDict;

            double biGramProb = 0.0;

            foreach (var pair in nativBD)
            {
                cipherBD.TryGetValue(pair.Key, out double probCipher);
                biGramProb += Math.Pow(Math.Abs(pair.Value - probCipher), 2);
            }

            return biGramProb;
        }

        private static double TriGramFitness(string decryptedText)
        {
            var statisticHelper = LanguageStatisticsHelper.GetLanguageStatistics();
            var cipherTD = statisticHelper.CreateTriGramStatistic(decryptedText);
            var nativTD = statisticHelper.TriGramDict;

            double triGramProb = 0.0;

            foreach (var pair in nativTD)
            {
                cipherTD.TryGetValue(pair.Key, out double probCipher);
                triGramProb += Math.Pow(Math.Abs(pair.Value - probCipher), 2);
            }

            return triGramProb;
        }

        private static double QuadGramFitness(string decryptedText)
        {
            var statisticHelper = LanguageStatisticsHelper.GetLanguageStatistics();
            var cipherQD = statisticHelper.CreateQuadGramStatistic(decryptedText);
            var nativQD = statisticHelper.QuadGramDict;

            double quadGramProb = 0.0;

            foreach (var pair in nativQD)
            {
                cipherQD.TryGetValue(pair.Key, out double probCipher);
                quadGramProb += Math.Pow(Math.Abs(pair.Value - probCipher), 2);
            }

            return quadGramProb;
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
