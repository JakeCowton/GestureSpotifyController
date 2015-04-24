using System.Windows.Forms;

namespace MediaController
{
    public class SpotifyController
    {
        public void play_pause()
        {
            SendKeys.SendWait(" ");
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

        public void mute()
        {
            for (int i = 0; i < 16; i++)
            {
                SendKeys.SendWait("^{DOWN}");
            }
        }
    }
}
