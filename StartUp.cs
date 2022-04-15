using System.Windows.Forms;

namespace Graph
{
    public static class Visualize
    {
        public static void ShowGraph(ASD.Graphs.IGraph<double> graph)
        {

            if (graph == null)
            {
                ShowGraph();
                return;
            }
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GraphForm(graph));
        }
        public static void ShowGraph(ASD.Graphs.IGraph<int> graph)
        {

            if (graph == null)
            {
                ShowGraph();
                return;
            }
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GraphForm(graph));
        }

        public static void ShowGraph()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GraphForm());
        }
    }
}
