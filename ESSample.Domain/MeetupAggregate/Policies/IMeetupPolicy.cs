namespace ESSample.Domain.MeetupAggregate.Policies
{
    using System;
    using System.Threading.Tasks;

    public interface IMeetupPolicy
    {
        Task CheckCanDefineMeetupAsync(OrganizerId organizerId, DateTime when);
    }
}
