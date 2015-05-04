using System.Windows.Forms;
using System.Threading;
namespace MediaController
{
    public class SpotifyController
    {

        // If there is music playing or not
        bool playing = false;

        public void play()
        {
            // If not playing, play. Else do nothing
            if (playing == false)
            {
                SendKeys.SendWait(" ");
                this.playing = true;
            }
        }

        public void pause()
        {
            // If playing, pause. Else do nothing
            if (playing == true)
            {
                SendKeys.SendWait(" ");
                this.playing = false;
            }
        }

        public void next()
        {
            SendKeys.SendWait("^{RIGHT}");
        }


        public void previous()
        {
            SendKeys.SendWait("^{LEFT}");
        }


        public void volumeUp()
        {
            SendKeys.SendWait("^{UP}");
        }


        public void volumeDown()
        {
            SendKeys.SendWait("^{DOWN}");
        }


        public void focusSearchBar()
        {
            SendKeys.SendWait("^(l)");
        }

        public void backspace()
        {
            SendKeys.SendWait("{BS}");
        }

        public void profilePage()
        {
            SendKeys.SendWait("^(p)");
        }

        public void pressUp()
        {
            SendKeys.SendWait("{UP}");
        }

        public void pressDown()
        {
            SendKeys.SendWait("{DOWN}");
        }

        public void pressLeft()
        {
            SendKeys.SendWait("{LEFT}");
        }

        public void pressRight()
        {
            SendKeys.SendWait("{RIGHT}");
        }

        public void logout()
        {
            SendKeys.SendWait("^+(w)");
        }

        public void exitSpotify()
        {
            SendKeys.SendWait("%{F4}");
        }

        public void pressEnter()
        {
            SendKeys.SendWait("{ENTER}");
        }

        public void pressTab()
        {
            SendKeys.SendWait("{TAB}");
        }

        public void playBestSong()
        {
            SendKeys.SendWait("%{DOWN}");
            SendKeys.SendWait("%{DOWN}");
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(1000);
            SendKeys.SendWait("{ENTER}");
           // SendKeys.SendWait("{ENTER}");
        }

        /// <summary>
        /// Selects the top song for the searched artist
        /// </summary>
        public void playArtistSong()
        {
            SendKeys.SendWait("%{DOWN}");
            SendKeys.SendWait("%{DOWN}");
            SendKeys.SendWait("%{DOWN}");
            SendKeys.SendWait("%{DOWN}");
            SendKeys.SendWait("%{DOWN}");
            SendKeys.SendWait("{ENTER}");
            //SendKeys.SendWait("{ENTER}");
            //focusSearchBar();
            Thread.Sleep(5000);
            tabToFirstArtistSong();
            //SendKeys.SendWait("{ENTER}");
            SendKeys.SendWait("{ENTER}");

        }

        /// <summary>
        /// Plays the best album from the searched album
        /// </summary>
        public void playAlbumSong()
        {
            SendKeys.SendWait("%{DOWN}");
            SendKeys.SendWait("{ENTER}");
            //focusSearchBar();
            tabToFirstSong();
            SendKeys.SendWait("{ENTER}");
            SendKeys.SendWait("{ENTER}");
        }

        /// <summary>
        /// Moves to first song
        /// </summary>
        public void tabToFirstSong()
        {
            for (int x = 0; x < 23; x++){
                SendKeys.SendWait("{TAB}");
            }
            SendKeys.SendWait("{DOWN}");
            SendKeys.SendWait("{UP}");
        }

        /// <summary>
        /// Moves to first artists song
        /// </summary>
        public void tabToFirstArtistSong()
        {
            for (int x = 0; x < 45; x++)
            {
                SendKeys.SendWait("{TAB}");
                SendKeys.SendWait("{DOWN}");
                SendKeys.SendWait("{UP}");
            }
        }

        /// <summary>
        /// Clears the search bar of text
        /// </summary>
        public void clearSearchBar()
        {
            focusSearchBar();
            SendKeys.SendWait("^(a)");
            SendKeys.SendWait("{DEL}");
        }

        public void typeLetterSpace()
        {
            SendKeys.SendWait(" ");
        }

        public void typeLetterA()
        {
            SendKeys.SendWait("A");
        }

        public void typeLetterB()
        {
            SendKeys.SendWait("B");
        }

        public void typeLetterC()
        {
            SendKeys.SendWait("C");
        }

        public void typeLetterD()
        {
            SendKeys.SendWait("D");
        }

        public void typeLetterE()
        {
            SendKeys.SendWait("E");
        }

        public void typeLetterF()
        {
            SendKeys.SendWait("F");
        }

        public void typeLetterG()
        {
            SendKeys.SendWait("G");
        }

        public void typeLetterH()
        {
            SendKeys.SendWait("H");
        }

        public void typeLetterI()
        {
            SendKeys.SendWait("I");
        }

        public void typeLetterJ()
        {
            SendKeys.SendWait("J");
        }

        public void typeLetterK()
        {
            SendKeys.SendWait("K");
        }

        public void typeLetterL()
        {
            SendKeys.SendWait("L");
        }

        public void typeLetterM()
        {
            SendKeys.SendWait("M");
        }

        public void typeLetterN()
        {
            SendKeys.SendWait("N");
        }

        public void typeLetterO()
        {
            SendKeys.SendWait("O");
        }

        public void typeLetterP()
        {
            SendKeys.SendWait("P");
        }

        public void typeLetterQ()
        {
            SendKeys.SendWait("Q");
        }

        public void typeLetterR()
        {
            SendKeys.SendWait("R");
        }

        public void typeLetterS()
        {
            SendKeys.SendWait("S");
        }

        public void typeLetterT()
        {
            SendKeys.SendWait("T");
        }

        public void typeLetterU()
        {
            SendKeys.SendWait("U");
        }

        public void typeLetterV()
        {
            SendKeys.SendWait("V");
        }

        public void typeLetterW()
        {
            SendKeys.SendWait("W");
        }

        public void typeLetterX()
        {
            SendKeys.SendWait("X");
        }

        public void typeLetterY()
        {
            SendKeys.SendWait("Y");
        }

        public void typeLetterZ()
        {
            SendKeys.SendWait("Z");
        }

        public void mute()
        {
            for (int i = 0; i < 16; i++)
            {
                SendKeys.SendWait("^{DOWN}");
            }
        }
    }
}
