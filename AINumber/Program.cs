using AINumber.Neural;
using System;
using System.Collections.Generic;

namespace AINumber
{
    class Program
    {
        class DoubleComparer : IComparer<double>
        {
            public int Compare(double a, double b)
            {

                if (a > b)
                {
                    return 1;
                }
                else if (a < b)
                {
                    return -1;
                }

                return 0;
            }
        }

        class NeuralNetwork
        {
            //NeuralNetwork()
            //{

            //}

            Random Rnd { get; set; } = new Random();

            public List<Weight> NeuralWeightInput { get; set; } = new List<Weight>();

            List<Weight> NeuralWeightHidden { get; set; } = new List<Weight>();

            public List<SygmoidNeural> InputNeurals { get; set; } = new List<SygmoidNeural>();

            public List<SygmoidNeural> HiddenNeurals { get; set; } = new List<SygmoidNeural>();

            public List<SygmoidNeural> OutputNeurals { get; set; } = new List<SygmoidNeural>();

            public List<SygmoidNeural> AllNeurals { get; set; } = new List<SygmoidNeural>();


            int Epoch { get; set; } = 1000;


            public void Create()
            {
                GenerateWeight();
                GenerateNeurals();
                AddLayers();
            }

            public void Learn(List<SygmoidNeural> learn)
            {
                CSVFileGetData cSVFileGetData = new CSVFileGetData();

                for (int g = 0; g < Epoch; g++)
                {
                    // считать данные
                    cSVFileGetData.GetData(g);
                    // передать данные на вход
                    for (int i = 0; i < 784; i++)
                    {
                        if (cSVFileGetData.Data[i] == 0.0)
                        {

                        }
                        learn[i].X = cSVFileGetData.Data[i];
                    }

                    int count = 0;
                    foreach (ILearn n in learn)
                    {
                        //Console.WriteLine(count);
                        n.Function();
                        n.OutWeight();
                        count++;
                    }
                    // передать ошибку выходным нейронам
                    for (int i = 884; i < 894; i++)
                    {
                        learn[i].ErrorNumber = Math.Abs(learn[i].XResult - cSVFileGetData.TrueAnswer[i - 884]);
                        //Console.WriteLine(OutputNeurals[i].ErrorNumber+"Error output");
                        //Console.WriteLine();
                        //Console.WriteLine();
                        //Console.WriteLine();
                        //Console.WriteLine();
                        //Console.WriteLine(OutputNeurals[i].XResult+"--------------");
                        //Console.WriteLine(OutputNeurals[i].X + "Input-");
                    }
                

                    for (int i = learn.Count - 1; i >= 0; i--)
                    {
                        //Console.WriteLine(i + " learn elemnt");
                        learn[i].Learn();
                    }
                    Console.WriteLine(g + " epoch complete");
                    double SumLayerInput = 0;
                    double SumLayerHidden = 0;
                    double SumLayerOutput = 0;

                    foreach(SygmoidNeural n in InputNeurals)
                    {
                        SumLayerInput += n.ErrorNumberLayer;
                    }

                    foreach (SygmoidNeural n in HiddenNeurals)
                    {
                        SumLayerHidden += n.ErrorNumberLayer;
                    }

                    foreach (SygmoidNeural n in OutputNeurals)
                    {
                        SumLayerOutput += n.ErrorNumberLayer;
                    }

                    Console.WriteLine(SumLayerInput + " SumLayerInput");

                    Console.WriteLine(SumLayerHidden + " SumLayerHidden");

                    Console.WriteLine(SumLayerOutput + " SumLayerOutput");

                    SumLayerInput = 0;
                    SumLayerHidden = 0;
                    SumLayerOutput = 0;

                    //for (int i = 0; i < 10; i++)
                    //{
                    //    Console.WriteLine(AllNeurals[i].Weight[0].weight + "------------neural epoch");
                    //}
                    //Console.WriteLine();
                    //Console.WriteLine();
                    //Console.WriteLine();

                    //foreach (SygmoidNeural n in HiddenNeurals)
                    //{
                    //    if (n.Weight != null)
                    //    {
                    //        foreach (Weight nn in n.Weight)
                    //            Console.WriteLine(nn.weight + "-------------------");
                    //    }
                    //}
                }
            }

