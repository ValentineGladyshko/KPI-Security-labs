using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decryption.SubstitutionDecript
{
    public class Gen4 : IComparable<Gen4>
    {
        private double? rate;
        private List<string> chromosome;
        private readonly string encryptedText;

        public List<string> Chromosome
        {
            get
            {
                return chromosome;
            }
            set
            {
                rate = null;
                chromosome = value;
            }
        }

        public Gen4(List<string> chromosome, string encryptedText)
        {
            rate = null;
            Chromosome = chromosome.ToList();
            this.encryptedText = encryptedText;
        }

        public Gen4 ChangeGen(int position, string gen)
        {
            Gen4 newGen = new Gen4(Chromosome, encryptedText);
            newGen.Chromosome[position] = gen;
            return newGen;
        }

        public void Show()
        {
            SubstitutionDecrypt4 decriptor = new SubstitutionDecrypt4(Chromosome);
            string decryptedText = decriptor.DecryptText(encryptedText);
            Console.WriteLine(CipherFitness4.Evaluate(decryptedText));
        }

        public double CalculateFitness()
        {
            if (rate.HasValue)
                return rate.Value;
            else
            {
                SubstitutionDecrypt4 decriptor = new SubstitutionDecrypt4(Chromosome);
                string decryptedText = decriptor.DecryptText(encryptedText);
                rate = CipherFitness4.Evaluate(decryptedText);
                return rate.Value;
            }
            
        }

        public int CompareTo(Gen4 other)
        {
            CalculateFitness();
            other.CalculateFitness();
            int result = other.rate.Value.CompareTo(rate.Value);
            if(result != 0)
            {
                return result;
            }
            else
            {
                for (int i = 0; i < Chromosome.Count; i++)
                {
                    result = Chromosome[i].CompareTo(other.Chromosome[i]);
                    if (result != 0)
                    {
                        return result;
                    }
                }
            }
            return 0;
        }
    }
}
