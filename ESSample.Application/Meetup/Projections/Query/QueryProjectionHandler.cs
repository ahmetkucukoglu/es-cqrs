namespace ESSample.Application.Meetup.Projections.Query
{
    using Domain.MeetupAggregate.Events;
    using MediatR;
    using Models;
    using Raven.Client.Documents;
    using Raven.Client.Documents.Session;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class QueryProjectionHandler : INotificationHandler<ProjectionNotification>
    {
        private readonly DocumentStore _documentStore;

        public QueryProjectionHandler(DocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        private bool IsSupported(object e)
            => e is MeetupRegistered
            || e is MeetupSubjectChanged
            || e is MeetupWhenChanged
            || e is MeetupDescriptionChanged
            || e is MeetupAddressChanged
            || e is MeetupLocationAdded
            || e is MeetupJoined
            || e is MeetupCompleted
            || e is MeetupPhotoAdded
            || e is MeetupCancelled
            || e is MeetupCommentAdded;

        private async Task<MeetupDocument> GetDocument(IAsyncDocumentSession session, Guid id, CancellationToken cancellationToken)
        {
            var document = await session.LoadAsync<MeetupDocument>(id.ToString());

            if (document == null)
            {
                document = new MeetupDocument
                {
                    MeetupId = id
                };

                await session.StoreAsync(document, id.ToString(), cancellationToken);
            }

            return document;
        }

        public async Task Handle(ProjectionNotification notification, CancellationToken cancellationToken)
        {
            if (!IsSupported(notification.DomainEvent))
                return;

            using (var session = _documentStore.OpenAsyncSession())
            {
                switch (notification.DomainEvent)
                {
                    case MeetupRegistered x:
                        OnRegistered(await GetDocument(session, x.MeetupId, cancellationToken), x);
                        break;
                    case MeetupSubjectChanged x:
                        OnSubjectChanged(await GetDocument(session, x.MeetupId, cancellationToken), x);
                        break;
                    case MeetupWhenChanged x:
                        OnWhenChanged(await GetDocument(session, x.MeetupId, cancellationToken), x);
                        break;
                    case MeetupDescriptionChanged x:
                        OnDescriptionChanged(await GetDocument(session, x.MeetupId, cancellationToken), x);
                        break;
                    case MeetupAddressChanged x:
                        OnAddressChanged(await GetDocument(session, x.MeetupId, cancellationToken), x);
                        break;
                    case MeetupLocationAdded x:
                        OnLocationAdded(await GetDocument(session, x.MeetupId, cancellationToken), x);
                        break;
                    case MeetupJoined x:
                        OnJoined(await GetDocument(session, x.MeetupId, cancellationToken), x);
                        break;
                    case MeetupCompleted x:
                        OnCompleted(await GetDocument(session, x.MeetupId, cancellationToken), x);
                        break;
                    case MeetupPhotoAdded x:
                        OnPhotoAdded(await GetDocument(session, x.MeetupId, cancellationToken), x);
                        break;
                    case MeetupCancelled x:
                        OnCancelled(await GetDocument(session, x.MeetupId, cancellationToken), x);
                        break;
                    case MeetupCommentAdded x:
                        OnCommentAdded(await GetDocument(session, x.MeetupId, cancellationToken), x);
                        break;
                }

                await session.SaveChangesAsync();
            }
        }

        private void OnRegistered(MeetupDocument document, MeetupRegistered @event)
        {
            document.MeetupId = @event.MeetupId;
            document.OrganizerId = @event.OrganizerId;
            document.Subject = @event.Subject;
            document.When = @event.When;
            document.Description = @event.Description;
            document.Location = new MeetupLocation
            {
                Address = @event.Address
            };
        }

        private void OnSubjectChanged(MeetupDocument document, MeetupSubjectChanged @event)
        {
            document.MeetupId = @event.MeetupId;
            document.Subject = @event.Subject;
        }

        private void OnWhenChanged(MeetupDocument document, MeetupWhenChanged @event)
        {
            document.MeetupId = @event.MeetupId;
            document.When = @event.When;
        }

        private void OnDescriptionChanged(MeetupDocument document, MeetupDescriptionChanged @event)
        {
            document.MeetupId = @event.MeetupId;
            document.Description = @event.Description;
        }

        private void OnAddressChanged(MeetupDocument document, MeetupAddressChanged @event)
        {
            document.MeetupId = @event.MeetupId;
            document.Location.Address = @event.Address;
        }

        private void OnLocationAdded(MeetupDocument document, MeetupLocationAdded @event)
        {
            document.Location.Latitude = @event.Latitude;
            document.Location.Longitude = @event.Longitude;
        }

        private void OnJoined(MeetupDocument document, MeetupJoined @event)
        {
            document.Participants.Add(new MeetupParticipant
            {
                ParticipantId = @event.ParticipantId,
                CreatedDate = @event.CreatedDate
            });
        }

        private void OnCompleted(MeetupDocument document, MeetupCompleted @event)
        {
            document.Completed = true;
        }

        private void OnPhotoAdded(MeetupDocument document, MeetupPhotoAdded @event)
        {
            document.Photos.Add(new MeetupPhoto
            {
                PhotographerId = @event.PhotographerId,
                Photo = @event.PhotoPath,
                CreatedDate = @event.CreatedDate
            });
        }

        private void OnCancelled(MeetupDocument document, MeetupCancelled @event)
        {
            document.Cancelled = true;
        }

        private void OnCommentAdded(MeetupDocument document, MeetupCommentAdded @event)
        {
            document.Comments.Add(new MeetupComment
            {
                CommentatorId = @event.CommentatorId,
                Comment = @event.Comment,
                CreatedDate = @event.CreatedDate
            });
        }
    }
}
