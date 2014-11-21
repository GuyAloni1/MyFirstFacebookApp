using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Facebook
{
    public class PlayList
    {
        private bool m_videoEnded = true;
        private System.Windows.Forms.Timer m_timerCheckVideos;
		private System.ComponentModel.IContainer m_playComponents;
        private int m_indexOfVideos = 0;
        private ArrayList m_videosInPlayList = new ArrayList();
		private AxWMPLib.AxWindowsMediaPlayer m_mediaPlayer;

        public PlayList(AxWMPLib.AxWindowsMediaPlayer i_axPlayer) 
        {
            m_mediaPlayer = i_axPlayer;
            m_indexOfVideos = 0;
            this.m_playComponents = new System.ComponentModel.Container();
            this.m_timerCheckVideos = new System.Windows.Forms.Timer(this.m_playComponents);
            this.m_timerCheckVideos.Tick += new System.EventHandler(this.CheckVideo_Tick);
            m_mediaPlayer.PlayStateChange +=
                new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(MediaPlayer_PlayStateChange);
        }

		public void AddVideosUrl(string[] i_videoList)
		{
			for (int i = 0; i < i_videoList.Length; i++)
			{
				AddVideoUrl(i_videoList[i]);
			}
		}

		public void AddVideoUrl(string i_videoURL)
		{
			m_videosInPlayList.Add(i_videoURL);
		}

		public void Play()
		{
			if (m_videosInPlayList[m_indexOfVideos] != null)
			{
				m_mediaPlayer.URL = m_videosInPlayList[m_indexOfVideos].ToString();
			}
		}

		public void Play(int i_slot)
		{
			if (m_videosInPlayList[i_slot - 1] != null)
				m_mediaPlayer.URL = m_videosInPlayList[i_slot - 1].ToString();
            m_indexOfVideos = i_slot - 1;
		}

		public void Pause()
		{
			m_mediaPlayer.Ctlcontrols.pause();
		}

		public void Stop()
		{
			m_mediaPlayer.Ctlcontrols.stop();
		}

		public void NextVideo()
		{
			if (m_indexOfVideos != m_videosInPlayList.Count - 1)
			{
				m_indexOfVideos++;
				m_mediaPlayer.Ctlcontrols.stop();
				m_mediaPlayer.URL = m_videosInPlayList[m_indexOfVideos].ToString();
				m_mediaPlayer.Ctlcontrols.play();
			}
			else
			{
				m_indexOfVideos = 0;
				m_mediaPlayer.Ctlcontrols.stop();
				m_mediaPlayer.URL = m_videosInPlayList[0].ToString();
				m_mediaPlayer.Ctlcontrols.play();
			}
		}

		public void PrevVideo()
		{
			if (m_indexOfVideos != 0)
			{
				m_indexOfVideos--;
				m_mediaPlayer.Ctlcontrols.stop();
				m_mediaPlayer.URL = m_videosInPlayList[m_indexOfVideos].ToString();
				m_mediaPlayer.Ctlcontrols.play();
			}
			else
			{
				m_indexOfVideos = m_videosInPlayList.Count - 1;
				m_mediaPlayer.Ctlcontrols.stop();
				m_mediaPlayer.URL = m_videosInPlayList[m_indexOfVideos].ToString();
				m_mediaPlayer.Ctlcontrols.play();
			}
		}

		private void CheckVideo_Tick(object sender, System.EventArgs e)
		{
			if (m_videoEnded)
			{
				NextVideo();
				m_videoEnded = false;
				m_timerCheckVideos.Stop();
			}
		}

		public void MediaPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
		{
			switch (m_mediaPlayer.playState)
			{
                case WMPLib.WMPPlayState.wmppsMediaEnded:
                    {
                        m_videoEnded = true;
                        m_timerCheckVideos.Start();
                        break;
                    }
                default:
                    {
                        break;
                    }
			}
		}
	}
}
