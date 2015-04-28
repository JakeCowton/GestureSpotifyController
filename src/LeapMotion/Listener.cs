using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace LeapMotion
{

    public class LeapListener : Listener
    {
        /// <summary>
        /// Event handler using custom EventArgs object (GestureEvent)
        /// </summary>
        public event EventHandler<GestureEvent> GestureMade;

        /// <summary>
        /// Custom EventArgs object to pass with event
        /// </summary>
        private GestureEvent GestureArgs;

        /// <summary>
        /// Method to trigger event
        /// </summary>
        /// <param name="GestureName">The name of the gesture performed</param>
        /// <param name="GestureId">The ID of the gesture</param>
        /// <param name="GestureState">The state of the gesture in the frame</param>
        /// <param name="GesturePosition">The position of the gesture in the frame (vector)</param>
        /// <param name="GestureDirection">The direction of the gesture in the frame (vector)</param>
        void OnGesture(string GestureName, int GestureId, string GestureState, float[] GesturePosition, float[] GestureDirection)
        {
            GestureArgs = new GestureEvent(GestureName, GestureId, GestureState, GesturePosition, GestureDirection);
            if (GestureMade != null)
                GestureMade(this, GestureArgs);
        }

        void OnCircleGesture(string GestureName, int GestureId, string GestureState, float GestureProgress, Vector GestureNormal, Pointable GesturePointable)
        {
            GestureArgs = new GestureEvent(GestureName, GestureId, GestureState, GestureProgress, GestureNormal, GesturePointable);
            if (GestureMade != null)
            {
                GestureMade(this, GestureArgs);
            }
        }

        public override void OnInit(Controller controller)
        {
            Console.WriteLine("Initialized leap listener");
        }

        public override void OnConnect(Controller controller)
        {
            Console.WriteLine("Connected to leap device");
            controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
            controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
        }

        public override void OnDisconnect(Controller controller)
        {
            //Note: not dispatched when running in a debugger.
            Console.WriteLine("Disconnected from leap device");
        }

        public override void OnExit(Controller controller)
        {
            Console.WriteLine("Exited leap listener");
        }

        public override void OnFrame(Controller controller)
        {
            // Get the most recent frame and report some basic information
            Frame frame = controller.Frame();

            // Get gestures
            // Only handles swipe gesture for now...
            GestureList gestures = frame.Gestures();
            for (int i = 0; i < gestures.Count; i++)
            {
                Gesture gesture = gestures[i];

                switch (gesture.Type)
                {
                    case Gesture.GestureType.TYPE_SWIPE:
                        SwipeGesture swipe = new SwipeGesture(gesture);
                        OnGesture("swipe", swipe.Id, swipe.State.ToString(), swipe.Position.ToFloatArray(), swipe.Direction.ToFloatArray());
                        break;
                    case Gesture.GestureType.TYPECIRCLE:
                        CircleGesture circle = new CircleGesture(gesture);
                        OnCircleGesture("circle", circle.Id, circle.State.ToString(), circle.Progress, circle.Normal, circle.Pointable);
                        break;
                }
            }
        }
    }
}
