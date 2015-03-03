using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
    /// <summary>
    /// Bring the layers together
    /// </summary>
    class NeuralNetwork
    {

        /// <summary>
        /// The input layer of the MLP
        /// </summary>
        private NeuronLayer inputLayer;

        /// <summary>
        /// Array of all layers of the MLP
        /// </summary>
        private NeuronLayer[] allLayers;

        /// <summary>
        /// The output layer of the MLP
        /// </summary>
        private NeuronLayer outputLayer;

        /// <summary>
        /// The number of hidden layers in the MLP
        /// </summary>
        private int numberHiddenLayers;

        /// <summary>
        /// Constructor for the neural network class
        /// </summary>
        /// <param name="numberInputs">
        /// The number of nodes in the input layer
        /// </param>
        /// <param name="numberHiddenNodes">
        /// An array of the number of hidden nodes in each hidden layer
        /// </param>
        /// <param name="numberOutputs">
        /// The number of nodes in the output layer
        /// </param>
        /// <param name="learningRate">
        /// The learning rate of the network
        /// </param>
        /// <param name="momentumFactor">
        /// The momentum factor of the network
        /// </param>
        public NeuralNetwork(int numberInputs, int[] numberHiddenNodes, int numberOutputs, float learningRate, float momentumFactor)
        {
            this.numberHiddenLayers = numberHiddenNodes.Length;
            allLayers = new NeuronLayer[numberHiddenLayers + 2];
            for (int i = 0; i < numberHiddenLayers + 2; i++)
            {
                if (i == 0)
                    inputLayer = allLayers[i] = new NeuronLayer(numberInputs, numberHiddenNodes[i], 0, learningRate, momentumFactor);
                else if (i < numberHiddenLayers)
                    allLayers[i] = new NeuronLayer(numberHiddenNodes[i - 1], numberHiddenNodes[i], allLayers[i - 1].GetNumberNodes(), learningRate, momentumFactor);
                else if (i < numberHiddenLayers + 1)
                    allLayers[i] = new NeuronLayer(numberHiddenNodes[i - 1], numberOutputs, allLayers[i - 1].GetNumberNodes(), learningRate, momentumFactor);
                else
                    outputLayer = allLayers[i] = new NeuronLayer(numberOutputs, 0, allLayers[i - 1].GetNumberNodes(), learningRate, momentumFactor);
            }

            inputLayer.Initialise(null, allLayers[1]);
            for (int i = 1; i < numberHiddenLayers + 1; i++)
                allLayers[i].Initialise(allLayers[i - 1], allLayers[i + 1]);
            outputLayer.Initialise(allLayers[numberHiddenLayers], null);
        }

        /// <summary>
        /// Set the input layer neuron at the given position
        /// </summary>
        /// <param name="position">
        /// The position of the neuron value to set
        /// </param>
        /// <param name="value">
        /// The new value of the neuron
        /// </param>
        public void SetInput(int position, float value)
        {
            inputLayer.SetNeuronValue(position, value);
        }

        /// <summary>
        /// Set the output layer target at the given position
        /// </summary>
        /// <param name="position">
        /// The position of the target value to set
        /// </param>
        /// <param name="value">
        /// The new value of the target
        /// </param>
        public void SetTarget(int position, float value)
        {
            outputLayer.SetTargetValue(position, value);
        }

        /// <summary>
        /// Feed the values forward through the network
        /// </summary>
        public void FeedForward()
        {
            for (int i = 0; i < numberHiddenLayers + 2; i++)
                allLayers[i].CalculateNeuronValues();
        }

        /// <summary>
        /// Return the aggregate error from the output layer
        /// </summary>
        /// <returns>
        /// Aggregate error
        /// </returns>
        public float CalculateError()
        {
            float error = 0F;

            for (int i = 0; i < outputLayer.GetNumberNodes(); i++)
            {
                error += (float)Math.Pow(outputLayer.GetNeuronValue(i) - outputLayer.GetTargetValue(i), 2);
            }

            error /= outputLayer.GetNumberNodes();

            return error;
        }

        /// <summary>
        /// Back propagate the errors through the network
        /// </summary>
        public void BackPropagate()
        {
            for (int i = numberHiddenLayers + 1; i > 0; i--)
                allLayers[i].CalculateErrors();

            for (int i = numberHiddenLayers; i >= 0; i--)
                allLayers[i].AdjustWeights();
        }

        /// <summary>
        /// Return the output of the network from the output layer at the position
        /// </summary>
        /// <param name="position">
        /// The position of the output to return
        /// </param>
        /// <returns>
        /// The output of the network
        /// </returns>
        public float GetOutput(int position)
        {
            return outputLayer.GetNeuronValue(position);
        }
    }
}
