namespace SA
{
    public static class Program
    {
        public static readonly Random Random = new Random();

        public static double StartAnnealing(Func<double[], double> costFunc, double[] initialSolution, double initialTemperature, double coolingRate, int maxN)
        {
            double[] currentSolution = initialSolution;
            double currentCost = costFunc(currentSolution);
            double optimalCost = currentCost;
            double[] optimalSolution = currentSolution;

            for (var iteration = 0; iteration < maxN; iteration++)
            {
                double temperature = initialTemperature / (1 + coolingRate * iteration);
                double[] newSolution = GenerateNeighbour(currentSolution);
                double newCost = costFunc(newSolution);

                double deltaCost = newCost - currentCost;
                if (deltaCost < 0)
                {
                    currentSolution = newSolution;
                    currentCost = newCost;

                    if (currentCost < optimalCost)
                    {
                        optimalSolution = currentSolution;
                        optimalCost = currentCost;
                    }
                }
                else
                {
                    double probability = Math.Exp(-deltaCost / temperature);
                    if (Random.NextDouble() < probability)
                    {
                        currentSolution = newSolution;
                        currentCost = newCost;
                    }
                }
            }
            return optimalCost;
        }

        private static double[] GenerateNeighbour(double[] solution)
        {
            double[] neighbour = new double[solution.Length];
            for (var iteration = 0; iteration < solution.Length; iteration++)
            {
                neighbour[iteration] = solution[iteration] + (Random.NextDouble() - 0.5);
            }
            return neighbour;
        }

        public static void Main(string[] args)
        {
        }
    }
}