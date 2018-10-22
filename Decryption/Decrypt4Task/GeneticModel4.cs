using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decryption.SubstitutionDecript
{
    public class GeneticModel4
    {
        Random rand = new Random();

        private readonly char[] alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private string encryptedText;
        private SortedSet<Gen4> population;
        private int generationCount = 10000;
        private int populationCount = 100;
        private int keySize = 4;
        private double[] maxScore;
        private int mutationCount = 1;
        private double percentageOfElitism = 5;
        private double percentageOfParents = 20;
        private double percentageOfNonMutate = 40;
        private int currentGeneration;

        public GeneticModel4(string encryptedText)
        {
            maxScore = new double[generationCount];
            this.encryptedText = encryptedText;
            population = new SortedSet<Gen4>();
        }

        public string GenerateRandomKey()
        {
            char[] key = alphabet;
            for (int i = 0; i < key.Length; i++)
            {
                char c = key[i];
                int shuffleIndex = rand.Next(key.Length - i) + i;
                key[i] = key[shuffleIndex];
                key[shuffleIndex] = c;
            }
            return new string(key);
        }

        public string Run()
        {
            CreateFirstPopulation();
            double maxRate = 0.0;
            double sumRate = 0.0;
            int ss = 0;

            foreach (var Gen4 in population)
            {
                sumRate += Gen4.CalculateFitness();
                if (maxRate < Gen4.CalculateFitness())
                    maxRate = Gen4.CalculateFitness();
            }

            maxScore[0] = maxRate;
            Console.WriteLine("generation: " + currentGeneration + " max rate: " + maxRate + " avg rate: " +
                (sumRate / populationCount));
            //population[0].Show(encryptedText);
            for (int i = 0; i < generationCount; i++)
            {
                
                if (ss == 20)
                {
                    string decryptedText = new SubstitutionDecrypt4(population.First().Chromosome).DecryptText(encryptedText);
                    Console.WriteLine("\n===========\n" + decryptedText);
                    CipherFitness4.Show(decryptedText);
                    //CipherFitness4.NewEvaluateShow(decryptedText);
                    Console.WriteLine("\n===========\n");
                    ss = 0;
                }
                ss++;
                CreateNewGeneration();
                //Console.WriteLine("generation: " + currentGeneration);
                maxRate = 0.0;
                sumRate = 0.0;
                foreach (var Gen4 in population)
                {
                    sumRate += Gen4.CalculateFitness();
                    if (maxRate < Gen4.CalculateFitness())
                        maxRate = Gen4.CalculateFitness();
                }
                maxScore[i] = maxRate;
                if (i > 20 && maxScore[i] == maxScore[i - 20])
                {

                    //Console.Write("No new gens: ");
                    //CipherFitness4.Show(new SubstitutionDecrypt4(population.First().Chromosome).DecryptText(encryptedText));
                }
                
 
               // Console.WriteLine("generation: " + currentGeneration + " max rate: " + maxRate + " avg rate: " +
               // (sumRate / populationCount));
                // population[0].Show(encryptedText);
            }
            return new SubstitutionDecrypt4(population.First().Chromosome).DecryptText(encryptedText);
        }

        private void CreateFirstPopulation()
        {
            currentGeneration = 0;
            List<string> gen = new List<string>();
            for (int i = 0; i < keySize; i++)
            {
                gen.Add(new string(alphabet));
            }
            population.Add(new Gen4(gen, encryptedText));
            while (population.Count < populationCount)
            {
                gen = new List<string>();
                for (int i = 0; i < keySize; i++)
                {
                    gen.Add(GenerateRandomKey());
                }
                Gen4 Gen4 = new Gen4(gen, encryptedText);
                population.Add(Gen4);
            }
        }

        private Gen4 Mutate(Gen4 gen)
        {
            List<char[]> chromosome = new List<char[]>();
            for (int i = 0; i < gen.Chromosome.Count; i++)
            {
                chromosome.Add(gen.Chromosome[i].ToCharArray());
            }

            int mutate = rand.Next(1, keySize);


            for (int j = 0; j < mutate; j++)
            {
                int position = rand.Next(0, keySize);
                int length = chromosome[position].Length;

                for (int i = 0; i < mutationCount; i++)
                {
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
                }
                int mut = 1;
                double mutation = rand.NextDouble();
                while (mutation < Math.Pow(0.8, mut))
                {
                    position = rand.Next(0, keySize);
                    mutation = rand.NextDouble();
                    mut += 1;
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
                }
            }
            List<string> newChromosome = new List<string>();

            for (int i = 0; i < chromosome.Count; i++)
            {
                newChromosome.Add(new string(chromosome[i]));
            }
            return new Gen4(newChromosome, encryptedText);
        }

        public void CreateNewGeneration()
        {
            SortedSet<Gen4> newPopulation = new SortedSet<Gen4>();
            List<Gen4> populationArray = population.ToList();
            int elitismAmount = (int)Math.Ceiling(populationCount * (percentageOfElitism / 100.0));
            int parentsAmount = (int)Math.Ceiling(populationCount * (percentageOfParents / 100.0));
            int nonMutateAmount = (int)Math.Ceiling(populationCount * (percentageOfNonMutate / 100.0));

            int numberOfChildren = populationCount - elitismAmount;
            if (elitismAmount > 0)
            {
                for (int i = 0; i < elitismAmount; i++)
                {
                    newPopulation.Add(populationArray[i]);
                }
            }

            List<Gen4> children = new List<Gen4>();
            for (int i = 0; i < nonMutateAmount; i++)
            {

                int pos1 = rand.Next(0, parentsAmount);
                int pos2 = -1;
                do
                {
                    pos2 = rand.Next(0, parentsAmount);
                }
                while (pos1 == pos2);

                Gen4 child = Gen4.Crossover(populationArray[pos1], populationArray[pos2]);
                newPopulation.Add(child);

                children.Add(child);


            }

            while (newPopulation.Count < populationCount)
            {
                if (children.Count > 0)
                {
                    Gen4 gen = children[0];
                    children.RemoveAt(0);
                    newPopulation.Add(Gen4.Mutate(gen));
                    continue;
                }
                newPopulation.Add(Mutate(populationArray[rand.Next(0, parentsAmount)]));
            }


            population = newPopulation;
            currentGeneration++;
        }

    }
}
