using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticHierarchyProcess
{
    // Class that represents a square matrix
    [Serializable]
    public class SquareMatrix
    {
        public int Size { get; private set; }

        private double[,] data;
        public SquareMatrix(int size)
        {
            Size = size;
            data = new double[size, size];
        }

        public void Resize(int newSize)
        {
            double[,] newData = new double[newSize, newSize];

            for (int i = 0; i < Math.Min(newSize, Size); i++)
            {
                for (int j = 0; j < Math.Min(newSize, Size); j++)
                {
                    newData[i, j] = data[i, j];
                }
            }

            Size = newSize;

            data = newData;
        }

        public double this[int i, int j]
        {
            get => data[i, j];
            set => data[i, j] = value;
        }

        public List<double> GetApproximatePrincipalEigenvector()
        {
            double[,] temp = (double[,]) data.Clone();
            for (int j = 0; j < Size; j++)
            {
                double sum = 0.0;

                for (int i = 0; i < Size; i++)
                {
                    sum += data[i, j]; 
                }

                for (int i = 0; i < Size; i++)
                {
                    temp[i, j] /= sum;
                }
            }

            List<double> eigenvector = new();

            for (int i = 0; i < Size; i++)
            {
                double value = 0.0;
                for (int j = 0; j < Size; j++)
                {
                    value += temp[i, j] / Size;
                }

                eigenvector.Add(value);
            }

            return eigenvector;
        }

        public double GetApproximatePrincipalEigenvalue()
        {
            double[,] temp = (double[,])data.Clone();

            List<double> sums = new();

            for (int j = 0; j < Size; j++)
            {
                double sum = 0.0;

                for (int i = 0; i < Size; i++)
                {
                    sum += data[i, j];
                }

                sums.Add(sum);

                for (int i = 0; i < Size; i++)
                {
                    temp[i, j] /= sum;
                }
            }

            List<double> eigenvector = new();

            for (int i = 0; i < Size; i++)
            {
                double value = 0.0;
                for (int j = 0; j < Size; j++)
                {
                    value += temp[i, j] / Size;
                }

                eigenvector.Add(value);
            }

            double result = 0.0;

            for (int i = 0; i < Size; i++)
            {
                result += sums[i] * eigenvector[i];
            }

            return result;
        }
    }
}
