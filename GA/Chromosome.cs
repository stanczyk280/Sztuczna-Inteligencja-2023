using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA
{
    public class Chromosome
    {
        public bool[] Genes { get; set; }
        public double Fitness { get; set; }

        public Chromosome(bool[] genes)
        {
            Genes = genes;
            Fitness = 0;
        }
    }
}