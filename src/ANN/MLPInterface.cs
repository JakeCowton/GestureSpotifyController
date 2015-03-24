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
            // 10 Inputs | 7 Outputs | 4 hidden ...
            nn = new MLP(10, 7, hiddenStructure, 0.2F, 0.9F);
            trainMLP();
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

        /// <summary>
        /// Calls the MLP
        /// </summary>
        /// <param name="inputs">
        ///     The skeleton points
        /// </param>
        /// <returns>
        ///     A string representing the gesture classification
        /// </returns>
        public String recall(float[] inputs)
        {
            float[] distances = measureInputs(inputs);

            return classifyGesture(distances);
        }

        /// <summary>
        /// Finds the Euclidean distance between two skeleton points using the x & y axis (not z)
        /// </summary>
        /// <param name="joints">
        ///     An array of floats containing the used joint data
        /// </param>
        /// <returns>
        ///     An array of distances
        ///         Order is shown in the comments
        /// </returns>
        public float[] measureInputs(float[] joints)
        {
            float[] measuredInputs = new float[10];

            // Head to left hand
            measuredInputs[0] = toFloat(Math.Sqrt(Math.Pow((joints[0] + joints[14]), 2) + Math.Pow((joints[1] + joints[15]), 2)));
            // Head to left wrist
            measuredInputs[1] = toFloat(Math.Sqrt(Math.Pow((joints[0] + joints[10]), 2) + Math.Pow((joints[1] + joints[11]), 2)));
            // Head to left elbow
            measuredInputs[2] = toFloat(Math.Sqrt(Math.Pow((joints[0] + joints[6]), 2) + Math.Pow((joints[1] + joints[7]), 2)));
            // Head to right elbow
            measuredInputs[3] = toFloat(Math.Sqrt(Math.Pow((joints[0] + joints[8]), 2) + Math.Pow((joints[1] + joints[9]), 2)));
            // Head to right wrist
            measuredInputs[4] = toFloat(Math.Sqrt(Math.Pow((joints[0] + joints[12]), 2) + Math.Pow((joints[1] + joints[13]), 2)));
            // Head to right hand
            measuredInputs[5] = toFloat(Math.Sqrt(Math.Pow((joints[0] + joints[16]), 2) + Math.Pow((joints[1] + joints[17]), 2)));
            // Hand left to shoulder left
            measuredInputs[6] = toFloat(Math.Sqrt(Math.Pow((joints[14] + joints[2]), 2) + Math.Pow((joints[15] + joints[3]), 2)));
            // wrist left to shoulder left
            measuredInputs[7] = toFloat(Math.Sqrt(Math.Pow((joints[10] + joints[2]), 2) + Math.Pow((joints[11] + joints[3]), 2)));
            // Hand right to shoulder right
            measuredInputs[8] = toFloat(Math.Sqrt(Math.Pow((joints[16] + joints[4]), 2) + Math.Pow((joints[17] + joints[5]), 2)));
            // Wrist right to shoulder right
            measuredInputs[9] = toFloat(Math.Sqrt(Math.Pow((joints[12] + joints[4]), 2) + Math.Pow((joints[13] + joints[5]), 2)));

            return measuredInputs;
        }

        /// <summary>
        ///     Handles the conversion of a double to float
        ///     whilst avoiding hitting +/- infinity.
        /// </summary>
        /// <param name="input">
        ///     The value to convert to float
        /// </param>
        /// <returns>
        ///     A float value
        /// </returns>
        private float toFloat(double input)
        {
            float result = (float)input;
            if (float.IsPositiveInfinity(result))
            {
                result = float.MaxValue;
            }
            else if (float.IsNegativeInfinity(result))
            {
                result = float.MinValue;
            }

            return result;

        }

        /// <summary>
        ///     Classifies 
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public string classifyGesture(float[] inputs)
        {
            String gesture = "";
            float[] outputs = this.nn.RecallNetwork(inputs);
            // Clasify outputs
            // Maybe if highest is over 0.75?
            float maxValue = outputs.Max();
            if (maxValue > 0.75F)
            {
                int maxIndex = outputs.ToList().IndexOf(maxValue);
                switch (maxIndex)
                {
                    case 0:
                        // Play
                        return "PLAY";
                    case 1:
                        // Pause
                        return "PAUSE";
                    case 2:
                        // Skip Forward
                        return "SKIP";
                    case 3:
                        // Go Backward
                        return "BACK";
                    case 4:
                        // Volume Up
                        return "VUP";
                    case 5:
                        // Volume Down
                        return "VDOWN";
                    case 6:
                        // Mute
                        return "MUTE";
                }
            }
            return gesture;
        }
    }
}
