using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Assets.AssetProvider = new WindowsAssetProvider();
            Engine.Manager.Instance = new Implementation.TestManager();
            new MainWindow().Run(60);
        }
    }
}
