
using JuanMartin.Kernel.Utilities.DataStructures;

namespace JuanMartin.Utilities.Euler
{
    public class Passcode
    {
        private DirectedAcyclicGraph<int> graph;

        public Path<int> Code => graph.GetLongestPath();

        public Passcode()
        {
            graph = new DirectedAcyclicGraph<int>();
        }
        /// <summary>
        /// Add digits passed as vertices in DAG graph and if they come togheter add edge between pairs
        /// </summary>
        /// <param name="d"></param>
        public void Insert(int[] digits)
        {
            foreach (var d in digits)
            {
                graph.AddVertex(d);
            }
            for (var i = 1; i < digits.Length; i++)
            {
                graph.AddEdge(graph.GetVertex(digits[i - 1].ToString()), graph.GetVertex(digits[i].ToString()), Edge<int>.EdgeDirection.bidirectional, "adjacent", 1);
            }
        }

        public override string ToString() => graph.ToString(true);
    }
}
