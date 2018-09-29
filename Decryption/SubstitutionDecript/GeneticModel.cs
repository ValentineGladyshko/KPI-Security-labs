using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decryption.SubstitutionDecript
{
    public class GeneticModel
    {
        public const bool SEED_POPULATION = true;
        Random rand = new Random();

        string exampleText;
        string encryptedText;
        NGramFrequency exampleFr;

        NGramFrequency NormalLetterFrequency;
        NGramFrequency ExampleLetterFrequency;

        int numberOfGenerations = 1000;
        int populationSize = 500;
        int tournamentSize = 20;
        double tournamentWinnerProbability = 0.75;
        double probabilityOfCrossover = 0.65;
        int numberOfCrossoverPoints = 5;
        double probabilityOfMutation = 0.20;
        double percentageOfElitism = 15;

        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        List<Chromosome> population;
        int currentGeneration;

        public GeneticModel(string exampleText, string encryptedText)
        {
            this.encryptedText = encryptedText.Trim().ToUpper();
            this.exampleText = exampleText;

            this.exampleFr = new NGramFrequency(3, exampleText);
            exampleFr.Analyse();

            this.population = new List<Chromosome>();
            this.currentGeneration = 0;

            CreateFirstPopulation();
        }

        private int CompareIndividuals(Chromosome a, Chromosome b)
        {
            return -a.rate.CompareTo(b.rate);
        }

        public string GenerateRandomKey()
        {
            char[] key = alphabet.ToCharArray();
            for (int i = 0; i < key.Length; i++)
            {
                char c = key[i];
                int shuffleIndex = rand.Next(key.Length - i) + i;
                key[i] = key[shuffleIndex];
                key[shuffleIndex] = c;
            }
            return new String(key);
        }

        private void CreateFirstPopulation()
        {
            this.currentGeneration = 0;
            int remaining = this.populationSize;
            if (SEED_POPULATION)
            {
                int seeded = (int)Math.Ceiling(this.populationSize * 0.1);
                remaining -= seeded;
                double probabilityOfObeyingFrequency = 0.9;
                List<char> trainedFrequencies = new List<char>(alphabet.ToCharArray());
                List<char> cipherFrequencies = new List<char>(alphabet.ToCharArray());

                Comparison<char> trainedComparison = delegate (char c1, char c2)
                {
                    return -this.NormalLetterFrequency.FrequencyOf(Convert.ToString(c1)).CompareTo(this.NormalLetterFrequency.FrequencyOf(Convert.ToString(c2)));
                };
                Comparison<char> cipherComparison = delegate (char c1, char c2)
                {
                    return -this.ExampleLetterFrequency.FrequencyOf(Convert.ToString(c1)).CompareTo(this.ExampleLetterFrequency.FrequencyOf(Convert.ToString(c2)));
                };
                
                trainedFrequencies.Sort(trainedComparison);
                cipherFrequencies.Sort(cipherComparison);

                for (int i = 0; i < seeded; i++)
                {
                    char[] chromosone = new char[alphabet.Length];
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        char c = alphabet[j];
                        chromosone[j] = cipherFrequencies[trainedFrequencies.IndexOf(c)];
                    
                        double probability = rand.Next();
                        if (probability > probabilityOfObeyingFrequency)
                        {
                            int swapPosition = -1;
                            do
                            {
                                swapPosition = this.rand.Next(0, alphabet.Length);
                            } while (swapPosition == j);
                            char oldValue = chromosone[j];
                            chromosone[j] = chromosone[swapPosition];
                            chromosone[swapPosition] = oldValue;
                        }
                    }
                    Chromosome node = new Chromosome(exampleFr, new String(chromosone));
                    node.CalculateFitness(encryptedText);
                    population.Add(node);
                }
            }
            for (int i = 0; i < remaining; i++)
            {
                String randomChromosone = GenerateRandomKey();
                Chromosome node = new Chromosome(exampleFr, randomChromosone);
                node.CalculateFitness(encryptedText);
                population.Add(node);
            }

            population.Sort(CompareIndividuals);
        }

        private Chromosome[] CreateChildren(string chromosome1, string chromosome2)
        {
            Chromosome[] newChildren = new Chromosome[2];
            double probabilityOfCrossover = rand.NextDouble();
            double probabilityOfChild1Mutating = rand.NextDouble();
            double probabilityOfChild2Mutating = rand.NextDouble();
            int length = chromosome1.Length;

            char[] child1 = new char[length];
            char[] child2 = new char[length];

            // Position Based Crossover
            if (probabilityOfCrossover < this.probabilityOfCrossover)
            {
                List<int> randomPositions = new List<int>();
                for (int i = 0; i < this.numberOfCrossoverPoints; i++)
                {
                    int index = -1;
                    do
                    {
                        index = rand.Next(0, length);
                    } while (randomPositions.Contains(index));
                    randomPositions.Add(index);
                }

                List<char> remainingCharacters1 = new List<char>(chromosome2.ToCharArray());
                List<char> remainingCharacters2 = new List<char>(chromosome1.ToCharArray());

                foreach (int point in randomPositions)
                {
                    char c1 = chromosome1[point];
                    char c2 = chromosome2[point];
                    child1[point] = c1;
                    child2[point] = c2;
                    remainingCharacters1.Remove(c1);
                    remainingCharacters2.Remove(c2);
                }
                int j = 0;
                for (int i = 0; i < length; i++)
                {
                    if (!randomPositions.Contains(i))
                    {
                        child1[i] = remainingCharacters1[j];
                        child2[i] = remainingCharacters2[j];
                        j++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    child1[i] = chromosome1[i];
                    child2[i] = chromosome2[i];
                }
            }

            // Will Child One Mutate
            if (probabilityOfChild1Mutating < this.probabilityOfMutation)
            {
                int oldPosition = rand.Next(0, length);
                int newPosition = -1;
                do
                {
                    newPosition = rand.Next(0, length);
                } while (oldPosition == newPosition);
                char oldChar = child1[oldPosition];
                child1[oldPosition] = child1[newPosition];
                child1[newPosition] = oldChar;
            }

            // Will Child Two Mutate
            if (probabilityOfChild2Mutating < this.probabilityOfMutation)
            {
                int oldPosition = rand.Next(0, length);
                int newPosition = -1;
                do
                {
                    newPosition = rand.Next(0, length);
                } while (oldPosition == newPosition);
                char oldChar = child2[oldPosition];
                child2[oldPosition] = child2[newPosition];
                child2[newPosition] = oldChar;
            }
            // Create new children
            newChildren[0] = new Chromosome(exampleFr, new String(child1));
            newChildren[0].CalculateFitness(encryptedText);
            newChildren[1] = new Chromosome(exampleFr, new String(child2));
            newChildren[1].CalculateFitness(encryptedText);
            return newChildren;
        }

        public void CreateNewGeneration()
        {
            List<Chromosome> newPopulation = new List<Chromosome>();
            // Calculate how many parents survive to the next generation
            int elitismAmount = (int)Math.Ceiling(this.populationSize * (this.percentageOfElitism / 100));
            // Calculate how many children are born in the next generation
            int numberOfChildren = this.populationSize - elitismAmount;
            if (elitismAmount > 0)
            {
                // Copy elite parents into new population
                newPopulation.AddRange(this.population.GetRange(0, elitismAmount));
            }
            if (numberOfChildren > 0)
            {
                List<Chromosome> tournament1Members = new List<Chromosome>();
                List<Chromosome> tournament2Members = new List<Chromosome>();
                List<double> probabilityList = new List<double>();
                probabilityList.Add(this.tournamentWinnerProbability);
                double runningProbability = probabilityList[0];
                for (int i = 1; i < this.tournamentSize; i++)
                {
                    runningProbability *= (1 - this.tournamentWinnerProbability);
                    probabilityList.Add(probabilityList[i - 1] + runningProbability);
                }
                for (int i = 0; i < (int)Math.Ceiling(numberOfChildren / 2.0); i++)
                {
                    tournament1Members.Clear();
                    tournament2Members.Clear();
                    // Choose individuals for the two separate tournaments require that no individual appear more then
                    // once in the union of the two tournaments.
                    for (int j = 0; j < this.tournamentSize; j++)
                    {
                        Chromosome individual = null;
                        do
                        {
                            individual = this.population[rand.Next(0, this.populationSize)];
                        }
                        while (tournament1Members.Contains(individual));
                        tournament1Members.Add(individual);
                        do
                        {
                            individual = this.population[rand.Next(0, this.populationSize)];
                        }
                        while (tournament2Members.Contains(individual) || tournament1Members.Contains(individual));
                        tournament2Members.Add(individual);
                    }
                    // Sort members by fitness (descending)
                    tournament1Members.Sort(CompareIndividuals);
                    tournament2Members.Sort(CompareIndividuals);
                    // Choose two members from tournament                    
                    double probability1 = rand.NextDouble();
                    double probability2 = rand.NextDouble();
                    Chromosome winner1 = null;
                    Chromosome winner2 = null;
                    for (int j = 0; j < this.tournamentSize; j++)
                    {
                        if (probability1 <= probabilityList[j] || j == this.tournamentSize - 1)
                        {
                            winner1 = tournament1Members[j];
                            break;
                        }

                        if (probability2 <= probabilityList[j] || j == this.tournamentSize - 1)
                        {
                            winner2 = tournament2Members[j];
                            break;
                        }
                    }
                    // winner1 and winner2 are now our parents for two new children
                    Chromosome[] children = CreateChildren(winner1.chromosome, winner2.chromosome);
                    newPopulation.Add(children[0]);
                    // In case we had an odd number of children to create this stops us expanding the
                    // population size
                    if (newPopulation.Count < this.populationSize)
                    {
                        newPopulation.Add(children[1]);
                    }
                }
            }
            newPopulation.Sort(CompareIndividuals);
            this.population = newPopulation;
            this.currentGeneration++;
        }

    }
}
