using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{

    /// <summary>
    /// A single layer of the MLP
    /// </summary>
    class NeuronLayer
    {
        /// <summary>
        /// The number of nodes in this layer
        /// </summary>
        private int numberNodes;

        /// <summary>
        /// The number of nodes in the next layer
        /// </summary>
        private int numberChildNodes;

        /// <summary>
        /// The number of nodes in the previous layer
        /// </summary>
        private int numberParentNodes;

        /// <summary>
        /// The learning rate of the MLP
        /// </summary>
        private float learningRate;

        /// <summary>
        /// The momentum factor of the MLP
        /// </summary>
        private float momentumFactor;

        /// <summary>
        /// 1d array holding the individual neuron values
        /// </summary>
        private float[] neuronValues;

        /// <summary>
        /// 1d array holding the individual target values
        /// </summary>
        private float[] targetValues;

        /// <summary>
        /// 1d array holding the error for each value
        /// </summary>
        private float[] errors;

        /// <summary>
        /// 2d array holding the weights
        /// </summary>
        private float[,] weights;

        /// <summary>
        /// 2d array holding the changes in weight
        /// </summary>
        private float[,] weightChanges;

        /// <summary>
        /// 1d array holding the individual bias values
        /// </summary>
        private float[] biasValues;

        /// <summary>
        /// 1d array holding the weights for the bias
        /// </summary>
        private float[] biasWeights;

        /// <summary>
        /// The previous layer object
        /// </summary>
        private NeuronLayer parentLayer;

        /// <summary>
        /// The next layer object
        /// </summary>
        private NeuronLayer childLayer;

        /// <summary>
        /// Constructor for the layer
        /// </summary>
        /// <param name="numberNodes">
        /// The number of nodes in the layer
        /// </param>
        /// <param name="numberChildNodes">
        /// The number of nodes in the next layer
        /// </param>
        /// <param name="numberParentNodes">
        /// The number of nodes in the previous layer
        /// </param>
        /// <param name="learningRate">
        /// The learning rate of the MLP
        /// </param>
        /// <param name="momentumFactor">
        /// The momentum factor of the MLP
        /// </param>
        public NeuronLayer(int numberNodes, int numberChildNodes, int numberParentNodes, float learningRate, float momentumFactor)
        {
            this.numberNodes = numberNodes;
            this.numberChildNodes = numberChildNodes;
            this.numberParentNodes = numberParentNodes;
            this.learningRate = learningRate;
            this.momentumFactor = momentumFactor;

            neuronValues = new float[numberNodes];
            targetValues = new float[numberNodes];
            errors = new float[numberNodes];

            weights = new float[numberNodes, numberChildNodes];
            weightChanges = new float[numberNodes, numberChildNodes];
            biasValues = new float[numberChildNodes];
            biasWeights = new float[numberChildNodes];

            Random randomGenerator = new Random();

            for (int i = 0; i < numberNodes; i++)
            {
                for (int j = 0; j < numberChildNodes; j++)
                {
                    weights[i, j] = (float)randomGenerator.NextDouble();
                    weightChanges[i, j] = 0;
                }
            }
            for (int i = 0; i < numberChildNodes; i++)
            {
                biasValues[i] = -1;
                biasWeights[i] = (float)randomGenerator.NextDouble();
            }
        }

        /// <summary>
        /// Returns the number of nodes in the layer
        /// </summary>
        /// <returns>
        /// The number of nodes in the layer
        /// </returns>
        public int GetNumberNodes()
        {
            return numberNodes;
        }

        /// <summary>
        /// Return the neuron value at the given position
        /// </summary>
        /// <param name="position">
        /// Position to return neuron value from
        /// </param>
        /// <returns>
        /// The neuron value
        /// </returns>
        public float GetNeuronValue(int position)
        {
            return neuronValues[position];
        }

        /// <summary>
        /// Set the value for a neuron at the given position
        /// </summary>
        /// <param name="position">
        /// The position to set the neuron value
        /// </param>
        /// <param name="value">
        /// The new value for the neuron
        /// </param>
        public void SetNeuronValue(int position, float value)
        {
            neuronValues[position] = value;
        }

        /// <summary>
        /// Return the target value at the given position
        /// </summary>
        /// <param name="position">
        /// The position to return the target value from
        /// </param>
        /// <returns>
        /// The target value
        /// </returns>
        public float GetTargetValue(int position)
        {
            return targetValues[position];
        }

        /// <summary>
        /// Set the target value at the given position
        /// </summary>
        /// <param name="position">
        /// The position to set the target value
        /// </param>
        /// <param name="value">
        /// The new value for the target
        /// </param>
        public void SetTargetValue(int position, float value)
        {
            targetValues[position] = value;
        }

        /// <summary>
        /// Register the previous and next layers with the current layer
        /// </summary>
        /// <param name="parentLayer">
        /// The previous layer
        /// </param>
        /// <param name="childLayer">
        /// The next layer
        /// </param>
        public void Initialise(NeuronLayer parentLayer, NeuronLayer childLayer)
        {
            this.parentLayer = parentLayer;
            this.childLayer = childLayer;
        }

        /// <summary>
        /// Return the sigmoid of the given action
        /// </summary>
        /// <param name="action">
        /// The action value
        /// </param>
        /// <returns>
        /// The value after sigmoid function applied to action
        /// </returns>
        public float Sigmoid(float action)
        {
            return 1.0F / (1.0F + (float)Math.Exp(-action));
        }

        /// <summary>
        /// Calculate new values for all of the neurons in the layer
        /// </summary>
        public void CalculateNeuronValues()
        {
            // All layers besides input layer
            if (parentLayer != null)
            {
                for (int j = 0; j < numberNodes; j++)
                {
                    float action = 0;
                    for (int i = 0; i < numberParentNodes; i++)
                    {
                        action += parentLayer.neuronValues[i] * parentLayer.weights[i, j];
                    }
                    action += parentLayer.biasValues[j] * parentLayer.biasWeights[j];
                    neuronValues[j] = Sigmoid(action);
                }
            }
        }

        /// <summary>
        /// Calculate the errors for the layer
        /// </summary>
        public void CalculateErrors()
        {
            // Output layer
            if (childLayer == null)
            {
                for (int i = 0; i < numberNodes; i++)
                {
                    errors[i] = (targetValues[i] - neuronValues[i]) * neuronValues[i] * (1.0F - neuronValues[i]);
                }
            }
            // Input layer
            else if (parentLayer == null)
            {
                for (int i = 0; i < numberNodes; i++)
                {
                    errors[i] = 0.0F;
                }
            }
            // Hidden layers
            else
            {
                for (int i = 0; i < numberNodes; i++)
                {
                    float sum = 0F;
                    for (int j = 0; j < numberChildNodes; j++)
                    {
                        sum += childLayer.errors[j] * weights[i, j];
                    }
                    errors[i] = sum * neuronValues[i] * (1.0F - neuronValues[i]);
                }
            }
        }

        /// <summary>
        /// Adjust the weights for the layer
        /// </summary>
        public void AdjustWeights()
        {
            // Not output layer
            if (childLayer != null)
            {
                for (int i = 0; i < numberNodes; i++)
                {
                    for (int j = 0; j < numberChildNodes; j++)
                    {
                        float dw = learningRate * childLayer.errors[j] * neuronValues[i];
                        weights[i, j] += dw + momentumFactor * weightChanges[i, j];
                        weightChanges[i, j] = dw;
                    }
                }
                for (int j = 0; j < numberChildNodes; j++)
                {
                    biasWeights[j] += learningRate * childLayer.errors[j] * biasValues[j];
                }
            }
        }
    }
}
