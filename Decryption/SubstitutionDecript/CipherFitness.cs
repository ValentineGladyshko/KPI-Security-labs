using System;
using System.Collections.Generic;
using System.Linq;

namespace Decryption.SubstitutionDecript
{
    class CipherFitness
    {
        private static readonly int[] NGrams = { 1, 2, 3, 4, 5, 6 };

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
            //return langStat + dictStat;
        }

        public static void Show(string decryptedText)
        {
            decryptedText = decryptedText.ToLower();
            double langStat = languageStatisticFitness(decryptedText);
            double dictStat = dictionaryStatisticFitness(decryptedText);
            //Console.WriteLine("lang: " + langStat+ " dict: " + +dictStat);

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
        //public static double Fitness(string decryptedText)
        //{
        //    double score = 0.0;
        //    var dict = DictionaryNGrams.GetDictionaryNGrams().NGramDict;
        //    foreach (int i in NGrams)
        //    {
        //        for (int j = 0; j < (decryptedText.Length - i + 1); j++)
        //        {
        //            dict.TryGetValue(decryptedText.Substring(j, i), out double curScore);
        //            score += 0.3 * Math.Pow(Math.Pow(i, 6) * curScore, 1.5);
        //        }
        //    }
        //    //decryptedText = decryptedText.ToLower();
        //    //double sum = 0.0;
        //    //Dictionary<string, double> curNGram = new Dictionary<string, double>();
        //    //Dictionary<string, double> curNGram1 = new Dictionary<string, double>();
        //    //foreach (int i in NGrams)
        //    //{
        //    //    for (int j = 0; j < (decryptedText.Length - i + 1); j++)
        //    //    {
        //    //        sum += 1;
        //    //        string key = decryptedText.Substring(j, i);
        //    //        if (curNGram.ContainsKey(key))
        //    //        {
        //    //            curNGram[key] += 1;
        //    //        }
        //    //        else
        //    //        {
        //    //            curNGram[key] = 1;
        //    //        }
        //    //    }
        //    //}

        //    //foreach(var pair in curNGram)
        //    //{
        //    //    curNGram1[pair.Key] = pair.Value / sum;
        //    //}
        //    //double sum1 = 0.0;
        //    //foreach (var pair in curNGram1.Values)
        //    //{
        //    //    sum1 += pair;
        //    //}
        //    //Console.WriteLine(sum1);
        //    //var dict = DictionaryNGrams.GetDictionaryNGrams().NGramDict;
        //    //foreach (var pair in curNGram)
        //    //{
        //    //    dict.TryGetValue(pair.Key, out double curScore);

        //    //}
        //    double score1 = 0.0;
        //    if (score > 1000)
        //    {
        //        double score2 = 0.0;
        //        double length = decryptedText.Length;
        //        var helper = DictionaryStatisticsHelper.GetDictionaryStatistics();
        //        for (int i = 4; i < 10; i++)
        //        {
        //            for (int j = i; j < decryptedText.Length; j++)
        //            {
        //                string word = decryptedText.Substring(j - i, i);
        //                if (helper.Dictionary.Contains(word))
        //                {
        //                    score2 += Math.Pow(i, 1.5);
        //                }
        //            }
        //        }
        //        score1 = 4000.0 * score2 / length;

        //    }
        //    return score + score1;
        //}

        private static double languageStatisticFitness(string decryptedText)
        {
            var statisticHelper = LanguageStatisticsHelper.GetLanguageStatistics();
            var cipherUD = statisticHelper.CreateUniGramStatistic(decryptedText);
            var cipherBD = statisticHelper.CreateBiGramStatistic(decryptedText);
            var cipherTD = statisticHelper.CreateTriGramStatistic(decryptedText);
            var nativUD = statisticHelper.UniGramDict;
            var nativBD = statisticHelper.BiGramDict;
            var nativTD = statisticHelper.TriGramDict;

            double alpha = 1.0;
            double beta = 1.0;
            double gamma = 1.0;

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
            //for (int j = 2; j < decryptedText.Length; j++)
            //{
            //    string word = decryptedText.Substring(j - 2, 2);

            //    double cNativA;
            //    double cCipherA;
            //    nativBD.TryGetValue(word, out cNativA);
            //    cipherBD.TryGetValue(word, out cCipherA);
            //    //Console.WriteLine("word: " + word + " score: " + ((cCipherA * 10) - Math.Pow((cCipherA - cNativA) * 2, 2)));
            //    biGramProb += ((cNativA * 10) - Math.Pow((cCipherA - cNativA) * 2, 2));
            //}

            //for (int j = 3; j < decryptedText.Length; j++)
            //{
            //    string word = decryptedText.Substring(j - 3, 3);

            //    double cNativA;
            //    double cCipherA;
            //    nativTD.TryGetValue(word, out cNativA);
            //    cipherTD.TryGetValue(word, out cCipherA);
            //    //Console.WriteLine("word: " + word + " score: " + ((cCipherA * 10) - Math.Pow((cCipherA - cNativA) * 2, 2)));
            //    triGramProb += ((cNativA * 10) - (Math.Pow((cCipherA - cNativA) * 2, 2)));
            //}
            return uniGramProb + biGramProb + triGramProb;
        }

        private static double dictionaryStatisticFitness(string decryptedText)
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
