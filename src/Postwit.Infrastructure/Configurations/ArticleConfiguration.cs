using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.PostgresTypes;
using Postwit.Domain;

namespace Postwit.Infrastructure.Configurations;

internal sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasIndex(a => a.Title).IsUnique();
        builder.HasIndex(a => a.Slug).IsUnique();

        builder.Property(a => a.Title)
            .HasMaxLength(Article.MaxTitleLength)
            .IsRequired();

        builder.Property(a => a.Content).HasMaxLength(Article.MaxContentLength);

        builder.Property(a => a.AuthorId).IsRequired();

        builder.Property(x=>x.Status).HasConversion<string>();

        var valueComparer = new ValueComparer<IEnumerable<Guid>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToHashSet());

        builder.Property(x=> x.TagsIds)
            .HasColumnType("jsonb")
            .HasConversion(
                array => JsonSerializer.Serialize(array, JsonSerializerOptions.Default), 
                json => JsonSerializer.Deserialize<List<Guid>>(json, JsonSerializerOptions.Default),
                valueComparer);

        builder.Property(x => x.FilesIds)
           .HasColumnType("jsonb")
           .HasConversion(
               array => JsonSerializer.Serialize(array, JsonSerializerOptions.Default),
               json => JsonSerializer.Deserialize<List<Guid>>(json, JsonSerializerOptions.Default),
               valueComparer);
    }
}


