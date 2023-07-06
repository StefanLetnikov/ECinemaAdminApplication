using System;
using System.ComponentModel.DataAnnotations;

namespace ECinemaAdminApplication.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string MovieName { get; set; }

        public string MovieDescription { get; set; }

        public int MovieRating { get; set; }

        public int TicketPrice { get; set; }

        public string MovieImage { get; set; }
    }
}
