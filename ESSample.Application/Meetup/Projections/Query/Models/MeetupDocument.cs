namespace ESSample.Application.Meetup.Projections.Query.Models
{
    using System;
    using System.Collections.Generic;

    public class MeetupDocument
    {
        public MeetupDocument()
        {
            Participants = new List<MeetupParticipant>();
            Photos = new List<MeetupPhoto>();
            Comments = new List<MeetupComment>();
        }

        public Guid MeetupId { get; set; }
        public Guid OrganizerId { get; set; }
        public string Subject { get; set; }
        public DateTime When { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public bool Cancelled { get; set; }
        public MeetupLocation Location { get; set; }
        public List<MeetupParticipant> Participants { get; set; }
        public List<MeetupPhoto> Photos { get; set; }
        public List<MeetupComment> Comments { get; set; }
    }
}
