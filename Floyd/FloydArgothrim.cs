using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floyd
{
    public class FloydArgothrim
    {
        public int[][] AdjMatrix { get; set; }
        public int[][] DistanceMatrix { get; set; }
        public int[][] BacktrackMatrix { get; set; }

        public const int DISTANCE_MAX = int.MaxValue;
        public bool IsStepByStep { get; set; }

        public int TotalDistance { get; set; }
        public int NumberPoint { get; set; }
        public FloydArgothrim()
        {

        }
        /// <summary>
        /// Read adjacency matrix from file and init DistanceMatrix and BacktrackMatrix via Adjmatrix
        /// </summary>
        /// <param name="fileName">Name of File</param>
        public void InitAdjMatrixFromFile(String fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    this.NumberPoint = int.Parse(reader.ReadLine());

                    this.AdjMatrix = new int[this.NumberPoint][];
                    this.DistanceMatrix = new int[this.NumberPoint][];
                    this.BacktrackMatrix = new int[this.NumberPoint][];

                    for (int i = 0; i < NumberPoint; i++)
                    {
                        this.AdjMatrix[i] = new int[this.NumberPoint];
                        this.DistanceMatrix[i] = new int[this.NumberPoint];
                        this.BacktrackMatrix[i] = new int[this.NumberPoint];
                    }

                    string s = string.Empty;
                    int rows = 0, columns = 0;
                    while ((s = reader.ReadLine()) != null)
                    {
                        int temp = 0;
                        string[] content = s.Split(new char[] { ' ' });
                        columns = 0;
                        foreach (string t in content)
                        {
                            if (t.Trim() != "")
                            {
                                int iValue = int.Parse(content[temp]);
                                AdjMatrix[rows][columns] = iValue;
                                temp++;
                            }
                            columns++;
                        }
                        rows++;
                    }
                    InitDistanceAndBacktrackMatrix();
                }
            }
        }

        public void ExcuteFloydArgothrim()
        {
            if (IsStepByStep)
            {
                DisplayMatrix(DistanceMatrix, NumberPoint, "DistanceMatrix");
                DisplayMatrix(BacktrackMatrix, NumberPoint, "BacktrackNumber");
            }
            for (int k = 0; k < NumberPoint; k++)
            {
                for (int i = 0; i < NumberPoint; i++)
                {
                    for (int j = 0; j < NumberPoint; j++)
                    {
                        if (i != j)
                        {
                            int d = DistanceMatrix[i][k] + DistanceMatrix[k][j];
                            if (IsStepByStep)
                            {
                                String data;

                                if (d > 0)
                                {
                                    data = "d[" + i + ", " + j + "] (" + DistanceMatrix[i][j] + ") > d[" + i + ", " + k + "] + d[" + k + ", " + j + "] (" + d + ")";
                                    Console.Write(data + "\t");
                                    WriteFile(data + "\t");
                                }

                                else
                                {
                                    data = "d[" + i + ", " + j + "] (" + DistanceMatrix[i][j] + ") > d[" + i + ", " + k + "] + d[" + k + ", " + j + "] (M)";
                                    Console.Write(data + "\t");
                                    WriteFile(data + "\t");
                                }

                            }


                            if (d > 0 && DistanceMatrix[i][j] > d)
                            {

                                DistanceMatrix[i][j] = d;
                                BacktrackMatrix[i][j] = k;
                                if (IsStepByStep)
                                {
                                    Console.WriteLine("True -> Update");
                                    WriteFile("True -> Update" + Environment.NewLine);
                                    DisplayMatrix(DistanceMatrix, NumberPoint, "DistanceMatrix");
                                    DisplayMatrix(BacktrackMatrix, NumberPoint, "BacktrackNumber");
                                    WriteFile(CreateStringFromMatrix(DistanceMatrix, NumberPoint, "DistanceMatrix"));
                                    WriteFile(CreateStringFromMatrix(BacktrackMatrix, NumberPoint, "BacktrackMatrix"));

                                }
                            }
                            else
                            {
                                if (IsStepByStep)
                                {
                                    Console.WriteLine("False -> Don't Change");
                                    WriteFile("False -> Don't Change" + Environment.NewLine);
                                }

                            }
                        }
                    }
                }
            }
        }

        private void InitDistanceAndBacktrackMatrix()
        {
            for (int i = 0; i < NumberPoint; i++)
            {
                for (int j = 0; j < NumberPoint; j++)
                {
                    if (AdjMatrix[i][j] == 0)
                        DistanceMatrix[i][j] = DISTANCE_MAX;
                    else
                        DistanceMatrix[i][j] = AdjMatrix[i][j];
                }
            }

            for (int i = 0; i < NumberPoint; i++)
            {
                for (int j = 0; j < NumberPoint; j++)
                {
                    if (DistanceMatrix[i][j] == DISTANCE_MAX)
                        BacktrackMatrix[i][j] = DISTANCE_MAX;
                    else
                        BacktrackMatrix[i][j] = j;
                }
            }
        }

        public List<int> FloyFindPath(int start, int end)
        {
            if (DistanceMatrix[start][end] == DISTANCE_MAX)
                Console.WriteLine("Not Found Path From " + start + " to " + end);
            else
            {
                List<int> path = new List<int>();
                int next = start;
                path.Add(start);
                while (true)
                {
                    Console.Write(next + "-> ");
                    if (next == end)
                        break;

                    next = BacktrackMatrix[next][end];
                    path.Add(next);
                }
                Console.WriteLine();
                TotalDistance = DistanceMatrix[start][end];

                return path;
            }

            return null;
        }

        public static void DisplayMatrix(int[][] matrix, int n, String name)
        {
            Console.WriteLine(name);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matrix[i][j] == DISTANCE_MAX)
                        Console.Write("M\t");
                    else
                        Console.Write(matrix[i][j] + "\t");
                }
                Console.WriteLine();
            }
        }

        public void WriteFile(String data)
        {
            using (FileStream fileStream = new FileStream("log.txt", FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(data);
                }
            }
        }

        public static string CreateStringFromMatrix(int[][] matrix, int n, String type)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(type + Environment.NewLine);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matrix[i][j] == DISTANCE_MAX)
                        builder.Append("M\t");
                    else
                        builder.Append(matrix[i][j] + "\t");
                }
                builder.Append(Environment.NewLine);
            }

            return builder.ToString();

        }
    }


}
