using Newtonsoft.Json;
using redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace casino
{
    public class Message
    {
        public string pattern;
        public string channel;
        public string message;
        public int src;
        public long? ttl;
        public DateTime ExpireAt;
    }
    public sealed class MessageManager
    {
        static readonly MessageManager _this = new MessageManager();

        #region IHttpModule

        public static void Initialize()
        {
            HttpApplication.RegisterModule(typeof(Module));
        }

        class Module : IHttpModule
        {
            void IHttpModule.Dispose() { }
            void IHttpModule.Init(HttpApplication app) { _this.ToString(); }
        }

        #endregion

        readonly RedisClient redis = RedisClient.GetClient(null, DB.Redis.Message1);
        public MessageManager()
        {
            redis.PubSub.PSUBSCRIBE("*", this.ReceiveMessage);
            //redis.PubSub.SUBSCRIBE("msg1", this.msg1);
            //message_channel_list.Add(new message_channel(this, "usermsg:{0}"));
        }

        void ReceiveMessage(RedisClient sender, string pattern, string channel, string message)
        {
            Message msg = new Message()
            {
                pattern = pattern,
                channel = channel,
                message = message,
                src = _HttpContext.Current._User.ID,
            };
        }

        [api.http("~/msg/post")]
        static int PublishMessage(string channel, string message)
        {
            SqlBuilder sql = new SqlBuilder();
            sql["*", "channel"] = channel.Trim(true);
            sql["*", "message"] = message.Trim(true);
            sql.TestMissingFields(true);
            return _HttpContext.Current.GetRedis(null, DB.Redis.General).PubSub.PUBLISH(channel, message);
        }

        //const string user_n = "usermsg:{0}";
        //List<message_channel> message_channel_list = new List<message_channel>();
        //class message_channel
        //{
        //    readonly UserManager owner;
        //    readonly string pattern;
        //    public message_channel(UserManager owner, string pattern)
        //    {
        //        this.owner = owner;
        //        this.pattern = pattern;
        //        owner.redis_msg1.PubSub.PSUBSCRIBE(string.Format(pattern, '*'), this.OnMessage);
        //    }

        //    void OnMessage(RedisClient sender, string pattern, string channel, string message)
        //    {
        //        Minimatch.Minimatcher m = new Minimatch.Minimatcher(channel);
        //        List<User> dest = null;
        //        lock (owner._users)
        //        {
        //            foreach (User user in owner._users)
        //            {
        //                if (!user.IsAlive) continue;
        //                string p = string.Format(this.pattern, user.ID);
        //                if (m.IsMatch(p))
        //                {
        //                    if (dest == null)
        //                        dest = new List<User>();
        //                    dest.Add(user);
        //                }
        //            }
        //        }
        //        if (dest != null)
        //            foreach (User user in dest)
        //                user.ReceiveMessage(sender, pattern, channel, message);
        //    }
        //}
    }
}
