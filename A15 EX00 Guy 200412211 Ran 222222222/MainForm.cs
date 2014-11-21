using FacebookWrapper;
using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Facebook
{
    public partial class MainForm : Form
    {
        private PlayList m_playList = null;

        public MainForm()
        {
            InitializeComponent();
            m_playList = new PlayList(axWindowsMediaPlayer1);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Login();

            if (FacebookServices.LoggedUser != null)
            {
                InitVideoList();
            }
            else
            {
                DisableButtons();
            }
        }

        private void InitVideoList()
        {
            List<Post> listOfVideoPosts = FacebookServices.GetAllVideosInNewsFeed();
            InitImageListOfVideos(listOfVideoPosts);
            CreateListViewItemsFromPosts(listOfVideoPosts);
            AddVideosToThePlayList(listOfVideoPosts);

            m_playList.Play(1);
        }

        private void AddVideosToThePlayList(List<Post> list)
        {
            var listOfURLs = list.Select(x => x.Source).ToArray();
            m_playList.AddVideosUrl(listOfURLs);
        }

        private void CreateListViewItemsFromPosts(List<Post> list)
        {
            int countPosts = 0;

            foreach (Post post in list)
            {
                ListViewItem lst = new ListViewItem();
                lst.Text = post.Name;
                lst.ImageIndex = countPosts++;
                lst.Tag = post;
                listView1.Items.Add(lst);
            }
        }

        private void InitImageListOfVideos(List<Post> i_postList)
        {
            List<string> listOfPicturesURL = i_postList.Select(x => x.PictureURL).ToList();

            ImageList imageList = new ImageList();
            foreach (string img in listOfPicturesURL)
            {
                Bitmap bmp = CreateBitmapFromImageURL(img);
                imageList.Images.Add(bmp);
            }

            imageList.ImageSize = new Size(90, 90);
            listView1.LargeImageList = imageList;
        }

        private static Bitmap CreateBitmapFromImageURL(string img)
        {
            System.Net.WebRequest request = System.Net.WebRequest.Create(img);
            System.Net.WebResponse resp = request.GetResponse();
            System.IO.Stream respStream = resp.GetResponseStream();
            Bitmap bmp = new Bitmap(respStream);
            respStream.Dispose();
            return bmp;
        }
        
        private void Login()
        {
            try
            {
                FacebookServices.Login();

                pictureBox1.LoadAsync(FacebookServices.LoggedUser.PictureNormalURL);
                labelUserName.Text = FacebookServices.LoggedUser.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected) 
            {
                m_playList.Play(e.ItemIndex + 1);
            }
        }

        private void buttonPrevSong_Click(object sender, EventArgs e)
        {
            m_playList.PrevVideo();
        }

        private void buttonNextSong_Click(object sender, EventArgs e)
        {
            m_playList.NextVideo();
        }

        private void DisableButtons()
        {
            buttonNextSong.Enabled = false;
            buttonPrevious.Enabled = false;
        }
    }
}
