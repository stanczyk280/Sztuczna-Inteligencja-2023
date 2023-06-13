namespace GeneralSchedulingProblemSA
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //Dane wejściowe w postaci listy zadań, numer / czas wykonania
            List<Task> tasks = new List<Task>
            {
                new Task(1, 10),
                new Task(2, 15),
                new Task(3, 5),
                new Task(4, 8),
                new Task(5, 12),
                new Task(6, 6)
            };

            // Ustawienie zależności zadań
            tasks[1].Dependencies.Add(0);
            tasks[2].Dependencies.Add(2);
            tasks[3].Dependencies.Add(3);
            tasks[4].Dependencies.Add(4);
            tasks[5].Dependencies.Add(0);

            int numProcessors = 3; // liczba dostępnych procesorów

            Scheduler scheduler = new Scheduler(tasks, numProcessors);
            Schedule bestSchedule = scheduler.FindBestSchedule(iterations: 1000, initialTemperature: 100.0, coolingRate: 0.95); //zastosowanie algorytmu SA

            if (bestSchedule != null)
            {
                Console.WriteLine("Best Schedule:");

                for (int i = 0; i < bestSchedule.Processors.Count; i++)
                {
                    Processor processor = bestSchedule.Processors[i];
                    Console.WriteLine($"\n==Processor {i + 1}==");
                    Console.WriteLine($"Assigned tasks: {string.Join(", ", processor.TaskList.Select(task => "Task " + task.Id))}");

                    foreach (Task task in processor.TaskList)
                    {
                        Console.WriteLine($"Task {task.Id} took to finish: {task.TaskTime}");
                    }
                }

                Console.WriteLine("\nOverall execution time: " + bestSchedule.CalculateMakespan());
            }
            else
            {
                Console.WriteLine("No valid schedule found.");
            }
        }
    }
}