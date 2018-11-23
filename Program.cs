#region Using Statements
using GalagaFramework;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace GalagaFramework
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}

//infos:
// http://www.funtrivia.com/en/VideoGames/Galaga-17658.html
// http://www.ign.com/faqs/2013/galaga-faqguide-436912