            public void ToDetermine()
            {
                // считать данные
                CSVFileGetData cSVFileGetData = new CSVFileGetData();
                List<double> outAnswer = new List<double>();
                for (int g = 500; g < 530; g++)
                {
                    // передать данные на входные нейроны
                    cSVFileGetData.GetData(g);

                    for (int i = 0; i < InputNeurals.Count; i++)
                    {
                        //if (i == 295)
                        //{
                        //    Console.WriteLine(cSVFileGetData.Data[i] + "-----------------");
                        //}
                        //Console.WriteLine(cSVFileGetData.Data[i] + "-----------------"+i);
                        InputNeurals[i].X = cSVFileGetData.Data[i];
                    }

                    //for(int i=0;i<784;i++)
                    //{
                    //    Console.WriteLine(AllNeurals[i].X + " AllNeurals input");
                    //    Console.WriteLine(InputNeurals[i].X + " InputNeurals input");
                    //}
                    // посчитать результат
                    foreach (SygmoidNeural n in AllNeurals)
                    {
                        n.Function();
                        n.OutWeight();
                    }



                    foreach (SygmoidNeural n in OutputNeurals)
                    {
                        Console.WriteLine(n.XResult);
                        //Console.WriteLine(n.X + "-----------------------");
                        outAnswer.Add(n.XResult);
                    }

                    //for (int i = 884; i < 894; i++)
                    //{
                    //    Console.WriteLine(AllNeurals[i].XResult + " AllNeurals input");
                    //    //Console.WriteLine(OutputNeurals[i - 884].XResult + " OutputNeurals input");
                    //}


                    // считать результат выходных нейронов
                    for (int i = 0; i < outAnswer.Count; i++)
                    {
                        if (CompareBoolean(outAnswer[i], outAnswer))
                        {
                            //foreach (double nn in outAnswer)
                            //{
                            //    Console.WriteLine(nn);
                            //}

                            Console.WriteLine("Answer:" + i + ". Real answer:" + cSVFileGetData.TrueAnswerNumber);
                            break;
                        }
                    }

                    outAnswer.Clear();




                    // другой метод подсчёта 

                    // передать данные на входные нейроны
                    for (int i = 0; i < InputNeurals.Count; i++)
                    {
                        //if (i == 295)
                        //{
                        //    Console.WriteLine(cSVFileGetData.Data[i] + "-----------------");
                        //}
                        //Console.WriteLine(cSVFileGetData.Data[i] + "-----------------"+i);
                        InputNeurals[i].X = cSVFileGetData.Data[i];
                    }

                    // пропустить через сигмоид
                    for (int i = 0; i < InputNeurals.Count; i++)
                    {
                        InputNeurals[i].XResult = SygmoidFunction(InputNeurals[i].X);
                        InputNeurals[i].X = 0;
                    }

                    double X;
                    // посчитать результат
                    for (int i = 0; i < HiddenNeurals.Count; i++)
                    {
                        X = 0;
                        for (int ii = 0; ii < HiddenNeurals[i].PreviousLayer.Count; ii++)
                        {
                            X += HiddenNeurals[i].PreviousLayer[ii].XResult * HiddenNeurals[i].PreviousWeight[ii].weight;
                        }
                        HiddenNeurals[i].X = X;
                    }

                    // пропустить через сигмоид
                    for (int i = 0; i < HiddenNeurals.Count; i++)
                    {
                        HiddenNeurals[i].XResult = SygmoidFunction(HiddenNeurals[i].X);
                        HiddenNeurals[i].X = 0;
                    }



                    for (int i = 0; i < OutputNeurals.Count; i++)
                    {
                        X = 0;
                        for (int ii = 0; ii < OutputNeurals[i].PreviousLayer.Count; ii++)
                        {
                            X += OutputNeurals[i].PreviousLayer[ii].XResult * OutputNeurals[i].PreviousWeight[ii].weight;
                        }
                        OutputNeurals[i].X = X;
                    }

                    // пропустить через сигмоид
                    for (int i = 0; i < OutputNeurals.Count; i++)
                    {
                        OutputNeurals[i].XResult = SygmoidFunction(OutputNeurals[i].X);
                        OutputNeurals[i].X = 0;
                    }

                    foreach (SygmoidNeural n in OutputNeurals)
                    {
                        Console.WriteLine(n.XResult);
                        //Console.WriteLine(n.X + "-----------------------");
                        outAnswer.Add(n.XResult);
                    }

                    // считать результат выходных нейронов
                    for (int i = 0; i < outAnswer.Count; i++)
                    {
                        if (CompareBoolean(outAnswer[i], outAnswer))
                        {
                            //foreach (double nn in outAnswer)
                            //{
                            //    Console.WriteLine(nn);
                            //}

                            Console.WriteLine("Answer:" + i + ". Real answer:" + cSVFileGetData.TrueAnswerNumber);
                            break;
                        }
                    }

                    outAnswer.Clear();

                    Console.WriteLine();
                    Console.WriteLine(g+" Iteration complete");
                    Console.WriteLine();
                }

            }


