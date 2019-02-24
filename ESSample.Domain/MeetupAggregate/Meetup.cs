namespace ESSample.Domain.MeetupAggregate
{
    using Events;
    using Exceptions;
    using Policies;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Meetup : AggregateRoot
    {
        public Subject Subject { get; private set; }
        public When When { get; private set; }
        public Description Description { get; private set; }
        public Location Location { get; private set; }
        public OrganizerId OrganizerId { get; private set; }
        public bool Completed { get; private set; }
        public bool Cancelled { get; private set; }

        private readonly List<MeetupParticipant> _participants = new List<MeetupParticipant>();
        public IReadOnlyCollection<MeetupParticipant> Participants => _participants;

        private readonly List<MeetupPhoto> _photos = new List<MeetupPhoto>();
        public IReadOnlyCollection<MeetupPhoto> Photos => _photos;

        private readonly List<MeetupComment> _comments = new List<MeetupComment>();
        public IReadOnlyCollection<MeetupComment> Comments => _comments;

        protected override void DoApply(DomainEvent @event)
        {
            switch (@event)
            {
                case MeetupRegistered x: OnRegistered(x); break;
                case MeetupSubjectChanged x: OnSubjectChanged(x); break;
                case MeetupWhenChanged x: OnWhenChanged(x); break;
                case MeetupDescriptionChanged x: OnDescriptionChanged(x); break;
                case MeetupAddressChanged x: OnAddressChanged(x); break;
                case MeetupLocationAdded x: OnLocationAdded(x); break;
                case MeetupJoined x: OnJoined(x); break;
                case MeetupCompleted x: OnCompleted(x); break;
                case MeetupPhotoAdded x: OnPhotoAdded(x); break;
                case MeetupCancelled x: OnCancelled(x); break;
                case MeetupCommentAdded x: OnCommentAdded(x); break;
            }
        }

        public async Task Register(MeetupId meetupId, OrganizerId organizerId, Subject subject, When when, Description description, Address address, IMeetupPolicy policy)
        {
            if (Version >= 0)
            {
                throw new MeetupAlreadyRegisteredException();
            }

            await policy.CheckCanDefineMeetupAsync(organizerId, when);

            ApplyEvent(new MeetupRegistered
            {
                MeetupId = meetupId,
                OrganizerId = organizerId,
                Subject = subject,
                When = when,
                Description = description,
                Address = address,
                CreatedDate = DateTime.Now
            });
        }

        public void ChangeSubject(OrganizerId organizerId, Subject subject)
        {
            if (Version == -1)
            {
                throw new MeetupNotFoundException();
            }

            if (Completed)
            {
                throw new MeetupCompletedException();
            }

            if (Cancelled)
            {
                throw new MeetupCancelledException();
            }

            if (organizerId != OrganizerId)
            {
                throw new MeetupDomainException("You are not organizer this meetup");
            }

            if (Subject == subject)
                return;

            ApplyEvent(new MeetupSubjectChanged
            {
                MeetupId = Id,
                Subject = subject,
                ChangedDate = DateTime.Now
            });
        }

        public void ChangeWhen(OrganizerId organizerId, When when)
        {
            if (Version == -1)
            {
                throw new MeetupNotFoundException();
            }

            if (Completed)
            {
                throw new MeetupCompletedException();
            }

            if (Cancelled)
            {
                throw new MeetupCancelledException();
            }

            if (organizerId != OrganizerId)
            {
                throw new MeetupDomainException("You are not organizer this meetup");
            }

            if (When == when)
                return;

            ApplyEvent(new MeetupWhenChanged
            {
                MeetupId = Id,
                When = when,
                ChangedDate = DateTime.Now
            });
        }

        public void ChangeDescription(OrganizerId organizerId, Description description)
        {
            if (Version == -1)
            {
                throw new MeetupNotFoundException();
            }

            if (Completed)
            {
                throw new MeetupCompletedException();
            }

            if (Cancelled)
            {
                throw new MeetupCancelledException();
            }

            if (organizerId != OrganizerId)
            {
                throw new MeetupDomainException("You are not organizer this meetup");
            }

            if (Description == description)
                return;

            ApplyEvent(new MeetupDescriptionChanged
            {
                MeetupId = Id,
                Description = description,
                ChangedDate = DateTime.Now
            });
        }

        public void ChangeAddress(OrganizerId organizerId, Address address)
        {
            if (Version == -1)
            {
                throw new MeetupNotFoundException();
            }

            if (Completed)
            {
                throw new MeetupCompletedException();
            }

            if (Cancelled)
            {
                throw new MeetupCancelledException();
            }

            if (organizerId != OrganizerId)
            {
                throw new MeetupDomainException("You are not organizer this meetup");
            }

            if (Location.Address == address)
                return;

            ApplyEvent(new MeetupAddressChanged
            {
                MeetupId = Id,
                Address = address,
                ChangedDate = DateTime.Now
            });
        }

        public void AddLocation(Latitude latitude, Longitude longitude)
        {
            if (Version == -1)
            {
                throw new MeetupNotFoundException();
            }

            if (Completed)
            {
                throw new MeetupCompletedException();
            }

            if (Cancelled)
            {
                throw new MeetupCancelledException();
            }

            ApplyEvent(new MeetupLocationAdded
            {
                MeetupId = Id,
                Latitude = longitude,
                Longitude = longitude,
                CreatedDate = DateTime.Now
            });
        }

        public void Join(ParticipantId participantId)
        {
            if (Version == -1)
            {
                throw new MeetupNotFoundException();
            }

            if (Completed)
            {
                throw new MeetupCompletedException();
            }

            if (Cancelled)
            {
                throw new MeetupCancelledException();
            }

            if (_participants.Count((x) => x.ParticipantId == participantId) > 0)
            {
                throw new MeetupDomainException("Participant already joined.");
            }

            ApplyEvent(new MeetupJoined
            {
                MeetupId = Id,
                ParticipantId = participantId,
                CreatedDate = DateTime.Now
            });
        }

        public void Complete(OrganizerId organizerId)
        {
            if (Version == -1)
            {
                throw new MeetupNotFoundException();
            }

            if (Completed)
            {
                throw new MeetupCompletedException();
            }

            if (Cancelled)
            {
                throw new MeetupCancelledException();
            }

            if (organizerId != OrganizerId)
            {
                throw new MeetupDomainException("You are not organizer this meetup");
            }

            ApplyEvent(new MeetupCompleted
            {
                MeetupId = Id,
                CreatedDate = DateTime.Now
            });
        }

        public void AddPhoto(PhotographerId photographerId, Photo photo)
        {
            if (Version == -1)
            {
                throw new MeetupNotFoundException();
            }

            if (!Completed)
            {
                throw new MeetupNotCompletedException();
            }

            if (Cancelled)
            {
                throw new MeetupCancelledException();
            }

            var participant = Participants.FirstOrDefault((x) => x.ParticipantId == photographerId);

            if (participant == null)
            {
                throw new ParticipantNotFoundException();
            }

            ApplyEvent(new MeetupPhotoAdded
            {
                MeetupId = Id,
                PhotographerId = photographerId,
                PhotoPath = photo,
                CreatedDate = DateTime.Now
            });
        }

        public void Cancel(OrganizerId organizerId)
        {
            if (Version == -1)
            {
                throw new MeetupNotFoundException();
            }

            if (Completed)
            {
                throw new MeetupCompletedException();
            }

            if (organizerId != OrganizerId)
            {
                throw new MeetupDomainException("You are not organizer this meetup");
            }

            ApplyEvent(new MeetupCancelled
            {
                MeetupId = Id,
                CreatedDate = DateTime.Now
            });
        }

        public void AddComment(CommentatorId commentatorId, Comment comment)
        {
            if (Version == -1)
            {
                throw new MeetupNotFoundException();
            }

            if (!Completed)
            {
                throw new MeetupNotCompletedException();
            }

            var participant = Participants.FirstOrDefault((x) => x.ParticipantId == commentatorId);

            if (participant == null)
            {
                throw new ParticipantNotFoundException();
            }

            ApplyEvent(new MeetupCommentAdded
            {
                MeetupId = Id,
                CommentatorId = commentatorId,
                Comment = comment,
                CreatedDate = DateTime.Now
            });
        }

        /*Event Handlers*/
        private void OnRegistered(MeetupRegistered @event)
        {
            Id = new MeetupId(@event.MeetupId);
            OrganizerId = new OrganizerId(@event.OrganizerId);
            Subject = new Subject(@event.Subject);
            When = new When(@event.When);
            Description = new Description(@event.Description);
            Location = new Location
            {
                Address = new Address(@event.Address)
            };
        }

        private void OnSubjectChanged(MeetupSubjectChanged @event)
        {
            Id = new MeetupId(@event.MeetupId);
            Subject = new Subject(@event.Subject);
        }

        private void OnWhenChanged(MeetupWhenChanged @event)
        {
            Id = new MeetupId(@event.MeetupId);
            When = new When(@event.When);
        }

        private void OnDescriptionChanged(MeetupDescriptionChanged @event)
        {
            Id = new MeetupId(@event.MeetupId);
            Description = new Description(@event.Description);
        }

        private void OnAddressChanged(MeetupAddressChanged @event)
        {
            Id = new MeetupId(@event.MeetupId);
            Location.Address = new Address(@event.Address);
        }

        private void OnLocationAdded(MeetupLocationAdded @event)
        {
            Location.Latitude = new Latitude(@event.Latitude);
            Location.Longitude = new Longitude(@event.Longitude);
        }

        private void OnJoined(MeetupJoined @event)
        {
            _participants.Add(new MeetupParticipant
            {
                ParticipantId = new ParticipantId(@event.ParticipantId),
                CreatedDate = @event.CreatedDate
            });
        }

        private void OnCompleted(MeetupCompleted @event)
        {
            Completed = true;
        }

        private void OnPhotoAdded(MeetupPhotoAdded @event)
        {
            _photos.Add(new MeetupPhoto
            {
                PhotographerId = new PhotographerId(@event.PhotographerId),
                Photo = new Photo(@event.PhotoPath),
                CreatedDate = @event.CreatedDate
            });
        }

        private void OnCancelled(MeetupCancelled @event)
        {
            Cancelled = true;
        }

        private void OnCommentAdded(MeetupCommentAdded @event)
        {
            _comments.Add(new MeetupComment
            {
                CommentatorId = new CommentatorId(@event.CommentatorId),
                Comment = new Comment(@event.Comment),
                CreatedDate = @event.CreatedDate
            });
        }
    }
}
