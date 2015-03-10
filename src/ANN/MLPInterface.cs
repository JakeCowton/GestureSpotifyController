using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{

    /// <summary>
    /// Class to interface with the MLP for test/example purposes
    /// </summary>
    public class MLPInterface
    {
        /// <summary>
        /// THe MLP object
        /// </summary>
        private MLP nn;

        /// <summary>
        /// 2d array of training data
        /// </summary>
        private float[,] trainingSet;

        /// <summary>
        /// 2d array of testing data
        /// </summary>
        private float[,] testingSet;

        /// <summary>
        /// Constructor to test training and testing of the neural network with some example data hardcoded
        /// </summary>
        public MLPInterface()
        {
            int[] hiddenStructure = {4};
            nn = new MLP(4, 7, hiddenStructure, 0.2F, 0.9F);
        }

        /// <summary>
        /// Trains & tests the MLP
        /// </summary>
        private void trainMLP()
        {
            // Get training data
            // Get testing data
            // Get training length
            int numOfTraining = 0;
            // Get testing length
            int numOfTesting = 0;

            this.nn.TrainNetwork(numOfTraining, trainingSet);
            this.nn.TestNetwork(numOfTesting, testingSet);
        }

        private float recallMLP(float[] inputs)
        {
            return 1F;
        }
    }
}
