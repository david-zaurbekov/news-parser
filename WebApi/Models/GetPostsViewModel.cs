using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class GetPostsViewModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}