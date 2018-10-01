using System;

namespace Decryption.SubstitutionDecript
{
    public class CipherFitness
    {
        public double Evaluate(string decryptedText)
        {
            decryptedText = decryptedText.ToUpper();
            double langStat = languageStatisticFitness(decryptedText);
            double dictStat = dictionaryStatisticFitness(decryptedText);
            //Console.WriteLine(0.00001 / langStat);
            //Console.WriteLine(dictStat);
            return 0.00001/langStat + dictStat;
        }

        private double languageStatisticFitness(string decryptedText)
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

            for (char c = 'A'; c <= 'Z'; c++)
            {
                double cNativ;
                double cCipher;
                nativUD.TryGetValue(c, out cNativ);
                cipherUD.TryGetValue(c, out cCipher);
                //add unigram 
                //Console.WriteLine(c + " n " + cNativ);
                //Console.WriteLine(c + " c " + cCipher);
                uniGramProb += Math.Pow(Math.Abs(cNativ - cCipher), 6);
                
                //bigram 
                for (char c1 = 'A'; c1 <= 'Z'; c1++)
                {
                    double cNativA;
                    double cCipherA;
                    char[] blockC = {c,c1};
                    string blockS = new string(blockC);
                    nativBD.TryGetValue(blockS, out cNativA);
                    cipherBD.TryGetValue(blockS, out cCipherA);
                    //add bigram prob
                    //Console.WriteLine(blockS + " n " + cNativA);
                    //Console.WriteLine(blockS + " c " + cCipherA);
                    biGramProb += Math.Pow(Math.Abs(cNativA - cCipherA), 6);

                    //trigram
                    for(char c2 = 'A'; c2 <= 'Z'; c2++)
                    {
                        double cNativB;
                        double cCipherB;
                        char[] blockD = { c, c1, c2 };
                        string blockF = new string(blockD);
                        nativTD.TryGetValue(blockF, out cNativB);
                        cipherTD.TryGetValue(blockF, out cCipherB);
                        //add trigram prob
                        //Console.WriteLine(blockF + " n " + cNativB);
                        //Console.WriteLine(blockF + " c " + cCipherB);
                        triGramProb += Math.Pow(Math.Abs(cNativB - cCipherB), 6);
                    }            
                }
            }
            return alpha * uniGramProb + beta * biGramProb + gamma * triGramProb;
        }

        private double dictionaryStatisticFitness(string decryptedText)
        {
            double score = 0.0;
            double length = decryptedText.Length;
            var helper = DictionaryStatisticsHelper.GetDictionaryStatistics();
            for(int i=4;i<10;i++)
            {
                for (int j = i; j < decryptedText.Length; j++)
                {
                    string word = decryptedText.Substring(j - i, i);
                    if (helper.Dictionary.Contains(word))
                    {
                        score += 2.0 * Math.Pow(i, 2);
                    }
                }
            }
           
            return 1000.0 * score/ length;
        }
    }
}
