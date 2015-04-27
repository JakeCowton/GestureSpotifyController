using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using MediaController;

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

        /// <summary>
        /// The speech recogniser information
        /// </summary>
        private RecognizerInfo ri;

        private Boolean searchBarFocused = false;

        private SpotifyController spotifyController;

        public VoiceControlEngine(KinectSensor sensor)
        {
            this.sensor = sensor;
            this.ri = GetKinectRecognizer();
            this.spotifyController = new SpotifyController();
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
                commands.Add(new SemanticResultValue("search", "SEARCH"));
                commands.Add(new SemanticResultValue("song", "SONG"));
                commands.Add(new SemanticResultValue("artist", "ARTIST"));
                commands.Add(new SemanticResultValue("space", "SPACE"));
                commands.Add(new SemanticResultValue("select", "SELECT"));
                commands.Add(new SemanticResultValue("logout", "LOGOUT"));
                commands.Add(new SemanticResultValue("profile", "PROFILE"));
                commands.Add(new SemanticResultValue("left", "LEFT"));
                commands.Add(new SemanticResultValue("up", "UP"));
                commands.Add(new SemanticResultValue("down", "DOWN"));
                commands.Add(new SemanticResultValue("right", "RIGHT"));
                commands.Add(new SemanticResultValue("exit", "EXIT"));
                commands.Add(new SemanticResultValue("backspace", "BACKSPACE"));
                commands.Add(new SemanticResultValue("clear", "CLEAR"));
                commands.Add(new SemanticResultValue("tab", "TAB"));
                commands.Add(new SemanticResultValue("a", "A"));
                commands.Add(new SemanticResultValue("b", "B"));
                commands.Add(new SemanticResultValue("c", "C"));
                commands.Add(new SemanticResultValue("d", "D"));
                commands.Add(new SemanticResultValue("E", "E"));
                commands.Add(new SemanticResultValue("f", "F"));
                commands.Add(new SemanticResultValue("g", "G"));
                commands.Add(new SemanticResultValue("h", "H"));
                commands.Add(new SemanticResultValue("i", "I"));
                commands.Add(new SemanticResultValue("j", "J"));
                commands.Add(new SemanticResultValue("k", "K"));
                commands.Add(new SemanticResultValue("l", "L"));
                commands.Add(new SemanticResultValue("m", "M"));
                commands.Add(new SemanticResultValue("n", "N"));
                commands.Add(new SemanticResultValue("o", "O"));
                commands.Add(new SemanticResultValue("p", "P"));
                commands.Add(new SemanticResultValue("q", "Q"));
                commands.Add(new SemanticResultValue("r", "R"));
                commands.Add(new SemanticResultValue("s", "S"));
                commands.Add(new SemanticResultValue("t", "T"));
                commands.Add(new SemanticResultValue("u", "U"));
                commands.Add(new SemanticResultValue("v", "V"));
                commands.Add(new SemanticResultValue("w", "W"));
                commands.Add(new SemanticResultValue("x", "X"));
                commands.Add(new SemanticResultValue("y", "Y"));
                commands.Add(new SemanticResultValue("z", "Z"));

                var gb = new GrammarBuilder { Culture = ri.Culture };
                gb.Append(commands);

                var g = new Grammar(gb);

                speechEngine.LoadGrammar(g);

                speechEngine.SpeechRecognized += SpeechRecognized;

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
                        this.spotifyController.play();
                        break;
                    case "PAUSE":
                        Console.WriteLine("Pause");
                        this.spotifyController.pause();
                        break;
                    case "NEXT":
                        Console.WriteLine("Next");
                        if (searchBarFocused == false)
                        {
                            this.spotifyController.next();
                        }
                        else
                        {
                            this.spotifyController.pressTab();
                            this.spotifyController.pressTab();
                            this.spotifyController.next();
                            this.spotifyController.focusSearchBar();
                            searchBarFocused = true;
                        }
                        break;
                    case "PREVIOUS":
                        Console.WriteLine("Previous");
                        if (searchBarFocused == false)
                        {
                        this.spotifyController.previous();
                        }
                        else
                        {
                            this.spotifyController.pressTab();
                            this.spotifyController.pressTab();
                            this.spotifyController.previous();
                            this.spotifyController.focusSearchBar();
                            searchBarFocused = true;
                        }
                        break;
                    case "VOLUME UP":
                        Console.WriteLine("Volume up");
                        if (searchBarFocused == false)
                        {
                            this.spotifyController.volumeUp();
                        }
                        else
                        {
                            this.spotifyController.pressTab();
                            this.spotifyController.pressTab();
                            this.spotifyController.volumeUp();
                            this.spotifyController.focusSearchBar();
                            searchBarFocused = true;
                        }
                        break;
                    case "VOLUME DOWN":
                        Console.WriteLine("Volume down");
                        if (searchBarFocused == false)
                        {
                            this.spotifyController.volumeDown();
                        }
                        else
                        {
                            this.spotifyController.pressTab();
                            this.spotifyController.pressTab();
                            this.spotifyController.volumeDown();
                            this.spotifyController.focusSearchBar();
                            searchBarFocused = true;
                        }
                        break;
                    case "MUTE":
                        Console.WriteLine("Mute");
                        if(searchBarFocused == false){
                        this.spotifyController.mute();
                        }
                        else 
                        {
                            this.spotifyController.pressTab();
                            this.spotifyController.pressTab();
                            this.spotifyController.mute();
                            this.spotifyController.focusSearchBar();
                            searchBarFocused = true; 
                        }
                        break;
                    
                    case "SEARCH":
                        //this.spotifyController.pressTab();
                        //this.spotifyController.pressTab();
                        this.spotifyController.focusSearchBar();
                        Console.WriteLine("Search:...");
                        searchBarFocused = true; 
                        break;

                    case "SONG":
                        Console.WriteLine("Deciding Best song to play");
                        if (searchBarFocused == true)
                        {
                            this.spotifyController.playBestSong();
                            this.spotifyController.pressEnter();
                            searchBarFocused = false;
                        }
                        else
                        {
                            this.spotifyController.focusSearchBar();
                            this.spotifyController.playBestSong();
                            this.spotifyController.pressEnter();
                            searchBarFocused = false;
                        }
                        break;

                    case "ARTIST":
                        Console.WriteLine("Deciding song by artist search to play");
                        if (searchBarFocused == true)
                        {
                            this.spotifyController.playArtistSong();
                            searchBarFocused = false;
                        }
                        else
                        {
                            this.spotifyController.focusSearchBar();
                            this.spotifyController.playArtistSong();
                            searchBarFocused = false;
                        }
                        break;

                    case "SPACE":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed a Space");
                            this.spotifyController.typeLetterSpace();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "SELECT":
                            Console.WriteLine("Pressed Enter");
                            this.spotifyController.pressEnter();
                            break;

                    case "CLEAR":
                            Console.WriteLine("Search Field Cleared");
                            this.spotifyController.clearSearchBar();
                            break;
                    case "BACKSPACE":
                            Console.WriteLine("Removed a character from search field");
                            this.spotifyController.backspace();
                            break;
                    case "PROFILE":
                            Console.WriteLine("Navigated to My Profile page");
                            this.spotifyController.profilePage();
                            break;
                    case "LOGOUT":
                            Console.WriteLine("Logged out of profile");
                            this.spotifyController.logout();
                            break;
                    case "EXIT":
                            Console.WriteLine("Closed the Spotify Application");
                            this.spotifyController.exitSpotify();
                            break;
                    case "UP":
                            Console.WriteLine("Up Arrow Key Pressed");
                            this.spotifyController.pressUp();
                            break;
                    case "DOWN":
                            Console.WriteLine("Down Arrow Key Pressed");
                            this.spotifyController.pressDown();
                            break;
                    case "LEFT":
                            Console.WriteLine("Left Arrow Key Pressed");
                            this.spotifyController.pressLeft();
                            break;
                    case "RIGHT":
                            Console.WriteLine("Right Arrow Key Pressed");
                            this.spotifyController.pressRight();
                            break;
                        
                    case "A":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'A'");
                            this.spotifyController.typeLetterA();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;

                    case "B":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'B'");
                            this.spotifyController.typeLetterB();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;

                    case "C":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'C'");
                            this.spotifyController.typeLetterC();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "D":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'D'");
                            this.spotifyController.typeLetterD();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "E":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'A'");
                            this.spotifyController.typeLetterE();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "F":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'F'");
                            this.spotifyController.typeLetterF();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "G":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'G'");
                            this.spotifyController.typeLetterG();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "H":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'H'");
                            this.spotifyController.typeLetterH();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "I":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'I'");
                            this.spotifyController.typeLetterI();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "J":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'J'");
                            this.spotifyController.typeLetterJ();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "K":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'K'");
                            this.spotifyController.typeLetterK();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "L":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'L'");
                            this.spotifyController.typeLetterL();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;

                    case "M":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'M'");
                            this.spotifyController.typeLetterM();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "N":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'N'");
                            this.spotifyController.typeLetterN();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "O":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'O'");
                            this.spotifyController.typeLetterO();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "P":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'P'");
                            this.spotifyController.typeLetterP();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "Q":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'Q'");
                            this.spotifyController.typeLetterQ();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "R":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'R'");
                            this.spotifyController.typeLetterR();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "S":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'S'");
                            this.spotifyController.typeLetterS();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "T":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'T'");
                            this.spotifyController.typeLetterT();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "U":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'U'");
                            this.spotifyController.typeLetterU();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "V":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'V'");
                            this.spotifyController.typeLetterV();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "W":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'W'");
                            this.spotifyController.typeLetterW();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "X":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'X'");
                            this.spotifyController.typeLetterX();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                    case "Y":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'Y'");
                            this.spotifyController.typeLetterY();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;

                    case "Z":
                        if (searchBarFocused == true)
                        {
                            Console.WriteLine("Typed 'Z'");
                            this.spotifyController.typeLetterZ();
                        }
                        else
                        {
                            Console.WriteLine("To type a character please say 'SEARCH' first...");
                        }
                        break;
                }
            }
        }

        public void kill()
        {
            this.speechEngine.Dispose();
        }

    }
}
