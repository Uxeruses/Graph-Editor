using System.Windows.Forms;

namespace Graph
{
    public static class Visualize
    {
        public static void ShowGraph()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GraphForm());
        }
    }
}
