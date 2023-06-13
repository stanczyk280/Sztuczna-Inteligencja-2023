using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralSchedulingProblemSA
{
    public class Processor
    {
        public List<Task> TaskList { get; set; } //lista zadan przydzielonych do procesora

        public Processor()
        {
            TaskList = new List<Task>();
        }
    }
}