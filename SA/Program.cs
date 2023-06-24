namespace SA
{
    public static class Program
    {
        public static readonly Random Random = new Random(); // generator liczb losowych

        // metoda przyjmuje funkcję kosztu (costFunc), początkowe rozwiązanie (initialSolution),
        // początkową temperaturę (initialTemperature),
        // współczynnik chłodzenia (coolingRate) i maksymalną liczbę iteracji (maxN).
        // Metoda zwraca wartość optymalnego kosztu.
        public static double StartAnnealing(Func<double[], double> costFunc, double[] initialSolution, 
            double initialTemperature, double coolingRate, int maxN)
        {
            double[] currentSolution = initialSolution; // poczatkowe rozwiązanie
            double currentCost = costFunc(currentSolution); //oblicza koszt aktualnego rozwiazania
            double optimalCost = currentCost; //inicjalizacja najlepszego kosztu jako bieżący koszt
            double[] optimalSolution = currentSolution; //inicjalizacja najlepszego rozwiazania jako aktualne rozwiązanie

            for (var iteration = 0; iteration < maxN; iteration++)
            {
                //Oblicza temperaturę dla bieżącej iteracji, korzystając z wartości początkowej
                //temperatury, współczynnika chłodzenia i numeru iteracji.
                double temperature = initialTemperature / (1 + coolingRate * iteration);

                //Generuje nowe rozwiązanie (newSolution) na podstawie bieżącego rozwiązania,
                //wywołując metodę GenerateNeighbour, która tworzy sąsiada bieżącego rozwiązania.
                double[] newSolution = GenerateNeighbour(currentSolution);

                //Oblicza koszt nowego rozwiązania, wywołując funkcję kosztu costFunc na nowym rozwiązaniu.
                double newCost = costFunc(newSolution);

                //Oblicza różnicę kosztu między nowym a bieżącym rozwiązaniem.
                double deltaCost = newCost - currentCost;

                //Sprawdza, czy różnica kosztu jest mniejsza od zera.
                //Jeśli tak, oznacza to, że nowe rozwiązanie jest lepsze i akceptowane jest automatycznie.
                if (deltaCost < 0)
                {
                    currentSolution = newSolution;
                    currentCost = newCost;

                    //Sprawdza, czy bieżący koszt jest lepszy od optymalnego kosztu.
                    //Jeśli tak, oznacza to, że znaleziono nowe optymalne rozwiązanie.
                    if (currentCost < optimalCost)
                    {
                        optimalSolution = currentSolution;
                        optimalCost = currentCost;
                    }
                }
                else
                {
                    //Oblicza prawdopodobieństwo przejścia do gorszego rozwiązania
                    //na podstawie różnicy kosztu i temperatury.
                    double probability = Math.Exp(-deltaCost / temperature);
                    if (Random.NextDouble() < probability)
                    {
                        currentSolution = newSolution;
                        currentCost = newCost;
                    }
                }
            }
            //Zwraca optymalny koszt
            return optimalCost;
        }

        //Metoda, która generuje sąsiada dla danego rozwiązania.
        private static double[] GenerateNeighbour(double[] solution)
        {
            //Tworzy sąsiada dla każdego elementu rozwiązania,
            //dodając do niego wartość losową z przedziału [-0.5, 0.5].
            double[] neighbour = new double[solution.Length];
            for (var iteration = 0; iteration < solution.Length; iteration++)
            {
                neighbour[iteration] = solution[iteration] + (Random.NextDouble() - 0.5);
            }

            //Zwraca sąsiada
            return neighbour;
        }

        public static void Main(string[] args)
        {
        }
    }
}