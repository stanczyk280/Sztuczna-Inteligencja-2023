namespace GeneralSchedulingProblemGEN
{
    public static class Program
    {
        public static void Main()
        {
            List<Task> tasks = new List<Task>
        {
            new Task(0, 10),
            new Task(1, 15),
            new Task(2, 5),
            new Task(3, 8),
            new Task(4, 12)
        };

            tasks[1].Dependencies.Add(0);
            tasks[2].Dependencies.Add(1);
            tasks[3].Dependencies.Add(0);
            tasks[3].Dependencies.Add(2);
            tasks[4].Dependencies.Add(1);

            int numProcessors = 2;
            int populationSize = 10;
            int maxGenerations = 100;

            GeneticScheduler scheduler = new GeneticScheduler(tasks, numProcessors, populationSize, maxGenerations);
            Schedule bestSchedule = scheduler.FindBestSchedule();

            for (int i = 0; i < numProcessors; i++)
            {
                Processor processor = bestSchedule.Processors[i];
                Console.WriteLine($"==Processor {processor.Id + 1}==");
                Console.Write("Assigned tasks: ");
                Console.WriteLine(string.Join(", ", processor.TaskList.Select(task => $"task{task.Id + 1}")));
                foreach (Task task in processor.TaskList)
                {
                    Console.WriteLine($"Task {task.Id + 1} took to finish: {task.TaskTime}");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Overall execution time: {bestSchedule.CalculateMakespan()}");
        }
    }
}