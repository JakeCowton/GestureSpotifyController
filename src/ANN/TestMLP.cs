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
    public class TestMLP
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
        public TestMLP()
        {
            trainingSet = new float[1, 37] { { 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F } };
            testingSet = new float[1, 37] { { 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F } };
            nn = new MLP(30, 7, 15, 0.2F, 0.9F);

            nn.TrainNetwork(1, trainingSet);
            nn.TestNetwork(1, testingSet);
        }
    }
}
