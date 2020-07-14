using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AINumber.Neural
{
    public interface IFunction
    {
        public void Function();
        public void OutWeight();
    }

    public interface ILearn
    {
        public void Function();
        public void OutWeight();
        public void Learn();
    }

    public interface IInputData
    {
        public void GetData(int count);
    }



    public class CSVFileGetData : IInputData
    {
        public CSVFileGetData()
        {
            OpenFile();
        }
        public List<double> Data { get; private set; } = new List<double>();

        public List<double> TrueAnswer { get; private set; } = new List<double>();

        public int TrueAnswerNumber { get; private set; }

        string[] ReadLines { get; set; }

        void OpenFile()
        {
            ReadLines = File.ReadAllLines("./file.csv");
        }

        public void GetData(int count)
        {
            Data.Clear();
            string[] numbers = ReadLines[count].Split(',');
            TrueAnswerNumber = Convert.ToInt32(numbers[0]);
            TrueAnswer.Clear();


            for (int i = 0; i < 10; i++)
            {
                if (i == TrueAnswerNumber)
                    TrueAnswer.Add(0.99);
                else
                    TrueAnswer.Add(0.01);
            }

            //Console.WriteLine(numbers.Length);

            for (int i = 1; i < numbers.Length; i++)
            {
                //Console.WriteLine("data"+(((Convert.ToDouble(numbers[i]) / 255.0) * 0.99) + 0.01));
                Data.Add(((Convert.ToDouble(numbers[i]) / 255.0 )* 0.99) + 0.01);
            }

            //foreach (double n in Data)
            //{
            //    Console.WriteLine(n);
            //}
            //Console.WriteLine("Answer-------------------------------");
            //foreach (double n in TrueAnswer)
            //{
            //    Console.WriteLine(n);
            //}


            //Console.WriteLine(TrueAnswerNumber);
        }
    }

    public class SygmoidNeural : IFunction, ILearn
    {
        public SygmoidNeural(Weight[] weight, Weight[] previousWeight)
        {
            Weight = weight;
            PreviousWeight = previousWeight;
        }

        public List<SygmoidNeural> NextLayer { get; set; } = new List<SygmoidNeural>();

        public List<SygmoidNeural> PreviousLayer { get; set; } = new List<SygmoidNeural>();

        public double X { get; set; } = 0;

        public double XResult { get; set; } = 0;

        public Weight[] Weight { get; set; }

        double SumWeight { get; set; } = 0;

        public Weight[] PreviousWeight { get; set; }

        public double ErrorNumber { get; set; } = 0;

        public double ErrorNumberLayer { get; set; } = 0;



        public void Function()
        {
            if (X == 0)
            {

            }
            //Console.WriteLine(X+" X");
            XResult = 1 / (1 + Math.Exp(-1 * X));
            //Console.WriteLine(XResult+" X Sygmoid");
            X = 0;
        }

        public void OutWeight()
        {
            if (NextLayer != null)
            {
                for (int i = 0; i < NextLayer.Count; i++)
                {
                    NextLayer[i].X += Weight[i].weight * XResult;
                    if (NextLayer[i].X == 0)
                    {

                    }
                }
            }
        }

        public void Learn()
        {
            if (PreviousWeight != null)
            {
                Error();
                WeightChange();
            }
            ErrorNumberLayer = ErrorNumber;
            ErrorNumber = 0;
        }

        void Error()
        {
            if (PreviousLayer != null)
            {
                if (PreviousLayer[0].PreviousLayer != null)
                {
                    SumWeight = 0;
                    foreach (Weight n in PreviousWeight)
                    {
                        SumWeight += n.weight;
                    }
                    for (int i = 0; i < PreviousLayer.Count; i++)
                    {
                        double coef = PreviousWeight[i].weight / SumWeight;
                        PreviousLayer[i].ErrorNumber += ErrorNumber * coef;
                    }
                }
            }
        }

        void WeightChange()
        {
            for (int i = 0; i < PreviousLayer.Count; i++)
            {
                //Console.WriteLine(PreviousWeight[i].weight + "before");
                //Console.WriteLine((1.0 - XResult));
                //Console.WriteLine(ErrorNumber);
                //Console.WriteLine(1 * ErrorNumber * XResult * (1.0 - XResult) * PreviousLayer[i].XResult);
                PreviousWeight[i].weight += (-1 * ErrorNumber * XResult * (1.0 - XResult) * PreviousLayer[i].XResult);
                //Console.WriteLine(PreviousWeight[i].weight + "after");

            }
        }
    }

    public class Weight
    {
        public Weight(double weight)
        {
            this.weight = weight;
        }

        public double weight { get; set; }
    }
}








