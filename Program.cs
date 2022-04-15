using System;

namespace Graph
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Random random = new Random();
            int size = 12, maxWeight = 10;
            var testGraph = new ASD.Graphs.DiGraph<int>(size);
            for (int i = 0; i < size; i++)
            {
                int v = random.Next(size);
                if (v == i)
                    v = (v + 1) % size;
                testGraph.AddEdge(i, v, random.Next(maxWeight));
            }
            //testGraph.AddEdge(0, 1, 1);
            //testGraph.AddEdge(1, 0, 1);
            //testGraph.AddEdge(1, 3, 1);
            //testGraph.AddEdge(1, 5, 1);
            //testGraph.AddEdge(6, 11, 1);
            //testGraph.AddEdge(11, 6, 1);
            Visualize.ShowGraph(testGraph);
        }
    }
}
