using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticHierarchyProcess
{
    // Class responsible for AHP structure
    [Serializable]
    internal class AHP
    {
        public Node Goal { get; }
        public List<Node> Criteria { get; } = new();
        public List<Node> Subcriteria { get; } = new();
        public List<Node> Alternatives { get; } = new();

        public AHP(string goal)
        {
            Goal = new Node(goal);
        }

        public void AddCriterion(string criterion)
        {
            Node newCriterion = new Node(criterion);
            Goal.AddChild(newCriterion);
            Criteria.Add(newCriterion);
        }

        public void AddSubcriterion(string criterion, string subcriterion)
        {
            Node newSubcriterion = new Node(subcriterion);
            foreach (Node node in Criteria)
            {
                if (node.Name == criterion)
                {
                    node.AddChild(newSubcriterion);
                }
            }
            Subcriteria.Add(newSubcriterion);
        }

        public void AddAlternative(string alternative)
        {
            Node newAlternative = new Node(alternative);
            foreach (Node node in Subcriteria)
            {
                node.AddChild(newAlternative);
            }
            Alternatives.Add(newAlternative);
        }

        public void FinalizeStructure()
        {
            foreach (Node criterion in Criteria)
            {
                if (criterion.Children.Count == 0)
                {
                    foreach (Node alternative in Alternatives)
                    {
                        criterion.AddChild(alternative);
                    }
                }
            }
        }

        public void UpdateAllPriorities()
        {
            Goal.Priority = 1.0;
            foreach (Node criterion in Criteria)
            {
                criterion.Priority = 0.0;
            }
            foreach (Node subcriterion in Subcriteria)
            {
                subcriterion.Priority = 0.0;
            }
            foreach (Node alternative in Alternatives)
            {
                alternative.Priority = 0.0;
            }

            Goal.PropagatePriority();
            foreach (Node criterion in Criteria)
            {
                criterion.PropagatePriority();
            }
            foreach (Node subcriterion in Subcriteria)
            {
                subcriterion.PropagatePriority();
            }
        }

        public void ResetNodes()
        {
            ResetNode(Goal);
            foreach (Node criterion in Criteria)
            {
                ResetNode(criterion);
            }
            foreach (Node subcriterion in Subcriteria)
            {
                ResetNode(subcriterion);
            }
            foreach (Node alternative in Alternatives)
            {
                ResetNode(alternative);
            }
        }

        private void ResetNode(Node node)
        {
            node.Priority = 0.0;
            for (int i = 0; i < node.Children.Count; i++)
            {
                for (int j = 0; j < node.Children.Count; j++)
                {
                    if (i == j)
                    {
                        node.Matrix[i, j] = 1.0;
                    } else
                    {
                        node.Matrix[i, j] = 0.0;
                    }
                }
            }
        }

        public List<Node> GetAllNodes()
        {
            List<Node> result = new();

            result.Add(Goal);
            foreach (Node criterion in Criteria)
            {
                result.Add(criterion);
            }
            foreach (Node subcriterion in Subcriteria)
            {
                result.Add(subcriterion);
            }
            foreach (Node alternative in Alternatives)
            {
                result.Add(alternative);
            }

            return result;
        }

        public string GetGoalName()
        {
            return Goal.Name;
        }

        public List<string> GetCriterionNames()
        {
            List<string> result = new();

            foreach (Node criterion in Criteria)
            {
                result.Add(criterion.Name);
            }

            return result;
        }

        public List<string> GetAllSubcriterionNames()
        {
            List<string> result = new();

            foreach (Node subcriterion in Subcriteria)
            {
                result.Add(subcriterion.Name);
            }

            return result;
        }

        public List<string> GetSubcriterionNames(string criterion)
        {
            List<string> result = new();

            foreach (Node node in Criteria)
            {
                if (node.Name == criterion)
                {
                    foreach (Node subcriterion in node.Children)
                    {
                        result.Add(subcriterion.Name);
                    }
                }
            }

            return result;
        }

        public List<string> GetAlternativeNames()
        {
            List<string> result = new();

            foreach (Node alternative in Alternatives)
            {
                result.Add(alternative.Name);
            }

            return result;
        }

        public bool HasSubcriteria(Node criterion)
        {
            return Subcriteria.Contains(criterion.Children[0]);
        }
    }
}
