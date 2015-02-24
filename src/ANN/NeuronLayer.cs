using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
    class NeuronLayer
    {
        private int numberNodes;
        private int numberChildNodes;
        private int numberParentNodes;

        private float learningRate;
        private float momentumFactor;

        private float[] neuronValues;
        private float[] targetValues;
        private float[] errors;

        private float[,] weights;
        private float[,] weightChanges;
        private float[] biasValues;
        private float[] biasWeights;

        private NeuronLayer parentLayer;
        private NeuronLayer childLayer;

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

        public int GetNumberNodes()
        {
            return numberNodes;
        }

        public float GetNeuronValue(int position)
        {
            return neuronValues[position];
        }

        public void SetNeuronValue(int position, float value)
        {
            neuronValues[position] = value;
        }

        public float GetTargetValue(int position)
        {
            return targetValues[position];
        }

        public void SetTargetValue(int position, float value)
        {
            targetValues[position] = value;
        }

        public void Initialise(NeuronLayer parentLayer, NeuronLayer childLayer)
        {
            this.parentLayer = parentLayer;
            this.childLayer = childLayer;
        }

        public float Sigmoid(float action)
        {
            return 1.0F / (1.0F + (float)Math.Exp(-action));
        }

        public void CalculateNeuronValues()
        {
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

        public void CalculateErrors()
        {
            if (childLayer == null)
            {
                for (int i = 0; i < numberNodes; i++)
                {
                    errors[i] = (targetValues[i] - neuronValues[i]) * neuronValues[i] * (1.0F - neuronValues[i]);
                }
            }
            else if (parentLayer == null)
            {
                for (int i = 0; i < numberNodes; i++)
                {
                    errors[i] = 0.0F;
                }
            }
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

        public void AdjustWeights()
        {
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
