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
            double langStat = languageStatisticFitness(decryptedText);
            double dictStat = dictionaryStatisticFitness(decryptedText);
            if ((30 / langStat) < 1200.0)
            {
                return 30 / langStat;
            }
            return 30 / langStat + dictStat;
            //return dictStat;
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
