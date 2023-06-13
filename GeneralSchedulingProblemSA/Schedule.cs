using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralSchedulingProblemSA
{
    public class Schedule
    {
        public List<Processor> Processors { get; set; } // list dostępnych procesorów do realizacji harmonogramu

        public Schedule()
        {
            Processors = new List<Processor>();
        }

        public int CalculateMakespan()
        {
            return Processors.Max(p => p.TaskList.Sum(t => t.TaskTime)); // funkcja obliczająca maksymalny czas wykonania zadań dla procesora
        }
    }
}