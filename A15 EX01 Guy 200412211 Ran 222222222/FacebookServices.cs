using FacebookWrapper;
using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Facebook
{
    public static class FacebookServices
    {
        public static User LoggedUser { get; set; }

        public static void Login()
        {
            try
            {
                LoginResult result = FacebookService.Login("761015400640072",
                "user_about_me", "user_friends", "friends_about_me", "publish_stream", "user_events", "read_stream", "user_videos",
                "user_status");

                FacebookService.s_CollectionLimit = 45;

                if (!string.IsNullOrEmpty(result.AccessToken))
                {
                    LoggedUser = result.LoggedInUser;
                }
                else
                {
                    LoggedUser = null;
                    throw new Exception(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}{1}{2}", "Error login to facebook service",Environment.NewLine, ex.Message));
            }
        }

        public static List<Post> GetAllVideosInNewsFeed()
        {
            List<Post> list = new List<Post>();

            foreach (Post pst in FacebookServices.LoggedUser.NewsFeed)
            {
                // Filter only video posts that users uploaded (and not linked to youtube)
                if ((pst.Type == Post.eType.video) && (!pst.Source.Contains("www.youtube.com")))
                {
                    list.Add(pst);
                }
            }
            return list;
        }
    }
}
