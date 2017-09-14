using System;

namespace Auction.Models
{

    public class Product : BaseEntity
    {
        public string photo { get; set; }
        public string product_name { get; set; }
        public string product_starting_bid { get; set; }
        public string product_description { get; set; }
        public int owner { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

}