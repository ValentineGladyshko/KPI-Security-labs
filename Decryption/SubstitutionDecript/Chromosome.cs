using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decryption.SubstitutionDecript
{
    public class Chromosome
    {
        public const double FITNESS_LOGARITHM_BASE = 2.0;
        public double rate = 0;
        public String chromosome;
        public NGramFrequency nGramFrequency;

        public Chromosome(NGramFrequency nGramFrequency, String chromosome)
        {
            this.chromosome = chromosome;
            this.nGramFrequency = nGramFrequency;
        }

        public void CalculateFitness(string encryptedText)
        {
            SubstitutionDecrypt decriptor = new SubstitutionDecrypt(encryptedText);
            string decryptedText = decriptor.DecryptText(chromosome);

            NGramFrequency frequency = new NGramFrequency(3, decryptedText);

            frequency.Analyse();

            foreach (string ngram in frequency.occurrences.Keys)
            {
                double trainedFrequency = nGramFrequency.FrequencyOf(ngram);
                if (trainedFrequency != 0)
                {
                    rate += frequency.FrequencyOf(ngram) * Math.Log(trainedFrequency, FITNESS_LOGARITHM_BASE);
                }
            }
        }
    }
}
