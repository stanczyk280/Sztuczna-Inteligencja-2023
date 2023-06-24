using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralSchedulingProblemGEN
{
    public class GeneticScheduler
    {
        private List<Task> tasks; //lista zadań
        private int numProcessors; //ilość procesorów
        private int populationSize; //rozmiar populacji
        private int maxGenerations; //maksymalna ilość kolejnych generacji
        private Random random; //losowa liczba

        public GeneticScheduler(List<Task> tasks, int numProcessors, int populationSize, int maxGenerations)
        {
            this.tasks = tasks;
            this.numProcessors = numProcessors;
            this.populationSize = populationSize;
            this.maxGenerations = maxGenerations;
            this.random = new Random();
        }

        //Znalezienie najlepszego harmonogramu poprzez realizacje alogrytmu genetycznego
        public Schedule FindBestSchedule()
        {
            //inicjalizacja populacji
            List<Schedule> population = InitializePopulation();
            Schedule bestSchedule = FindFittestSchedule(population);

            //pętla po generacjach
            for (int generation = 0; generation < maxGenerations; generation++)
            {
                List<Schedule> newPopulation = new List<Schedule>();
                //określenie najlepszego rodzica i utworzenie nowych dzieci
                while (newPopulation.Count < populationSize)
                {
                    Schedule parent1 = SelectParent(population);
                    Schedule parent2 = SelectParent(population);

                    Schedule child = Crossover(parent1, parent2);
                    Mutate(child);

                    newPopulation.Add(child);
                }

                //zamiana starej populacji na nową populacje
                population = newPopulation;
                Schedule fittestSchedule = FindFittestSchedule(population);

                //ustalenie najlepszego harmonogramu
                if (fittestSchedule.CalculateMakespan() < bestSchedule.CalculateMakespan())
                {
                    bestSchedule = fittestSchedule;
                }
            }

            return bestSchedule;
        }

        //inicjalizacja populacji z losowymi harmonogramami
        private List<Schedule> InitializePopulation()
        {
            List<Schedule> population = new List<Schedule>();

            for (int i = 0; i < populationSize; i++)
            {
                Schedule schedule = CreateRandomSchedule();
                population.Add(schedule);
            }

            return population;
        }

        //stworzenie losowego harmonogramu
        private Schedule CreateRandomSchedule()
        {
            List<Processor> processors = new List<Processor>();

            for (int i = 0; i < numProcessors; i++)
            {
                processors.Add(new Processor(i));
            }

            foreach (Task task in tasks)
            {
                int processorIndex = random.Next(numProcessors);
                processors[processorIndex].TaskList.Add(task);
            }

            return new Schedule(processors);
        }

        //znalezienie najlepszego harmonogramu
        private Schedule FindFittestSchedule(List<Schedule> population)
        {
            Schedule fittest = population[0];

            foreach (Schedule schedule in population)
            {
                if (schedule.CalculateMakespan() < fittest.CalculateMakespan())
                {
                    fittest = schedule;
                }
            }

            return fittest;
        }

        //wybierz losowo rodzica
        private Schedule SelectParent(List<Schedule> population)
        {
            int index = random.Next(population.Count);
            return population[index];
        }

        //metoda krzyżująca rodziców aby stworzyć nowe dziecko
        private Schedule Crossover(Schedule parent1, Schedule parent2)
        {
            List<Processor> childProcessors = new List<Processor>(); //lista dzieci rodziców

            for (int i = 0; i < numProcessors; i++)
            {
                Processor parent1Processor = parent1.Processors[i];
                Processor parent2Processor = parent2.Processors[i];

                Processor childProcessor = new Processor(i);
                List<Task> childTaskList = new List<Task>();

                // Określa, który procesor ma mniejszą listę zadań
                int minTaskListSize = Math.Min(parent1Processor.TaskList.Count, parent2Processor.TaskList.Count);

                // Losowo wybiera zadanie od rodzica
                for (int j = 0; j < minTaskListSize; j++)
                {
                    if (random.NextDouble() < 0.5)
                    {
                        childTaskList.Add(parent1Processor.TaskList[j]);
                    }
                    else
                    {
                        childTaskList.Add(parent2Processor.TaskList[j]);
                    }
                }

                // Dorzuca pozostałe zadania z większej listy do listy dziecka
                if (parent1Processor.TaskList.Count > parent2Processor.TaskList.Count)
                {
                    childTaskList.AddRange(parent1Processor.TaskList.GetRange(minTaskListSize, parent1Processor.TaskList.Count - minTaskListSize));
                }
                else if (parent2Processor.TaskList.Count > parent1Processor.TaskList.Count)
                {
                    childTaskList.AddRange(parent2Processor.TaskList.GetRange(minTaskListSize, parent2Processor.TaskList.Count - minTaskListSize));
                }

                childProcessor.TaskList = childTaskList;
                childProcessors.Add(childProcessor);
            }

            //tworzy nowy harmonogram używajać procesoru dziecka
            return new Schedule(childProcessors);
        }

        //metoda mutująca, czyli zmieniająca harmonogram w losowy sposob
        private void Mutate(Schedule schedule)
        {
            foreach (Processor processor in schedule.Processors)
            {
                for (int i = 0; i < processor.TaskList.Count; i++)
                {
                    if (random.NextDouble() < 0.1) // Prawdopodobieństwo mutacji ustawione na 10%
                    {
                        int j = random.Next(processor.TaskList.Count);
                        SwapTasks(processor.TaskList, i, j);
                    }
                }
            }
        }

        //Metoda zamieniająca zadania
        private void SwapTasks(List<Task> taskList, int index1, int index2)
        {
            Task temp = taskList[index1];
            taskList[index1] = taskList[index2];
            taskList[index2] = temp;
        }
    }
}