using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
    class NeuralNetwork
    {

        private NeuronLayer inputLayer;
        private NeuronLayer hiddenLayer;
        private NeuronLayer outputLayer;

        public NeuralNetwork(int numberInputs, int numberHiddenNodes, int numberOutputs, float learningRate, float momentumFactor)
        {
            inputLayer = new NeuronLayer(numberInputs, numberHiddenNodes, 0, learningRate, momentumFactor);
            hiddenLayer = new NeuronLayer(numberHiddenNodes, numberOutputs, numberInputs, learningRate, momentumFactor);
            outputLayer = new NeuronLayer(numberOutputs, 0, numberHiddenNodes, learningRate, momentumFactor);

            inputLayer.Initialise(null, hiddenLayer);
            hiddenLayer.Initialise(inputLayer, outputLayer);
            outputLayer.Initialise(hiddenLayer, null);
        }

        public void SetInput(int position, float value)
        {
            inputLayer.SetNeuronValue(position, value);
        }

        public void SetTarget(int position, float value)
        {
            outputLayer.SetTargetValue(position, value);
        }

        public void FeedForward()
        {
            inputLayer.CalculateNeuronValues();
            hiddenLayer.CalculateNeuronValues();
            outputLayer.CalculateNeuronValues();
        }

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

        public void BackPropagate()
        {
            outputLayer.CalculateErrors();
            hiddenLayer.CalculateErrors();

            hiddenLayer.AdjustWeights();
            inputLayer.AdjustWeights();
        }

        public float GetOutput(int position)
        {
            return outputLayer.GetNeuronValue(position);
        }
    }
}
