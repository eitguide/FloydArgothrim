using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floyd
{
    class Program
    {
        static void Main(string[] args)
        {
            int start = 0;
            int end = 1;
            String[] testCase = new String[] { "data.txt", "data1.txt", "data2.txt", "data3.txt" };
            FloydArgothrim floydArgothrim = new FloydArgothrim();
            floydArgothrim.InitAdjMatrixFromFile(testCase[0]);
            floydArgothrim.IsStepByStep = true;
            FloydArgothrim.DisplayMatrix(floydArgothrim.AdjMatrix, floydArgothrim.NumberPoint, "AdjMatrix");

            floydArgothrim.ExcuteFloydArgothrim();
            FloydArgothrim.DisplayMatrix(floydArgothrim.DistanceMatrix, floydArgothrim.NumberPoint, "DistanceMatrix");
            FloydArgothrim.DisplayMatrix(floydArgothrim.BacktrackMatrix, floydArgothrim.NumberPoint, "BacktrackMatrix");


            Console.WriteLine("From " + start + " to " + end);
            List<int> Path = floydArgothrim.FloyFindPath(start, end);
            Console.WriteLine("TotalDistance: " + floydArgothrim.TotalDistance);

            Console.ReadLine();
        }
    }
}
