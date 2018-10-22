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

        public static Gen4 Crossover(Gen4 gen1, Gen4 gen2)
        {
            Random rand = new Random();
           
            List<string> newChromosome = new List<string>();

            for (int i = 0; i < gen1.chromosome.Count; i++)
            {
                Tuple<bool, char, char>[] difference = new Tuple<bool, char, char>[gen1.chromosome[i].Length];
                List<char> crossoverChars = new List<char>();
                List<char> gen = new List<char>();
                for (int j = 0; j < gen1.chromosome[i].Length; j++)
                {
                    bool equal = (gen1.chromosome[i][j] == gen2.chromosome[i][j]);
                    if (!equal)
                        crossoverChars.Add(gen1.chromosome[i][j]);
                    difference[j] = new Tuple<bool, char, char>(equal, gen1.chromosome[i][j], gen2.chromosome[i][j]);
                }

                foreach(var elem in difference)
                {
                    if (elem.Item1 == true)
                        gen.Add(elem.Item2);
                    else
                    {
                        int pos = rand.Next(0, 2);
                        if (pos == 0)
                        {
                            if(crossoverChars.Contains(elem.Item2))
                            {
                                gen.Add(elem.Item2);
                                crossoverChars.Remove(elem.Item2);
                            }
                            else if(crossoverChars.Contains(elem.Item3))
                            {
                                gen.Add(elem.Item3);
                                crossoverChars.Remove(elem.Item3);
                            }
                            else
                            {
                                gen.Add('~');
                            }
                        }
                        else
                        {
                            if (crossoverChars.Contains(elem.Item3))
                            {
                                gen.Add(elem.Item3);
                                crossoverChars.Remove(elem.Item3);
                            }
                            else if (crossoverChars.Contains(elem.Item2))
                            {
                                gen.Add(elem.Item2);
                                crossoverChars.Remove(elem.Item2);
                            }                            
                            else
                            {
                                gen.Add('~');
                            }
                        }
                    }
                }

                for (int k = 0; k < crossoverChars.Count;)
                {
                    int pos = rand.Next(0, crossoverChars.Count);
                    int pos2 = gen.FindIndex(c => c == '~');
                    gen[pos2] = crossoverChars[pos];
                    crossoverChars.RemoveAt(pos);
                }

                newChromosome.Add(new string(gen.ToArray()));
            }

            return new Gen4(newChromosome, gen1.encryptedText);
        }

        public static Gen4 Mutate(Gen4 gen)
        {
            Random rand = new Random();

            List<char[]> chromosome = new List<char[]>();
            for (int i = 0; i < gen.Chromosome.Count; i++)
            {
                chromosome.Add(gen.Chromosome[i].ToCharArray());
            }

            int position = rand.Next(0, gen.chromosome.Count);
            int length = chromosome[position].Length;

            int oldPosition = rand.Next(0, length);
            int newPosition = -1;
            do
            {
                newPosition = rand.Next(0, length);
            }
            while (oldPosition == newPosition);
            char oldChar = chromosome[position][oldPosition];
            chromosome[position][oldPosition] = chromosome[position][newPosition];
            chromosome[position][newPosition] = oldChar;

            while (rand.NextDouble() < 0.5)
            {
                position = rand.Next(0, gen.chromosome.Count);
                
                oldPosition = rand.Next(0, length);
                newPosition = -1;
                do
                {
                    newPosition = rand.Next(0, length);
                }
                while (oldPosition == newPosition);
                oldChar = chromosome[position][oldPosition];
                chromosome[position][oldPosition] = chromosome[position][newPosition];
                chromosome[position][newPosition] = oldChar;
            }

            List<string> newChromosome = new List<string>();

            for (int i = 0; i < chromosome.Count; i++)
            {
                newChromosome.Add(new string(chromosome[i]));
            }
            return new Gen4(newChromosome, gen.encryptedText);
        }

        public void Show()
        {
            SubstitutionDecrypt4 decriptor = new SubstitutionDecrypt4(Chromosome);
            string decryptedText = decriptor.DecryptText(encryptedText);
            Console.WriteLine(CipherFitness4.NewEvaluate(decryptedText));
        }

        public double CalculateFitness()
        {
            if (rate.HasValue)
                return rate.Value;
            else
            {
                SubstitutionDecrypt4 decriptor = new SubstitutionDecrypt4(Chromosome);
                string decryptedText = decriptor.DecryptText(encryptedText);
                rate = CipherFitness4.NewEvaluate(decryptedText);
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
