using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralSchedulingProblemGEN
{
    public class Schedule
    {
        public List<Processor> Processors { get; set; } // lista dostępnych procesorów

        public Schedule(List<Processor> processors)
        {
            Processors = processors;
        }

        public int CalculateMakespan() // metoda obliczająca ogólny czas wykonania zadań
        {
            int maxEndTime = 0;

            foreach (Processor processor in Processors)
            {
                int endTime = processor.TaskList.Sum(task => task.TaskTime);
                if (endTime > maxEndTime)
                {
                    maxEndTime = endTime;
                }
            }

            return maxEndTime;
        }
    }
}