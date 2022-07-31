
using JuanMartin.Kernel.Utilities.DataStructures;
using System.Diagnostics;

namespace JuanMartin.Utilities.Euler.Objects
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "}")]

    public class Passcode
    {
        private readonly DirectedAcyclicGraph<int> graph;

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
                var from = graph.GetVertex(digits[i - 1]);
                var to = graph.GetVertex(digits[i]);

                graph.AddEdge(nameFrom: from.Name, nameTo:to.Name, name: "adjacent", weight: 1);
            }
        }

        public override string ToString() => graph.ToString(true);

        private string GetDebuggerDisplay()
        {
            return string.Empty;
        }
    }
}
