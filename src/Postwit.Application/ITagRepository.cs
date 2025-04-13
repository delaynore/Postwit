using Microsoft.EntityFrameworkCore;
using Postwit.Domain;

namespace Postwit.Application;

public interface ITagRepository
{
    DbSet<Tag> Tags { get; }
}
