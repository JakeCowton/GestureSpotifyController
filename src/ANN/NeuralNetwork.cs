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
        /// The hidden layer of the MLP
        /// </summary>
        private NeuronLayer hiddenLayer;

        // TODO: Modify this class to allow multiple hidden layers (SHOULD be minor changes to this class only)

        /// <summary>
        /// The output layer of the MLP
        /// </summary>
        private NeuronLayer outputLayer;

        /// <summary>
        /// Constructor for the neural network class
        /// </summary>
        /// <param name="numberInputs">
        /// The number of nodes in the input layer
        /// </param>
        /// <param name="numberHiddenNodes">
        /// The number of nodes in the hidden layer
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
        public NeuralNetwork(int numberInputs, int numberHiddenNodes, int numberOutputs, float learningRate, float momentumFactor)
        {
            inputLayer = new NeuronLayer(numberInputs, numberHiddenNodes, 0, learningRate, momentumFactor);
            hiddenLayer = new NeuronLayer(numberHiddenNodes, numberOutputs, numberInputs, learningRate, momentumFactor);
            outputLayer = new NeuronLayer(numberOutputs, 0, numberHiddenNodes, learningRate, momentumFactor);

            inputLayer.Initialise(null, hiddenLayer);
            hiddenLayer.Initialise(inputLayer, outputLayer);
            outputLayer.Initialise(hiddenLayer, null);
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
            inputLayer.CalculateNeuronValues();
            hiddenLayer.CalculateNeuronValues();
            outputLayer.CalculateNeuronValues();
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
            outputLayer.CalculateErrors();
            hiddenLayer.CalculateErrors();

            hiddenLayer.AdjustWeights();
            inputLayer.AdjustWeights();
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
