using GA;

public static class Program
{
    public static void Main(string[] args)
    {
        GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(
            populationSize: 50,
            chromosomeLength: 8,
            crossoverProbability: 0.8,
            mutationProbability: 0.05,
            fitnessFunction: x => x * x
        );

        double result = geneticAlgorithm.Run(numGenerations: 100);

        Console.WriteLine("Result: " + result);
    }
}