using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralSchedulingProblemGEN
{
    public class Task
    {
        public int Id { get; set; } // numer zadania
        public int TaskTime { get; set; } // czas wymagany do ukonczenia zadania
        public List<int> Dependencies { get; set; } // lista zależności między zadaniami

        public Task(int id, int taskTime)
        {
            Id = id;
            TaskTime = taskTime;
            Dependencies = new List<int>();
        }
    }
}