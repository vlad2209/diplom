using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Specialized;
using System.Net.Http;
using System.IO;

namespace Telegram
{
    namespace Request
    {
        #region DefaultClass
        public class User
        {
            public int id;
            public string first_name;
            public string last_name;
            public string username;
        }
        public class Chat
        {
            public int id;
            public string type;
            public string title;
            public string username;
            public string first_name;
            public string last_name;

        }
        public class PhotoSize
        {
            public string file_id;
            public int width;
            public int height;
            public int file_size;
        }
        #endregion
        #region Делгаты
        public delegate void ResponseText(object sendr, MessageText e);
        public delegate void ResponsePhoto(object sendr, MessagePhoto e);
        public delegate void ResponseLocation(object sendr, MessageLocation e);
        public delegate void ResponseContact(object sendr, MessageContact e);
        #endregion
        #region Классы Типов
        public class MessageText : EventArgs
        {
            #region DefaulParameters
            public int message_id;
            public User from = new User();
            public Chat chat = new Chat();
            public int date;
            #endregion
            public string text;
        }

        public class MessagePhoto : EventArgs
        {
            #region DefaulParameters
            public int message_id;
            public User from = new User();
            public Chat chat = new Chat();
            public int date;
            #endregion
            public List<PhotoSize> photo = new List<PhotoSize>();
            public string caption;
        }
        public class MessageLocation : EventArgs
        {
            #region DefaulParameters
            public int message_id;
            public User from = new User();
            public Chat chat = new Chat();
            public int date;
            #endregion
            public float longitude;
            public float latitude;
        }
        public class MessageContact : EventArgs
        {
            #region DefaulParameters
            public int message_id;
            public User from = new User();
            public Chat chat = new Chat();
            public int date;
            #endregion
            public string phone_number;
            public string first_name;
            public string last_name;
            public int user_id;
        }


        public class TelegramRequest
        {
            public string _token;
            public TelegramRequest(string Token)
            {
                _token = Token;
            }

        }
    }


    class Method
    {
        string _token;
        string LINK = "https://api.telegram.org/bot";
        public Method(string Token)
        {
            _token = Token;
        }
        public string Getme()
        {
            using (WebClient webClient = new WebClient())
            {
                string response = webClient.DownloadString(LINK + _token + "/getMe");
                return response;
            }
        }

        public void ForwardMessage(int fromChatID, int chatId, int messageID)
        {
            using (WebClient webClient = new WebClient())
            {
                NameValueCollection pars = new NameValueCollection();
                pars.Add("chat_id", chatId.ToString());
                pars.Add("from_chat_id", fromChatID.ToString());
                pars.Add("message_id", messageID.ToString());
                webClient.UploadValues(LINK + _token + "/forwardMessage", pars);
            }
        }

        async public Task SendPhotoIputFile(int ChatID, string pathToPhoto, string catprion = "")
        {
            using (MultipartFormDataContent form = new MultipartFormDataContent())
            {
                string url = LINK + _token + "/sendPhoto";
                string fileName = pathToPhoto.Split('\\').Last();

                form.Add(new StringContent(ChatID.ToString(), Encoding.UTF8), "chat_id");
                form.Add(new StringContent(catprion.ToString(), Encoding.UTF8), "caption");
                using (FileStream fileStream = new FileStream(pathToPhoto, FileMode.Open, FileAccess.Read))
                {
                    form.Add(new StreamContent(fileStream), "photo", fileName);
                    using (HttpClient client = new HttpClient())
                        await client.PostAsync(url, form);
                }
            }

        }
    }
}


         #endregion