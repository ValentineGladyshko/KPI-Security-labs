//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Decryption.SubstitutionDecript
//{
//    public class GeneticModel4
//    {
//        Random rand = new Random();

//        private readonly char[] alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
//        private string encryptedText;
//        private SortedSet<Gen4> population;
//        private int generationCount = 500;
//        private int populationCount = 50;
//        private double[] maxScore;
//        //private double probabilityOfCrossover = 0.65;
//        private int numberOfCrossoverPoints = 2;
//        private int mutationCount = 1;
//        private double percentageOfElitism = 25;
//        private int currentGeneration;

//        public GeneticModel4(string encryptedText)
//        {
//            maxScore = new double[generationCount];
//            this.encryptedText = encryptedText;
//            population = new SortedSet<Gen4>();
//        }

//        public string GenerateRandomKey()
//        {
//            char[] key = alphabet;
//            for (int i = 0; i < key.Length; i++)
//            {
//                char c = key[i];
//                int shuffleIndex = rand.Next(key.Length - i) + i;
//                key[i] = key[shuffleIndex];
//                key[shuffleIndex] = c;
//            }
//            return new string(key);
//        }

//        public string Run()
//        {
//            CreateFirstPopulation();
//            double maxRate = 0.0;
//            double sumRate = 0.0;
//            foreach (var Gen4 in population)
//            {
//                sumRate += Gen4.CalculateFitness();
//                if (maxRate < Gen4.CalculateFitness())
//                    maxRate = Gen4.CalculateFitness();
//            }

//            maxScore[0] = maxRate;
//            Console.WriteLine("generation: " + currentGeneration + " max rate: " + maxRate +  " avg rate: " + 
//                (sumRate / populationCount) + " Gen4: " + population.Max.Chromosome);
//            //population[0].Show(encryptedText);
//            for (int i = 0; i < generationCount; i++)
//            {
//                CreateNewGeneration();
//                //Console.WriteLine("generation: " + currentGeneration);
//                maxRate = 0.0;
//                sumRate = 0.0;
//                foreach (var Gen4 in population)
//                {
//                    sumRate += Gen4.CalculateFitness();
//                    if (maxRate < Gen4.CalculateFitness())
//                        maxRate = Gen4.CalculateFitness();
//                }
//                maxScore[i] = maxRate;
//                if (i > 20 && maxScore[i] == maxScore[i - 20])
//                {
                    
//                    Console.Write("No new gens: ");
//                    CipherFitness.Show(new SubstitutionDecrypt4(population.First().Chromosome).DecryptText(encryptedText));

//                    //int elitismAmount = (int)Math.Ceiling(populationCount * (percentageOfElitism / 100.0));
//                    //for (int j = 0; j < elitismAmount; j++)
//                    //{
//                    //    Console.WriteLine(population[j].chromosome);
//                    //}
//                }
//                //if (maxRate > 800.0)
//                //{
//                //    mutationCount = 4;
//                //    probabilityOfMutation = 0.4;
//                //    numberOfCrossoverPoints = 4;
//                //    probabilityOfCrossover = 0.6;
//                //}
//                //if (maxRate > 1500.0)
//                //{
//                //    mutationCount = 3;
//                //    probabilityOfMutation = 0.35;
//                //    numberOfCrossoverPoints = 3;
//                //    probabilityOfCrossover = 0.5;
//                //}
//                if (maxRate > 5500.0)
//                {
//                    Console.WriteLine(new SubstitutionDecrypt4(population.First().Chromosome).DecryptText(encryptedText));
//                    //Console.WriteLine(sd.DecryptText(encryptedText));
//                }
//                Console.WriteLine("generation: " + currentGeneration + " max rate: " + maxRate + " avg rate: " +
//                (sumRate / populationCount) + " Gen4: " + population.First().Chromosome);
//                // population[0].Show(encryptedText);
//            }
//            return new SubstitutionDecrypt4(population.First().Chromosome).DecryptText(encryptedText);
//        }

//        private void CreateFirstPopulation()
//        {
//            currentGeneration = 0;
//            population.Add(new Gen4(new string(alphabet), encryptedText));
//            while (population.Count < populationCount)
//            {
//                Gen4 Gen4 = new Gen4(GenerateRandomKey(), encryptedText);
//                population.Add(Gen4);
//            }
//        }

