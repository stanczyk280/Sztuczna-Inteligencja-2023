using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA
{
    public class GeneticAlgorithm
    {
        private Random random;
        private int populationSize;
        private int chromosomeLength;
        private double crossoverProbability;
        private double mutationProbability;
        private Func<double, double> fitnessFunction;

        public GeneticAlgorithm(int populationSize, int chromosomeLength, double crossoverProbability, double mutationProbability, Func<double, double> fitnessFunction)
        {
            random = new Random();
            this.populationSize = populationSize;
            this.chromosomeLength = chromosomeLength;
            this.crossoverProbability = crossoverProbability;
            this.mutationProbability = mutationProbability;
            this.fitnessFunction = fitnessFunction;
        }

        public double Run(int numGenerations)
        {
            List<Chromosome> population = InitializePopulation();

            for (int generation = 0; generation < numGenerations; generation++)
            {
                EvaluateFitness(population);

                List<Chromosome> newPopulation = new List<Chromosome>();

                while (newPopulation.Count < populationSize)
                {
                    Chromosome parent1 = RouletteWheelSelection(population);
                    Chromosome parent2 = RouletteWheelSelection(population);
                    Chromosome child = Crossover(parent1, parent2);
                    Mutate(child);

                    newPopulation.Add(child);
                }

                population = newPopulation;
            }

            // Find the best solution
            EvaluateFitness(population);
            Chromosome bestChromosome = FindBestChromosome(population);

            return DecodeChromosome(bestChromosome);
        }

        private List<Chromosome> InitializePopulation()
        {
            List<Chromosome> population = new List<Chromosome>();

            for (int i = 0; i < populationSize; i++)
            {
                population.Add(GenerateRandomChromosome());
            }

            return population;
        }

        private Chromosome GenerateRandomChromosome()
        {
            bool[] genes = new bool[chromosomeLength];

            for (int i = 0; i < chromosomeLength; i++)
            {
                genes[i] = random.NextDouble() < 0.5;
            }

            return new Chromosome(genes);
        }

        private void EvaluateFitness(List<Chromosome> population)
        {
            foreach (Chromosome chromosome in population)
            {
                double value = DecodeChromosome(chromosome);
                chromosome.Fitness = fitnessFunction(value);
            }
        }

        private Chromosome RouletteWheelSelection(List<Chromosome> population)
        {
            double totalFitness = 0;

            foreach (Chromosome chromosome in population)
            {
                totalFitness += chromosome.Fitness;
            }

            double randomValue = random.NextDouble() * totalFitness;

            double sum = 0;

            foreach (Chromosome chromosome in population)
            {
                sum += chromosome.Fitness;

                if (sum >= randomValue)
                {
                    return chromosome;
                }
            }

            return population[population.Count - 1];
        }

        private Chromosome Crossover(Chromosome parent1, Chromosome parent2)
        {
            Chromosome child = new Chromosome(new bool[chromosomeLength]);

            for (int i = 0; i < chromosomeLength; i++)
            {
                if (random.NextDouble() < crossoverProbability)
                {
                    child.Genes[i] = parent1.Genes[i];
                }
                else
                {
                    child.Genes[i] = parent2.Genes[i];
                }
            }

            return child;
        }

        private void Mutate(Chromosome chromosome)
        {
            for (int i = 0; i < chromosomeLength; i++)
            {
                if (random.NextDouble() < mutationProbability)
                {
                    chromosome.Genes[i] = !chromosome.Genes[i];
                }
            }
        }

        private Chromosome FindBestChromosome(List<Chromosome> population)
        {
            Chromosome bestChromosome = population[0];

            foreach (Chromosome chromosome in population)
            {
                if (chromosome.Fitness > bestChromosome.Fitness)
                {
                    bestChromosome = chromosome;
                }
            }

            return bestChromosome;
        }

        private double DecodeChromosome(Chromosome chromosome)
        {
            int value = 0;

            for (int i = 0; i < chromosomeLength; i++)
            {
                if (chromosome.Genes[i])
                {
                    value += (int)Math.Pow(2, i);
                }
            }

            return value;
        }
    }
}