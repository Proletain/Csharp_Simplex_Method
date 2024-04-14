using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApp9
{
    internal class Program
    {
        static void PrintSimplexTable(double[,] matrix, double[,] showmat, double[] Cjs, int[] Xbase, int rows, int cols, int xb, double[] results)//form the simplex format and output
        {//forming and presenting the full simplex table
            showmat = new double[rows, cols + 2];//defining the display chat

            for (int i = 0; i < rows; i++)//coefficients of base variables
            {
                showmat[i, 0] = Cjs[Xbase[i]];
            }
            for (int i = 0; i < rows; i++)//display which variables are the base variables
            {
                showmat[i, 1] = Xbase[i];
            }
            for (int i = 0; i < rows; i++)//assign the matrix in to the display chart
            {
                for (int j = 2; j < cols + 2; j++)
                {
                    showmat[i, j] = matrix[i, j - 2];
                }
            }
            Console.Write("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=\nCj:         ");
            for (int i = 0; i < cols - 1; i++)
            {
                Console.Write($"{Cjs[i],7:F3}|");
            }
            Console.WriteLine("\nC 基b| variables");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols + 2; j++)
                {
                    Console.Write($"{showmat[i, j],7:F3}|");
                }
                Console.WriteLine();
            }
            Console.Write("\nCj-Zj|");
            for (int j = 0; j < cols - 1; j++)
            {
                double zj = 0;
                for (int i = 0; i < rows; i++)
                {
                    zj = zj + Cjs[Xbase[i]] * matrix[i, j + 1];
                }
                results[j] = Cjs[j] - zj;
                Console.Write($"{results[j],7:F3}|");
            }
            Console.WriteLine("\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
        }
        static int FindMax(double[] array)//Find the LOCATION of maximum values
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("数组不能为空或长度不能为零。");
            }
            int maxIndex = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > array[maxIndex])
                    maxIndex = i;
            }
            return maxIndex;
        }

        static int FindMin(double[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("数组不能为空或长度不能为零。");
            }
            int minIndex = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < array[minIndex])
                    minIndex = i;
            }
            return minIndex;
        }

        static void SimplexCalculation(double[,] matrix, double[,] showmat, double[] Cjs, int[] Xbase, int rows, int cols, int xb, double[] results)// from judging to indentifying
        {
            int entVal = FindMax(results);      //finding entering variables
            double[] setae = new double[rows];// finding leaving/exiting variables
            for (int i = 0; i < rows; i++)
            {
                double cal = 0;
                cal = matrix[i, 0] / matrix[i, entVal + 1];
                setae[i] = cal;
            }
            int levVal = FindMin(setae);//Console.WriteLine("we found the target at ({0},{1})",levVal+1,entVal+1);

            Xbase[levVal] = entVal;
            
            for (int i = 0; i < rows; i++)
            {
                if (i != levVal) // 对非换出行进行操作
                {
                    double factor = matrix[i, entVal + 1] / matrix[levVal, entVal + 1];//for division of Suptraktion
                    for (int j = 0; j < cols; j++)
                    {
                        matrix[i, j] = matrix[i, j] - factor * matrix[levVal, j];
  //                      Console.WriteLine("X({0},{1}) 更新后的值为 {2:F3}", i, j, matrix[i, j]);
                    }
                }
                else // 对换出行进行操作
                {
                    double divisor = matrix[levVal, entVal + 1];
                    if (divisor == 0)
                    {
                        Console.WriteLine("错误：主元为零，无法进行行变换。");
                        continue; // 跳过这一轮循环，防止除以零
                    }
                    for (int j = 0; j < cols; j++)
                    {
                        matrix[i, j] /= divisor;
                    }
  //                 Console.WriteLine("换出行 {0} 被标准化。", i);
                }
            }

        }
        static bool finalsimplextableau(double[] results)
        {
            bool judg = true;
            for (int j=0; j < results.Length;j++)
            {
                if (results[j]> 0)
                {
                    judg = false;
                    break;
                }
            }
            return judg;
        }
        static void Main(string[] args)
        {          
            //defining and initializing the Matrix
            Console.WriteLine("请输入变量个数");
            int xb = int.Parse(Console.ReadLine());
            Console.WriteLine("请输入松弛变量个数");
            int xs = int.Parse(Console.ReadLine());


            int rows = xs;          //rows,行数    ---
            int cols = xs + xb + 1;     //columns,列数  |     （包含b的那列！）

            double[] Cjs = new double[cols];
            double[,] matrix = new double[rows, cols];
            //inputing Cj
            Console.WriteLine("请输入Cj");
            for (int i = 0; i < xb; i++)
            {
                Cjs[i] = double.Parse(Console.ReadLine());
            }

            Console.WriteLine("请输入非人工变量的系数");       //entering the content of the Matrix
            for (int i = 0; i < rows; i++)
            {
                for (int j = 1; j < xb + 1; j++)
                {
                    double input = double.Parse(Console.ReadLine());
                    matrix[i, j] = input;

                }
            }
            //making matrix Identity matrix
            for (int i = 0; i < xs; i++)
            {
                matrix[i, i + xb +1] = 1;
//debug                Console.WriteLine("{0}ste mal,assign to matrix[{1},{2}]",i,i,i+xb+1);
            }
            Console.WriteLine("请输入b");
            for (int i = 0; i < rows; i++)
            {
                matrix[i, 0] = double.Parse(Console.ReadLine());
            }

            int[] Xbase = new int[cols];                        //initializing base: which variables are bases
            for (int i = 0; i < xs; i++)
            {
                Xbase[i] = i + xb + 1;
            }

            double[] results = new double[cols-1];
            double[,] showmat = new double[rows, cols + 2];
            PrintSimplexTable(matrix, showmat, Cjs, Xbase, rows, cols, xb,results);
            Console.WriteLine("确认单纯形表无误吗？开始运算请输入Y，否则输入其他字符以重启输入程序");
            string confirm = Console.ReadLine();confirm = confirm.Trim().ToUpper();
            if ((confirm == "Y") == false)
            {
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                Environment.Exit(0); // exit with code 0 (success)
            }
            else
            {
                Console.WriteLine("开始执行标准化");
            }
            bool goOn = !finalsimplextableau(results);
            do
            {
                PrintSimplexTable(matrix, showmat, Cjs, Xbase, rows, cols, xb, results);
                SimplexCalculation(matrix, showmat, Cjs, Xbase, rows, cols, xb, results);
                goOn = !finalsimplextableau(results); 
                if (goOn) { Console.WriteLine("继续迭代"); } else { Console.WriteLine("已得出最佳单纯形表"); }
            } while (goOn);
            Console.WriteLine(); Console.ReadKey();
        }
    }
}
