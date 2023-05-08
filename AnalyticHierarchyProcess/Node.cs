using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticHierarchyProcess
{
    // Class that represents a node in AHP hierarchy
    [Serializable]
    public class Node
    {
        public string Name { get; }
        public List<Node> Children { get; private set; } = new();
        public SquareMatrix Matrix { get; private set; } = new(0);
        public double Priority { get; set; } = 0.0;

        public Node(string name)
        {
            Name = name;
        }

        public void AddChild(Node child)
        {
            Children.Add(child);
            Matrix.Resize(Matrix.Size + 1);
            for (int i = 0; i < Matrix.Size; i++)
            {
                Matrix[i, Matrix.Size - 1] = 0.0;
            }
            for (int i = 0; i < Matrix.Size; i++)
            {
                Matrix[Matrix.Size - 1, i] = 0.0;
            }
            Matrix[Matrix.Size - 1, Matrix.Size - 1] = 1.0;
        }

        public void PropagatePriority()
        {
            List<double> eigenvector = Matrix.GetApproximatePrincipalEigenvector();
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Priority += eigenvector[i] * Priority;
            }
        }

        public double[] GetPropagatedPriorities()
        {
            double[] result = new double[Children.Count];
            List<double> eigenvector = Matrix.GetApproximatePrincipalEigenvector();
            for (int i = 0; i < Children.Count; i++)
            {
                result[i] = eigenvector[i] * Priority;
            }

            return result;
        }
    }
}
