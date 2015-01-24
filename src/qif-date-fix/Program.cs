using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qif_date_fix
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Worker();
            app.Run();
        }
    }
}
