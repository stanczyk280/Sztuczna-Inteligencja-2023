using Microsoft.ML.Probabilistic.Models;
using Microsoft.ML.Probabilistic.Distributions;
using Range = Microsoft.ML.Probabilistic.Models.Range;

namespace BayesCoin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Variable<double> Probability = Variable.Beta(1, 1);
            Range tries = new Range(100); // ilość rzutów monetą
            VariableArray<bool> round = Variable.Array<bool>(tries);

            int heads = 0;
            int tails = 0;

            //prior dla parametru sukcesu
            round[tries] = Variable.Bernoulli(Probability).ForEach(tries);

            //wygeneruj 100 wyników rzutów
            Random random = new Random();

            bool[] randomBools = new bool[100];

            for (int i = 0; i < randomBools.Length; i++)
            {
                bool randomBool = random.Next(2) == 0;
                randomBools[i] = randomBool;
            }

            round.ObservedValue = randomBools;

            //oblicz wyniki
            for (int i = 0; i < randomBools.Length; i++)
            {
                if (round.ObservedValue[i] == true)
                {
                    heads++;
                }
                else
                {
                    tails++;
                }
            }

            //Wnioskowanie
            InferenceEngine engine = new InferenceEngine();
            Beta inferredProbability = engine.Infer<Beta>(Probability);

            //Wyniki
            Console.WriteLine("Average of throws: " + inferredProbability.GetMean());
            Console.WriteLine("Heads: " + heads + " Tails: " + tails);
        }
    }
}