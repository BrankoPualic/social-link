using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Data;
using SocialLink.Messaging.Domain.Relational;

namespace SocialLink.Messaging;

internal interface IEFMessagingDatabaseContext : IEFDatabaseContext
{
	DbSet<ChatGroup> ChatGroups { get; }

	DbSet<ChatGroupUser> ChatGroupUsers { get; }
}
