using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
    /// <summary>
    /// Implements the main Multi-Layer Perceptron
    /// </summary>
    class MLP
    {
        /// <summary>
        /// The number of input nodes
        /// </summary>
        private int numInputValues;

        /// <summary>
        /// The number of output nodes
        /// </summary>
        private int numOutputs;

        /// <summary>
        /// The number of hidden nodes
        /// </summary>
        private int[] numHiddenNodes;

        /// <summary>
        /// The learning rate of the MLP
        /// </summary>
        private float learningRate;

        /// <summary>
        /// The momentum factor of the MLP
        /// </summary>
        private float momentumFactor;

        /// <summary>
        /// Instance of the neural network
        /// </summary>
        private NeuralNetwork neuralNetwork;

        /// <summary>
        /// Constructor to create neural network using passed parameters
        /// </summary>
        /// <param name="numInputValues">
        /// Number of input nodes
        /// </param>
        /// <param name="numOutputs">
        /// Number of output nodes
        /// </param>
        /// <param name="numHiddenNodes">
        /// Array of the number of hidden nodes in each hidden layer
        /// </param>
        /// <param name="learningRate">
        /// Learning rate of network
        /// </param>
        /// <param name="momentumFactor">
        /// Momentum factor of network
        /// </param>
        public MLP(int numInputValues, int numOutputs, int[] numHiddenNodes, float learningRate, float momentumFactor)
        {
            this.numInputValues = numInputValues;
            this.numOutputs = numOutputs;
            this.numHiddenNodes = numHiddenNodes;
            this.learningRate = learningRate;
            this.momentumFactor = momentumFactor;

            neuralNetwork = new NeuralNetwork(numInputValues, numHiddenNodes, numOutputs, learningRate, momentumFactor);
        }

        /// <summary>
        /// Train the neural network, providing the number of training data runs and a 2d array of data
        /// </summary>
        /// <param name="numInputs">
        /// Number of training data runs
        /// </param>
        /// <param name="trainingSet">
        /// 2d array of training data
        /// </param>
        public void TrainNetwork(int numInputs, float[,] trainingSet)
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

        /// <summary>
        /// Test the neural network providing the number of test runs and a 2d array of test data
        /// </summary>
        /// <param name="numInputs">
        /// Number of test runs
        /// </param>
        /// <param name="testingSet">
        /// 2d array of test data
        /// </param>
        public void TestNetwork(int numInputs, float[,] testingSet)
        {
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
            }
        }

        /// <summary>
        /// Recall the network
        /// </summary>
        /// <param name="data">
        /// Data to run through the network
        /// </param>
        /// <returns>
        /// 1d array of output values
        /// </returns>
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
