using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
    public class TestMLP
    {
        private MLP nn;
        private float[,] trainingSet;
        private float[,] testingSet;

        public TestMLP()
        {
            trainingSet = new float[1, 37] { { 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F } };
            testingSet = new float[1, 37] { { 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F } };
            nn = new MLP(1, 30, 7, 15, 0.2F, 0.9F, trainingSet);

            nn.TestNetwork(1, testingSet);
        }
    }
}
