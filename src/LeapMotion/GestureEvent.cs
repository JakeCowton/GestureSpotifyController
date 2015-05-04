using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace LeapMotion
{
    public class GestureEvent : EventArgs
    {
        /// <summary>
        /// Name of the gesture performed
        /// </summary>
        private string name;

        /// <summary>
        /// ID of the gesture
        /// </summary>
        private int id;

        /// <summary>
        /// State of the gesture in the current frame
        /// </summary>
        private string state;

        /// <summary>
        /// Position of the gesture (vector)
        /// </summary>
        private float[] position;

        /// <summary>
        /// Direction of the gesture (vector)
        /// </summary>
        private float[] direction;

        /// <summary>
        /// Number of rotations
        /// </summary>
        private float progress;

        /// <summary>
        /// To calculate direction
        /// </summary>
        private Vector normal;

        /// <summary>
        /// The circle pointable object
        /// </summary>
        private Pointable pointable;

        /// <summary>
        /// Constructor to create a swipe event object
        /// </summary>
        /// <param name="name">Name of the gesture</param>
        /// <param name="id">ID of the gesture</param>
        /// <param name="state">State of the gesture in the frame</param>
        /// <param name="position">Position of the gesture in the frame</param>
        /// <param name="direction">Direction of the gesture in the frame</param>
        public GestureEvent(string name, int id, string state, float[] position, float[] direction)
        {
            this.name = name;
            this.id = id;
            this.state = state;
            this.position = position;
            this.direction = direction;
            this.progress = 0F;
            this.normal = null;
        }

        /// <summary>
        /// Alternative constructor for circle gesture events
        /// <param name="normal">Contains the x, y and z vectors of the gesture</param>
        /// <param name="pointable">The object that is making the gesture</param>
        public GestureEvent(string name, int id, string state, float progress, Vector normal, Pointable pointable)
        {
            this.name = name;
            this.id = id;
            this.state = state;
            this.progress = progress;
            this.normal = normal;
            this.position = null;
            this.direction = null;
            this.pointable = pointable;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public string State
        {
            get
            {
                return state;
            }
        }

        public float[] Position
        {
            get
            {
                return position;
            }
        }

        public float[] Direction
        {
            get
            {
                return direction;
            }
        }

        public float Progress
        {
            get
            {
                return progress;
            }
        }

        /// <summary>
        /// Calculates if a circle gesture is clockwise or not
        /// </summary>
        public bool isClockwise()
        {
            if (this.pointable.Direction.AngleTo(this.normal) <= Math.PI / 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
