//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace SpotifyKinectInterface
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.ComponentModel;
    using System.Threading;
    using Microsoft.Kinect;
    using SpotifyKinectInterface.VoiceControl;
    using ANN;
    using MediaController;


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Debug counter
        /// </summary>
        private int tickCounter = 0;

        /// <summary>
        /// Width of output drawing
        /// </summary>
        private const float RenderWidth = 640.0f;

        /// <summary>
        /// Height of our output drawing
        /// </summary>
        private const float RenderHeight = 480.0f;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 3;

        /// <summary>
        /// Thickness of body center ellipse
        /// </summary>
        private const double BodyCenterThickness = 10;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Brush used to draw skeleton center point
        /// </summary>
        private readonly Brush centerPointBrush = Brushes.Blue;

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        private readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently tracked
        /// </summary>
        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>        
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor sensor;

        /// <summary>
        /// The voice control engine
        /// </summary>
        private VoiceControlEngine vc;

        /// <summary>
        /// Drawing group for skeleton rendering output
        /// </summary>
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        private DrawingImage imageSource;

        /// <summary>
        /// The MLP class for classification
        /// </summary>
        private MLPInterface mlpInterface;

        /// <summary>
        /// The leapinterface class for gesture handling
        /// </summary>
        private LeapMotion.LeapInterface leapinterface;

        /// <summary>
        /// Boolean operator to represent play/pause
        /// </summary>
        private Boolean Playing;

        /// <summary>
        /// The object which controls spotify
        /// </summary>
        private SpotifyController spotifyController;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.spotifyController = new SpotifyController();
        }

        /// <summary>
        /// Draws indicators to show which edges are clipping skeleton data
        /// </summary>
        /// <param name="skeleton">skeleton to draw clipping information for</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private static void RenderClippedEdges(Skeleton skeleton, DrawingContext drawingContext)
        {
            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, RenderHeight - ClipBoundsThickness, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, RenderHeight));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(RenderWidth - ClipBoundsThickness, 0, ClipBoundsThickness, RenderHeight));
            }
        }

        /// <summary>
        /// Execute startup tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Test MLP
           // this.mlpInterface = new MLPInterface();

            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup);

            // Display the drawing using our image control
            Image.Source = this.imageSource;

            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                // Turn on the skeleton stream to receive skeleton frames
                this.sensor.SkeletonStream.Enable();

                // Add an event handler to be called whenever there is new color frame data
                this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;

                // Start the sensor!
                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }

            if (null == this.sensor)
            {
                this.statusBarText.Text = Properties.Resources.NoKinectReady;
            }

            vc = new VoiceControlEngine(sensor);
            vc.voiceRec();

            // Create and add event handler for leap gestures from LeapInterface
            this.leapinterface = new LeapMotion.LeapInterface();
            leapinterface.GestureInterface += new EventHandler<LeapMotion.GestureEvent>(ProcessGesture);
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.AudioSource.Stop();

                this.sensor.Stop();
                this.sensor = null;
            }

            this.vc.kill();

            // Clean up the leap interface, controller and listener
            this.leapinterface.CleanUpLeapInterface();

        }

        /// <summary>
        /// Event handler for Leap sensor listener class OnGesture event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void ProcessGesture(object sender, LeapMotion.GestureEvent e)
        {
            // Example setting global variable to play/pause and printing actions (Using object state - "STATESTART" signifies a new gesture)
            if (e.State == "STATESTART")
            {
                if (Playing)
                {
                    Console.WriteLine("Swipe - Paused music");
                    Playing = false;
                }
                else
                {
                    Console.WriteLine("Swipe - Play music");
                    Playing = true;
                }
            }
        }

        /// <summary>
        /// Event handler for Kinect sensor's SkeletonFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }
            try
            {
                using (DrawingContext dc = this.drawingGroup.Open())
                {
                    // Draw a transparent background to set the render size
                    dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));

                    if (skeletons.Length != 0)
                    {
                        foreach (Skeleton skel in skeletons)
                        {
                            RenderClippedEdges(skel, dc);

                            if (skel.TrackingState == SkeletonTrackingState.Tracked)
                            {
                                this.DrawBonesAndJoints(skel, dc);
                            }
                            else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                            {
                                dc.DrawEllipse(
                                this.centerPointBrush,
                                null,
                                this.SkeletonPointToScreen(skel.Position),
                                BodyCenterThickness,
                                BodyCenterThickness);
                            }
                        }
                    }
                }
                
                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Caught overlapped drawing frames...");
            }
        }

        /// <summary>
        /// Dumps skeleton joint co-ordinates to debug console
        /// </summary>
        /// <param name="skeleton">skeleton to dump joint data for</param>
        private float[] dumpJointData(Skeleton skeleton)
        {
            float[] joints = new float[24] {
                skeleton.Joints[JointType.Head].Position.X, // 0
                skeleton.Joints[JointType.Head].Position.Y, // 1
                skeleton.Joints[JointType.ShoulderLeft].Position.X, //2
                skeleton.Joints[JointType.ShoulderLeft].Position.Y, //3
                skeleton.Joints[JointType.ShoulderRight].Position.X, //4
                skeleton.Joints[JointType.ShoulderRight].Position.Y, //5
                skeleton.Joints[JointType.ElbowLeft].Position.X, //6
                skeleton.Joints[JointType.ElbowLeft].Position.Y, //7
                skeleton.Joints[JointType.ElbowRight].Position.X, //8
                skeleton.Joints[JointType.ElbowRight].Position.Y, //9
                skeleton.Joints[JointType.WristLeft].Position.X, //10
                skeleton.Joints[JointType.WristLeft].Position.Y, //11
                skeleton.Joints[JointType.WristRight].Position.X, //12
                skeleton.Joints[JointType.WristRight].Position.Y, //13
                skeleton.Joints[JointType.HandLeft].Position.X, //14
                skeleton.Joints[JointType.HandLeft].Position.Y, //15
                skeleton.Joints[JointType.HandRight].Position.X, //16
                skeleton.Joints[JointType.HandRight].Position.Y, //17
                skeleton.Joints[JointType.Spine].Position.X, //18
                skeleton.Joints[JointType.Spine].Position.Y, //19
                skeleton.Joints[JointType.KneeLeft].Position.X, //20
                skeleton.Joints[JointType.KneeLeft].Position.Y, //21
                skeleton.Joints[JointType.KneeRight].Position.X, //22
                skeleton.Joints[JointType.KneeRight].Position.Y //23
            };

            return joints;
        }

        /// <summary>
        /// Draws a skeleton's bones and joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
        {
            // Render Torso
            this.DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
            this.DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

            // Left Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

            // Right Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

            // Left Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

            // Right Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);

            // Render Joints
            foreach (Joint joint in skeleton.Joints)
            {
                Brush drawBrush = null;

                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    drawBrush = this.trackedJointBrush;
                }
                else if (joint.TrackingState == JointTrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;
                }

                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                }
            }
            if (tickCounter % 50 == 0)
            {
                Thread myThread = new System.Threading.Thread(delegate()
                {
                    string gesture = this.mlpInterface.recall(dumpJointData(skeleton));
                    if (!gesture.Equals(""))
                    {
                        runCommand(gesture);
                    }
                });
                myThread.Start();
            }
            tickCounter++;
        }

        /// <summary>
        /// Calls the command from the interface project
        /// </summary>
        /// <param name="command">
        ///     The name of the command to run
        /// </param>
        private void runCommand(String command)
        {
            switch (command)
            {
                case "PLAY":
                    Console.WriteLine("Play");
                    this.spotifyController.play_pause();
                    break;
                case "PAUSE":
                    Console.WriteLine("Pause");
                    this.spotifyController.play_pause();
                    break;
                case "SKIP":
                    Console.WriteLine("Skip");
                    this.spotifyController.next();
                    break;
                case "BACK":
                    Console.WriteLine("Back");
                    this.spotifyController.previous();
                    break;
                case "VUP":
                    Console.WriteLine("Volume Up");
                    this.spotifyController.volumeUp();
                    break;
                case "VDOWN":
                    Console.WriteLine("Volume Down");
                    this.spotifyController.volumeDown();
                    break;
                case "MUTE":
                    Console.WriteLine("Mute");
                    this.spotifyController.mute();
                    break;
            }
        }

        /// <summary>
        /// Maps a SkeletonPoint to lie within our render space and converts to Point
        /// </summary>
        /// <param name="skelpoint">point to map</param>
        /// <returns>mapped point</returns>
        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        /// <summary>
        /// Draws a bone line between two joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw bones from</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="jointType0">joint to start drawing from</param>
        /// <param name="jointType1">joint to end drawing at</param>
        private void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = this.inferredBonePen;
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            {
                drawPen = this.trackedBonePen;
            }

            drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));
        }

        /// <summary>
        /// Handles the checking or unchecking of the seated mode combo box
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void CheckBoxSeatedModeChanged(object sender, RoutedEventArgs e)
        {
            if (null != this.sensor)
            {
                if (this.checkBoxSeatedMode.IsChecked.GetValueOrDefault())
                {
                    this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                }
                else
                {
                    this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;
                }
            }
        }
    }
}