//        private Gen4 CreateChild(Gen4 gen1, Gen4 gen2)
//        {
//            int length = gen1.Chromosome.Length;

//            char[] child = new char[length];

//            // Position Based Crossover

//            List<int> randomPositions = new List<int>();
//            for (int i = 0; i < numberOfCrossoverPoints; i++)
//            {
//                int index = -1;
//                do
//                {
//                    index = rand.Next(0, length);
//                }
//                while (randomPositions.Contains(index));

//                randomPositions.Add(index);
//            }

//            List<char> remainingCharacters1 = new List<char>(gen2.Chromosome.ToCharArray());
//            List<char> remainingCharacters2 = new List<char>(gen1.Chromosome.ToCharArray());

//            foreach (int point in randomPositions)
//            {
//                char c1 = gen1.Chromosome[point];
//                char c2 = gen2.Chromosome[point];
//                child[point] = c1;
//                remainingCharacters1.Remove(c1);
//            }
//            int j = 0;
//            for (int i = 0; i < length; i++)
//            {
//                if (!randomPositions.Contains(i))
//                {
//                    child[i] = remainingCharacters1[j];
//                    j++;
//                }
//            }

//            // Will Child Mutate

//            for (int i = 0; i < mutationCount; i++)
//            {
//                int oldPosition = rand.Next(0, length);
//                int newPosition = -1;
//                do
//                {
//                    newPosition = rand.Next(0, length);
//                }
//                while (oldPosition == newPosition);
//                char oldChar = child[oldPosition];
//                child[oldPosition] = child[newPosition];
//                child[newPosition] = oldChar;
//            }
//            return new Gen4(new string(child), encryptedText);
//        }

//        private Gen4 Mutate(Gen4 Gen4)
//        {
//            int length = Gen4.Chromosome.Length;
//            char[] chromosome = Gen4.Chromosome.ToCharArray();

//            for (int i = 0; i < mutationCount; i++)
//            {
//                int oldPosition = rand.Next(0, length);
//                int newPosition = -1;
//                do
//                {
//                    newPosition = rand.Next(0, length);
//                }
//                while (oldPosition == newPosition);
//                char oldChar = chromosome[oldPosition];
//                chromosome[oldPosition] = chromosome[newPosition];
//                chromosome[newPosition] = oldChar;
//            }
//            return new Gen4(new string(chromosome), encryptedText);
//        }

//        public void CreateNewGeneration()
//        {
//            SortedSet<Gen4> newPopulation = new SortedSet<Gen4>();
//            List<Gen4> populationArray = population.ToList();
//            int elitismAmount = (int)Math.Ceiling(populationCount * (percentageOfElitism / 100.0));
            
//            int numberOfChildren = populationCount - elitismAmount;
//            if (elitismAmount > 0)
//            {
//                for (int i = 0; i < elitismAmount; i++)
//                {
//                    newPopulation.Add(populationArray[i]);
//                }
//            }
//            if (numberOfChildren > 0)
//            {
//                double sumRate = 0.0;
//                foreach(var Gen4 in population)
//                {
//                    sumRate += Gen4.CalculateFitness();
//                }
//                List<int> index = new List<int>();
//                for (int i = 0; i < population.Count; i++)
//                {
//                    int count = (int)Math.Ceiling(Math.Pow(populationArray[i].CalculateFitness(), 0.9) / 10.0);
//                    for (int j = 0; j < count; j++)
//                    {
//                        index.Add(i);
//                    }
//                }
                
//                for (int i = 0; i < numberOfChildren; i++)
//                {
//                    Gen4 parent1 = populationArray[index[rand.Next(0, index.Count)]];
//                    Gen4 parent2 = new Gen4("", encryptedText);
//                    do
//                    {
//                        parent2 = populationArray[index[rand.Next(0, index.Count)]];
//                    }
//                    while (parent1.CompareTo(parent2) == 0);

//                    newPopulation.Add(CreateChild(parent1, parent2));
//                }
                
//                while (newPopulation.Count < populationCount)
//                {
//                    newPopulation.Add(Mutate(populationArray[index[rand.Next(0, index.Count)]]));
//                }
//            }
            
//            population = newPopulation;
//            currentGeneration++;
//        }

//    }
//}
