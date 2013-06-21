using NUnit.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Wpf
{
    class DebugStarter
    {
        static void Main(string[] args)
        {
            try
            {
                AppEntry.Main(args);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }
        }
    }
}
