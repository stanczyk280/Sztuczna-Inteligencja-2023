using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA
{
    public class GeneticAlgorithm
    {
        private Random random; // generator liczb losowych
        private int populationSize; //rozmiar populacji
        private int chromosomeLength; //długość chormosomu
        private double crossoverProbability; //prawdopodobieństwo, że nastąpi skrzyżowanie
        private double mutationProbability; // prawdopodobieństwo, że nastąpi mutacja
        private Func<double, double> fitnessFunction; // Funkcja określająca czy chromosom się nadaje

        //konstruktor
        public GeneticAlgorithm(int populationSize, int chromosomeLength, double crossoverProbability, double mutationProbability, Func<double, double> fitnessFunction)
        {
            random = new Random();
            this.populationSize = populationSize;
            this.chromosomeLength = chromosomeLength;
            this.crossoverProbability = crossoverProbability;
            this.mutationProbability = mutationProbability;
            this.fitnessFunction = fitnessFunction;
        }

        //Przyjmuje liczbę liczbę generacji i zwraca najlepsze rozwiązanie
        public double Run(int numGenerations)
        {
            List<Chromosome> population = InitializePopulation(); //inicjalizacja populacji

            for (int generation = 0; generation < numGenerations; generation++)
            {
                EvaluateFitness(population); //każdy chromosom z populacji sprawdzany jest czy sie nadaje

                List<Chromosome> newPopulation = new List<Chromosome>(); // stworzenie nowej pustej populacji

                //Dodanie do nowej populacji chromosomów za pomocą metody koła ruletki, krzyżowania i mutacji

                /*
                 * działanie metody koła ruletki:
                 * Metoda działa na zasadzie proporcjonalnego reprezentowania przystosowania na kole ruletki
                 * gdzie osobniki o wyższym przystosowaniu mają większe szanse na wybór. W rezultacie
                 * lepiej przystosowane osobniki mają większe prawdopodobieństwo zostania wybrane do reprodukcji, 
                 * co pozwala na przekazywanie pożądanych cech i optymalizację rozwiązania 
                 * w kolejnych pokoleniach algorytmu genetycznego. Metoda koła ruletki może faworyzować silnie 
                 * przystosowane osobniki kosztem słabiej przystosowanych, co może prowadzić do zjawiska zwanego 
                 * "premature convergence" (przedwczesna zbieżność), gdy algorytm utyka w lokalnym optimum.
                 */

                while (newPopulation.Count < populationSize)
                {
                    Chromosome parent1 = RouletteWheelSelection(population);
                    Chromosome parent2 = RouletteWheelSelection(population);
                    Chromosome child = Crossover(parent1, parent2);
                    Mutate(child);

                    newPopulation.Add(child);
                }

                population = newPopulation; //nowa populacja staje się aktualną populacją
            }

            
            EvaluateFitness(population); //Sprawdź każdy chromosom w populacji
            Chromosome bestChromosome = FindBestChromosome(population); //Wybierz najlepszy chromosom

            return DecodeChromosome(bestChromosome); // dekodowanie chromosomu
        }

        //Funkcja tworząca populacje
        private List<Chromosome> InitializePopulation()
        {
            List<Chromosome> population = new List<Chromosome>();

            for (int i = 0; i < populationSize; i++)
            {
                population.Add(GenerateRandomChromosome());
            }

            return population;
        }

        //Tworzenie losowych chromosomów
        private Chromosome GenerateRandomChromosome()
        {
            bool[] genes = new bool[chromosomeLength];

            for (int i = 0; i < chromosomeLength; i++)
            {
                genes[i] = random.NextDouble() < 0.5;
            }

            return new Chromosome(genes);
        }


        //Funckja oceniająca
        private void EvaluateFitness(List<Chromosome> population)
        {
            foreach (Chromosome chromosome in population)
            {
                double value = DecodeChromosome(chromosome);
                chromosome.Fitness = fitnessFunction(value);
            }
        }

        //Metoda koła ruletki
        /*
         * Na początku, każdemu osobnikowi w populacji przypisuje się wartość przystosowania, która informuje, 
         * jak dobrze dany osobnik radzi sobie w kontekście rozwiązywanego problemu. Wyższe wartości 
         * przystosowania oznaczają lepsze dopasowanie do rozwiązania.
         * 
         * Sumuje się wszystkie wartości przystosowania wszystkich osobników w populacji. 
         * Ta suma jest nazywana sumą przystosowania.
         * 
         * Następnie przekształca się wartości przystosowania w przedziały na kole ruletki proporcjonalne do ich wartości. 
         * Im wyższa wartość przystosowania, tym większy udział na kole ruletki otrzymuje dany osobnik.
         * 
         * Losowo wybiera się punkt startowy na kole ruletki.
         * 
         * Wybiera się osobniki na podstawie losowego punktu startowego. Im większy udział na kole ruletki ma dany osobnik,
         * tym większe prawdopodobieństwo, że zostanie wybrany.
         * 
         * Powtarza się kroki 4 i 5, aż do wyboru oczekiwanej liczby osobników.
         */

        //Funkcja, która przyjmuje populację osobników jako argument i zwraca wybrany osobnik(chromosom).
        private Chromosome RouletteWheelSelection(List<Chromosome> population)
        {
            //Zmienna przechowuje sumę wartości przystosowania wszystkich osobników w populacji.
            double totalFitness = 0;

            //Sumowanie wartości przystosowania
            foreach (Chromosome chromosome in population)
            {
                totalFitness += chromosome.Fitness;
            }

            // Generowanie losowej wartości użytej do wyboru osobnika na podstawie losowego punktu startowego na kole
            double randomValue = random.NextDouble() * totalFitness;  

            double sum = 0;

            foreach (Chromosome chromosome in population)
            {
                //Dodaje wartość przystosowania bieżącego osobnika do zmiennej sum.
                //Suma wartości przystosowania osobników od początku populacji.
                sum += chromosome.Fitness;

                //Sprawdza, czy suma przystosowań osiągnęła lub przekroczyła wartość randomValue.
                //Jeśli tak, oznacza to, że osiągnięto punkt na kole ruletki, w którym wybrany zostanie bieżący osobnik.
                if (sum >= randomValue)
                {
                    return chromosome;
                }
            }

            //Jeśli żaden osobnik nie został wybrany w poprzednich krokach, zwraca ostatniego osobnika w populacji.
            return population[population.Count - 1];
        }


        //Funckja przeprowadzająca krzyżowanie genów w sposób losowy
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

        //Funkcja przeprowadzająca mutacje w losowy sposób
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

        //Funckja znajdująca najlepszy chromosom
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

        //Funkcja dekodująca chromosom
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