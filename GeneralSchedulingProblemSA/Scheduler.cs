using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralSchedulingProblemSA
{
    public class Scheduler //klasa realizująca alogrytm symulowanego wyrzażania
    {
        private List<Task> tasks;  // lista dostępnych zadan do wykonania
        private int numProcessors; // ilość procesorów, które mają wykonać te zadania

        public Scheduler(List<Task> tasks, int numProcessors)
        {
            this.tasks = tasks;
            this.numProcessors = numProcessors;
        }

        public Schedule FindBestSchedule(int iterations, double initialTemperature, double coolingRate) //realizacja algorytmu SA
        {
            Schedule currentSchedule = CreateInitialSchedule(); // Stworzenie początkowego harmonogramu
            Schedule bestSchedule = currentSchedule; //aktualny harmonogram staje się najlepszym harmonogramem bo jeszcze nie znamy lepszego

            double temperature = initialTemperature; // inicjalizacja początkowej temperatury dla algorytmu

            for (int i = 0; i < iterations; i++) // pętla zmieniająca aktualny harmonogram, obraca się tyle razy i podaliśmy mu iteracji, domyślnie 1000
            {
                Schedule newSchedule = MutateSchedule(currentSchedule);
                int currentMakespan = currentSchedule.CalculateMakespan(); // obliczenie czasu wykonania zadań w aktualnym harmonogramie
                int newMakespan = newSchedule.CalculateMakespan();

                if (AcceptanceProbability(currentMakespan, newMakespan, temperature) > RandomDouble()) // jeżeli czas wykonania mieści w przedziale akceptacji, który losowo jest generowany z liczb 0 do 1 to aktualny harmonogram jest aktualizowany do nowego harmonogramu
                {
                    currentSchedule = newSchedule;
                    if (newMakespan < bestSchedule.CalculateMakespan())
                    {
                        bestSchedule = newSchedule;
                    }
                }

                temperature *= coolingRate; //zmniejszenie temperatury
            }

            return bestSchedule; //zwraca najlepszy harmonogram
        }

        private Schedule CreateInitialSchedule() // metoda rozdziela porówno zadania pomiędzy wszystkie procesory
        {
            Schedule schedule = new Schedule();

            for (int i = 0; i < numProcessors; i++)
            {
                schedule.Processors.Add(new Processor());
            }

            int nextProcessorIndex = 0;
            foreach (Task task in tasks)
            {
                schedule.Processors[nextProcessorIndex].TaskList.Add(task);
                nextProcessorIndex = (nextProcessorIndex + 1) % numProcessors;
            }

            return schedule;
        }

        private Schedule MutateSchedule(Schedule schedule) // metoda tworzy nowy harmonogram poprzez zmiane przekazanego jej harmonogramu, tworzy nowy obiekt i kopiuje zadania z poprzedniego do nowego harmonogramy, następnie w losowy sposób zmienia kolejność tych zadań wprowadzając w ten sposób wariacje
        {
            Schedule newSchedule = new Schedule();

            foreach (Processor processor in schedule.Processors)
            {
                Processor newProcessor = new Processor();
                foreach (Task task in processor.TaskList)
                {
                    newProcessor.TaskList.Add(new Task(task.Id, task.TaskTime));
                }
                newSchedule.Processors.Add(newProcessor);
            }

            Random random = new Random();

            foreach (Processor processor in newSchedule.Processors)
            {
                int taskIndex1 = random.Next(processor.TaskList.Count);
                int taskIndex2 = random.Next(processor.TaskList.Count);
                SwapTasks(processor.TaskList, taskIndex1, taskIndex2);
            }

            return newSchedule;
        }

        private void SwapTasks(List<Task> taskList, int index1, int index2) // metoda zamieniająca kolejność zadań
        {
            Task temp = taskList[index1];
            taskList[index1] = taskList[index2];
            taskList[index2] = temp;
        }

        // metoda oblicza prawdopodobieństwo akceptacji poprzez porównanie aktualnego czasu wykonania zadań i nowego czasu wykonania zadań oraz temperatury.
        // Jeżeli nowy czas wykonania jest mniejszy niż aktualny to prawdopodbieństwo ustawiane jest na 1.0 czyli akceptacja nowego harmonogramu.
        // W innym przypadku oblicza go za pomocą funcki wykładniczej
        private double AcceptanceProbability(int currentMakespan, int newMakespan, double temperature)
        {
            if (newMakespan < currentMakespan)
            {
                return 1.0;
            }
            else
            {
                return Math.Exp((currentMakespan - newMakespan) / temperature);
            }
        }

        private double RandomDouble() // losowanie liczby
        {
            Random random = new Random();
            return random.NextDouble();
        }
    }
}