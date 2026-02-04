using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace testtest
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 2. Initialize the Global Style
            KryptonManager kryptonManager = new KryptonManager();

            // 3. Set your desired Palette Mode
            // Common styles: Office365Blue, Office2010Blue, SparkleBlue, ProfessionalSystem
            kryptonManager.GlobalPaletteMode = PaletteMode.SparkleBlue;

            Application.Run(new LoginForm());
        }
    }
}

