using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.SharedKernel.Domain;

namespace SocialMedia.SharedKernel.Data;

public abstract class BaseDomainModelConfiguration<T, TKey> : IEntityTypeConfiguration<T>
	where T : DomainModel<TKey>
	where TKey : struct
{
	public virtual void Configure(EntityTypeBuilder<T> builder)
	{
		builder.HasKey(_ => _.Id);

		ConfigureDomainModel(builder);
	}

	protected abstract void ConfigureDomainModel(EntityTypeBuilder<T> builder);
}