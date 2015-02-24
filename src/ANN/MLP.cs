using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
    class MLP
    {
        private int numInputs;
        private int numInputValues;
        private int numOutputs;
        private int numHiddenNodes;

        private float[,] trainingSet;
        private float[,] testingSet;

        private float learningRate;
        private float momentumFactor;

        private NeuralNetwork neuralNetwork;

        public MLP(int numInputs, int numInputValues, int numOutputs, int numHiddenNodes, float learningRate, float momentumFactor, float[,] trainingSet)
        {
            this.numInputs = numInputs;
            this.numInputValues = numInputValues;
            this.numOutputs = numOutputs;
            this.numHiddenNodes = numHiddenNodes;
            this.learningRate = learningRate;
            this.momentumFactor = momentumFactor;

            this.trainingSet = trainingSet;

            neuralNetwork = new NeuralNetwork(numInputValues, numHiddenNodes, numOutputs, learningRate, momentumFactor);
        }

        public void TrainNetwork()
        {
            float error = 1F;
            int count = 0;

            while ((error > 0.002) && (count < 50000))
            {
                error = 0;
                count += 1;
                int lastInput = 0;
                for (int i = 0; i < numInputs; i++)
                {
                    for (int j = 0; j < numInputValues; j++)
                    {
                        neuralNetwork.SetInput(j, trainingSet[i, j]);
                        lastInput = j;
                    }
                    for (int j = 0; j < numOutputs; j++)
                    {
                        neuralNetwork.SetTarget(j, trainingSet[i, lastInput + 1]);
                        lastInput += 1;
                    }
                    neuralNetwork.FeedForward();
                    error += neuralNetwork.CalculateError();
                    neuralNetwork.BackPropagate();
                }
                error /= numInputs;
            }
        }

        public void TestNetwork(int numInputs, float[,] testingSet)
        {
            this.numInputs = numInputs;
            this.testingSet = testingSet;
            for (int i = 0; i < numInputs; i++)
            {
                Console.WriteLine(i + 1 + ":");
                for (int j = 0; j < (numInputValues + numOutputs); j++)
                {
                    Console.Write(testingSet[i, j] + ";");
                }
                for (int j = 0; j < numInputValues; j++)
                {
                    neuralNetwork.SetInput(j, testingSet[i, j]);
                }
                neuralNetwork.FeedForward();

                //float max = -1000.0F;
                //int index = -1000;
                //for (int j = 0; j < numOutputs; j++)
                //{
                //    Console.WriteLine(neuralNetwork.GetOutput(j));
                //    if (max < neuralNetwork.GetOutput(j))
                //    {
                //        max = neuralNetwork.GetOutput(j);
                //        index = j;
                //    }
                //}
            }
        }

        public float[] RecallNetwork(float[] data)
        {
            for (int i = 0; i < numInputValues; i++)
            {
                neuralNetwork.SetInput(i, data[i]);
            }
            neuralNetwork.FeedForward();

            float[] outputs = new float[numOutputs];
            for (int i = 0; i < numOutputs; i++)
            {
                outputs[i] = neuralNetwork.GetOutput(i);
            }
            return outputs;
        }
    }
}
