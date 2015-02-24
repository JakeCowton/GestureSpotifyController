using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;

namespace SpotifyKinectInterface.VoiceControl
{
    public class VoiceControlEngine
    {

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor sensor;

        /// <summary>
        /// Speech recognition engine using audio data from Kinect.
        /// </summary>
        private SpeechRecognitionEngine speechEngine;

        private RecognizerInfo ri;

        public VoiceControlEngine(KinectSensor sensor)
        {
            this.sensor = sensor;
            this.ri = GetKinectRecognizer();
        }

        /// <summary>
        /// Gets the metadata for the speech recognizer (acoustic model) most suitable to
        /// process audio from Kinect device.
        /// </summary>
        /// <returns>
        /// RecognizerInfo if found, <code>null</code> otherwise.
        /// </returns>
        private static RecognizerInfo GetKinectRecognizer()
        {
            foreach (RecognizerInfo recognizer in SpeechRecognitionEngine.InstalledRecognizers())
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
            }

            return null;
        }

        public void voiceRec()
        {
            if (null != this.ri)
            {

                this.speechEngine = new SpeechRecognitionEngine(ri.Id);

                //Use this code to create grammar programmatically rather than from
                //a grammar file.

                var commands = new Choices();
                commands.Add(new SemanticResultValue("play", "PLAY"));
                commands.Add(new SemanticResultValue("pause", "PAUSE"));
                commands.Add(new SemanticResultValue("next", "NEXT"));
                commands.Add(new SemanticResultValue("previous", "PREVIOUS"));
                commands.Add(new SemanticResultValue("mute", "MUTE"));
                commands.Add(new SemanticResultValue("volume up", "VOLUME UP"));
                commands.Add(new SemanticResultValue("volume down", "VOLUME DOWN"));

                var gb = new GrammarBuilder { Culture = ri.Culture };
                gb.Append(commands);

                var g = new Grammar(gb);

                speechEngine.LoadGrammar(g);

                speechEngine.SpeechRecognized += SpeechRecognized;
                speechEngine.SpeechRecognitionRejected += SpeechRejected;

                // For long recognition sessions (a few hours or more), it may be beneficial to turn off adaptation of the acoustic model. 
                // This will prevent recognition accuracy from degrading over time.
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                speechEngine.SetInputToAudioStream(
                    sensor.AudioSource.Start(), new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Handler for recognized speech events.
        /// </summary>
        /// <param name="sender">object sending the event.</param>
        /// <param name="e">event arguments.</param>
        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Speech utterance confidence below which we treat speech as if it hadn't been heard
            const double ConfidenceThreshold = 0.3;

            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                switch (e.Result.Semantics.Value.ToString())
                {
                    case "PLAY":
                        Console.WriteLine("Play");
                        break;
                    case "PAUSE":
                        Console.WriteLine("Pause");
                        break;
                    case "NEXT":
                        Console.WriteLine("Next");
                        break;
                    case "PREVIOUS":
                        Console.WriteLine("Previous");
                        break;
                    case "MUTE":
                        Console.WriteLine("Mute");
                        break;
                    case "VOLUME UP":
                        Console.WriteLine("Volume up");
                        break;
                    case "VOLUME DOWN":
                        Console.WriteLine("Volume down");
                        break;
                }
            }
        }

        /// <summary>
        /// Handler for rejected speech events.
        /// </summary>
        /// <param name="sender">object sending the event.</param>
        /// <param name="e">event arguments.</param>
        private void SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            Console.WriteLine("Rejected");
        }

        public void kill()
        {

        }

    }
}
