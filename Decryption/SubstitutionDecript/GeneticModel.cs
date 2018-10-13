using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decryption.SubstitutionDecript
{
    public class GeneticModel
    {
        Random rand = new Random();

        private readonly char[] alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private string encryptedText;
        private SortedSet<Gen> population;
        private int generationCount = 350;
        private int populationCount = 30;
        private double[] maxScore;
        private double probabilityOfMutation = 0.8;
        private int numberOfCrossoverPoints = 2;
        private int mutationCount = 1;
        private double percentageOfElitism = 10;
        private int currentGeneration;

        public GeneticModel(string encryptedText)
        {
            maxScore = new double[generationCount];
            this.encryptedText = encryptedText;
            population = new SortedSet<Gen>();
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
            foreach (var gen in population)
            {
                sumRate += gen.CalculateFitness();
                if (maxRate < gen.CalculateFitness())
                    maxRate = gen.CalculateFitness();
            }

            maxScore[0] = maxRate;
            Console.WriteLine("generation: " + currentGeneration + " max rate: " + maxRate +  " avg rate: " + 
                (sumRate / populationCount) + " gen: " + population.Max.Chromosome);
            //population[0].Show(encryptedText);
            for (int i = 0; i < generationCount; i++)
            {
                CreateNewGeneration();
                //Console.WriteLine("generation: " + currentGeneration);
                maxRate = 0.0;
                sumRate = 0.0;
                foreach (var gen in population)
                {
                    sumRate += gen.CalculateFitness();
                    if (maxRate < gen.CalculateFitness())
                        maxRate = gen.CalculateFitness();
                }
                maxScore[i] = maxRate;
                
                if (maxRate > 6000.0)
                {
                    probabilityOfMutation = 0.65;
                }

                if (maxRate > 9500.0)
                {
                    probabilityOfMutation = 0.35;
                }
                Console.WriteLine("generation: " + currentGeneration + " max rate: " + maxRate + " avg rate: " +
                (sumRate / populationCount) + " gen: " + population.First().Chromosome);
                if (maxRate > 11913.0)
                {
                    return "Gen: " + population.First().Chromosome + "\n" +
                        new SubstitutionDecrypt(population.First().Chromosome).DecryptText(encryptedText);
                }
            }
            return new SubstitutionDecrypt(population.First().Chromosome).DecryptText(encryptedText);
        }

        private void CreateFirstPopulation()
        {
            currentGeneration = 0;
            population.Add(new Gen(new string(alphabet), encryptedText));
            while (population.Count < populationCount)
            {
                Gen gen = new Gen(GenerateRandomKey(), encryptedText);
                population.Add(gen);
            }
        }

        private Gen Mutate(Gen gen)
        {
            int length = gen.Chromosome.Length;
            char[] chromosome = gen.Chromosome.ToCharArray();
            for (int i = 0; i < mutationCount; i++)
            {
                int oldPosition = rand.Next(0, length);
                int newPosition = -1;
                do
                {
                    newPosition = rand.Next(0, length);
                }
                while (oldPosition == newPosition);
                char oldChar = chromosome[oldPosition];
                chromosome[oldPosition] = chromosome[newPosition];
                chromosome[newPosition] = oldChar;
            }
            int mut = 1;
            double mutation = rand.NextDouble();
            while(mutation < Math.Pow(probabilityOfMutation, mut))
            {
                mutation = rand.NextDouble();
                mut += 2;
                int oldPosition = rand.Next(0, length);
                int newPosition = -1;
                do
                {
                    newPosition = rand.Next(0, length);
                }
                while (oldPosition == newPosition);
                char oldChar = chromosome[oldPosition];
                chromosome[oldPosition] = chromosome[newPosition];
                chromosome[newPosition] = oldChar;
            }
            return new Gen(new string(chromosome), encryptedText);
        }

        public void CreateNewGeneration()
        {
            SortedSet<Gen> newPopulation = new SortedSet<Gen>();
            List<Gen> populationArray = population.ToList();
            int elitismAmount = (int)Math.Ceiling(populationCount * (percentageOfElitism / 100.0));
            
            int numberOfChildren = populationCount - elitismAmount;
            if (elitismAmount > 0)
            {
                for (int i = 0; i < elitismAmount; i++)
                {
                    newPopulation.Add(populationArray[i]);
                }
            }
            if (numberOfChildren > 0)
            {
                double sumRate = 0.0;
                for (int i = 0; i < elitismAmount; i++)
                {
                    sumRate += populationArray[i].CalculateFitness();
                }
                List<int> index = new List<int>();
                for (int i = 0; i < elitismAmount; i++)
                {
                    int count = (int)Math.Ceiling(Math.Pow(populationArray[i].CalculateFitness() / 100.0, 1.5));
                    for (int j = 0; j < count; j++)
                    {
                        index.Add(i);
                    }
                }

                while (newPopulation.Count < populationCount)
                {
                    newPopulation.Add(Mutate(populationArray[index[rand.Next(0, index.Count)]]));
                }
            }
            
            population = newPopulation;
            currentGeneration++;
        }

    }
}