            double SygmoidFunction(double X)
            {
               return (1 / (1 + Math.Exp(-1 * X)));
            }

            bool CompareBoolean(double number, List<double> numbers)
            {
                foreach (double n in numbers)
                {
                    if (n > number)
                        return false;
                }
                return true;
            }

            void GenerateWeight()
            {
                for (int i = 0; i < 784; i++)
                {
                    for (int n = 0; n < 100; n++)
                    {
                        NeuralWeightInput.Add(new Weight(RandomNumber(1 / Math.Sqrt(784.0))));
                    }
                }

                Console.WriteLine(NeuralWeightInput[0].weight + "gneral");

                for (int i = 0; i < 100; i++)
                {
                    for (int n = 0; n < 10; n++)
                    {
                        NeuralWeightHidden.Add(new Weight(RandomNumber(1 / Math.Sqrt(100.0))));
                    }
                }
            }

            double RandomNumber(double max)
            {
                double number;
                for (; ; )
                {
                    number = Rnd.NextDouble();
                    if (number != 0 && number < max)
                    {
                        //Console.WriteLine(max+"------------------");
                        return number;
                    }
                }
            }

            void GenerateNeurals()
            {
                GenerateInputNeurals();
                GenerateHiddenNeurals();
                GenerateOutputNeurals();

                Console.WriteLine(AllNeurals.Count + "AllNeurals.Count");
            }

            void GenerateInputNeurals()
            {

                for (int i = 0; i < 784; i++)
                {
                    Weight[] NeuralWeightInputArr = new Weight[100];
                    NeuralWeightInput.CopyTo(100 * i, NeuralWeightInputArr, 0, 100);

                    //Console.WriteLine(NeuralWeightInputArr[0].weight + "---input1");
                    //Console.WriteLine(NeuralWeightInput[i*100].weight + "---input11");
                    InputNeurals.Add(new SygmoidNeural(NeuralWeightInputArr, null));
                }
                //Console.WriteLine(InputNeurals.Count+ " InputNeurals.Count");
            }

            void GenerateHiddenNeurals()
            {
                for (int i = 0; i < 100; i++)
                {
                    Weight[] NeuralWeightHiddenArr = new Weight[10];
                    Weight[] NeuralWeightInputArr;
                    NeuralWeightHidden.CopyTo(10 * i, NeuralWeightHiddenArr, 0, 10);
                    if (i == 1)
                    {

                    }
                    NeuralWeightInputArr = InputWeightAsPreviousArr(100, NeuralWeightInput, i);
                    //Console.WriteLine(i);

                    //Console.WriteLine(NeuralWeightInputArr[0].weight+"---input");
                    HiddenNeurals.Add(new SygmoidNeural(NeuralWeightHiddenArr, NeuralWeightInputArr));
                }
            }

            void GenerateOutputNeurals()
            {
                for (int i = 0; i < 10; i++)
                {
                    if (i == 9)
                    {

                    }
                    Weight[] NeuralWeightHiddenArr;
                    NeuralWeightHiddenArr = InputWeightAsPreviousArr(10, NeuralWeightHidden, i);
                    OutputNeurals.Add(new SygmoidNeural(null, NeuralWeightHiddenArr));
                }
            }

            Weight[] InputWeightAsPreviousArr(int count, List<Weight> list, int register)
            {
                Weight[] weights = new Weight[list.Count / count];
                int coun = 0;

                for (int i = register; i < list.Count; i += count)
                {
                    weights[coun] = list[i];
                    if (list[i] == null)
                    {

                    }

                    coun++;
                }
                return weights;
            }

