using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GeneralSchedulingProblemSA
{
    public class Task
    {
        public int Id { get; set; } // numer zadania
        public int TaskTime { get; set; } // jak długo trwa wykonanie zadania
        public List<int> Dependencies { get; set; } // lista zależności zadania od innych zadań

        //konstruktor
        public Task(int id, int taskTime)
        {
            Id = id;
            TaskTime = taskTime;
            Dependencies = new List<int>();
        }
    }
}