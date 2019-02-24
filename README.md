## Event Sourcing - CQRS

### Used technologies
Docker
EventStore
RavenDB
ElasticSearch

> docker-compose up

#### Aggregate Roots
Meetup

#### Domain Events
MeetupRegistered
MeetupSubjectChanged
MeetupWhenChanged
MeetupDescriptionChanged
MeetupAddressChanged
MeetupJoined
MeetupPhotoAdded
MeetupCommentAdded
MeetupCompleted
MeetupCancelled

#### Domain Services
MeetupPolicy

#### Projections
QueryProjection
AutoCompleteProjection