            void AddLayers()
            {
                foreach (SygmoidNeural n in InputNeurals)
                {
                    n.NextLayer = HiddenNeurals;
                }

                foreach (SygmoidNeural n in HiddenNeurals)
                {
                    n.PreviousLayer = InputNeurals;
                    n.NextLayer = OutputNeurals;
                }

                foreach (SygmoidNeural n in OutputNeurals)
                {
                    n.PreviousLayer = HiddenNeurals;
                }

                AllNeurals.AddRange(InputNeurals);
                AllNeurals.AddRange(HiddenNeurals);
                AllNeurals.AddRange(OutputNeurals);
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            NeuralNetwork neuralNetwork = new NeuralNetwork();
            neuralNetwork.Create();
            //int count = 0;
            //int countWeigth = 0;
            //foreach (SygmoidNeural n in neuralNetwork.InputNeurals)
            //{
            //    //Console.WriteLine(count + " neural------------------------------------------------------------------------------------");
            //    count++;
            //    for (int i = 0; i < n.Weight.Length; i++)
            //    {
            //        countWeigth++;
            //        //Console.WriteLine(i + " Weight");
            //        //Console.WriteLine(n.Weight[i].weight);
            //    }
            //}
            //Console.WriteLine("----------------Входные нейроны-----------------");
            //Console.WriteLine(countWeigth + " всего весов");
            //Console.WriteLine(count + " всего нейронов");


            //int countHide = 0;
            //int countWeigthHide = 0;
            //foreach (SygmoidNeural n in neuralNetwork.HiddenNeurals)
            //{
            //    //Console.WriteLine(count + " neural------------------------------------------------------------------------------------");
            //    countHide++;
            //    for (int i = 0; i < n.Weight.Length; i++)
            //    {
            //        countWeigthHide++;
            //        //Console.WriteLine(i + " Weight");
            //        //Console.WriteLine(n.Weight[i].weight);
            //    }
            //}
            //Console.WriteLine("----------------Скрытые нейроны-----------------");
            //Console.WriteLine(countWeigthHide + " всего весов");
            //Console.WriteLine(countHide + " всего нейронов");

            //int countOut = 0;
            //int countWeigthOut = 0;
            //foreach (SygmoidNeural n in neuralNetwork.OutputNeurals)
            //{
            //    //Console.WriteLine(count + " neural------------------------------------------------------------------------------------");
            //    countOut++;
            //    for (int i = 0; i < n.PreviousWeight.Length; i++)
            //    {
            //        countWeigthOut++;
            //        //Console.WriteLine(i + " Weight");
            //        //Console.WriteLine(n.Weight[i].weight);
            //    }
            //}
            //Console.WriteLine("----------------Выходные нейроны-----------------");
            //Console.WriteLine(countWeigthOut + " всего весов");
            //Console.WriteLine(countOut + " всего нейронов");

            //CSVFileGetData cSVFile = new CSVFileGetData();
            //cSVFile.GetData(2);
            //for(int i=784;i<894;i++)
            //{
            //    Console.WriteLine(neuralNetwork.AllNeurals[i].NextLayer.Count+ " n.PreviousLayer.Count");
            //}
            Console.WriteLine(neuralNetwork.InputNeurals[0].Weight[1].weight + "------------------");
            Console.WriteLine(neuralNetwork.AllNeurals[0].Weight[1].weight + "------------------");
            Console.WriteLine(neuralNetwork.AllNeurals[785].PreviousWeight[0].weight + "------------------");
            Console.WriteLine(neuralNetwork.HiddenNeurals[1].PreviousWeight[0].weight + "------------------");
            Console.WriteLine(neuralNetwork.NeuralWeightInput[1].weight);
            neuralNetwork.Learn(neuralNetwork.AllNeurals);

            Console.WriteLine(neuralNetwork.InputNeurals[0].Weight[1].weight + "------------------");
            Console.WriteLine(neuralNetwork.AllNeurals[0].Weight[1].weight + "------------------");
            Console.WriteLine(neuralNetwork.AllNeurals[785].PreviousWeight[0].weight + "------------------");
            Console.WriteLine(neuralNetwork.HiddenNeurals[1].PreviousWeight[0].weight + "------------------");

            neuralNetwork.ToDetermine();
        }
    }
}
