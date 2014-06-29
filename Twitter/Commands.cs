using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Twitter
{
    public class Commands
    {
        public TwitterContext Master { get; set; }
        public Commands(TwitterContext master)
        {
            this.Master = master;
        }

        public Task<Twitter.Status> Favorite(string id)
        {
            return Twitter.APIs.REST.Favorites.Create(this.Master, id);
        }
    }
}
