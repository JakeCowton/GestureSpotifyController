using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace LeapMotion
{
    public class LeapInterface
    {
        /// <summary>
        /// Event handler using custom EventArgs object (GestureEvent)
        /// </summary>
        public event EventHandler<GestureEvent> GestureInterface;

        /// <summary>
        /// Custom EventArgs object to pass with event
        /// </summary>
        private GestureEvent GestureArgs;

        /// <summary>
        /// Instance of LeapListener class to register with controller
        /// </summary>
        private LeapListener listener;

        /// <summary>
        /// Controller to register with LeapListener object
        /// </summary>
        private Controller controller;

        /// <summary>
        /// Constructor for LeapInterface class
        /// </summary>
        public LeapInterface()
        {
            // Create a listener and controller
            this.listener = new LeapListener();
            this.controller = new Controller();

            // Have the sample listener receive events from the controller
            controller.AddListener(listener);
            controller.SetPolicyFlags(Controller.PolicyFlag.POLICY_BACKGROUND_FRAMES);

            // Register event handler
            listener.GestureMade += new EventHandler<GestureEvent>(ProcessGesture);
        }

        /// <summary>
        /// Ensure objects are disposed of cleanly on exit
        /// </summary>
        public void CleanUpLeapInterface()
        {
            controller.RemoveListener(listener);
            controller.Dispose();
        }

        /// <summary>
        /// Method to receive events from LeapListener and pass on to GestureInterface
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Custom EventArgs object (GestureEvent)</param>
        public void ProcessGesture(object sender, GestureEvent e)
        {
            GestureArgs = e;
            if (GestureInterface != null)
                GestureInterface(this, GestureArgs);
        }
    }
}
