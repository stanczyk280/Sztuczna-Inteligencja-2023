using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralSchedulingProblemGEN
{
    public class Processor
    {
        public int Id { get; set; } // numer procesora
        public List<Task> TaskList { get; set; } // lista zadań przekzanych do procesora

        public Processor(int id)
        {
            Id = id;
            TaskList = new List<Task>();
        }
    }
}