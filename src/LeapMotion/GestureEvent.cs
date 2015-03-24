using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Constructor to create object
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

    }
}
