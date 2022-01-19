using Blog.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Server.Utilities;
public static class EntityFrameworkHelpers
{
	public static void ToTableSnakeCase<T>(this EntityTypeBuilder<T> builder)
		where T : class
	{
		var tableName = builder.Metadata.GetTableName() ?? builder.Metadata.GetDefaultTableName() ?? builder.Metadata.ShortName();
		var schema = builder.Metadata.GetSchema() ?? builder.Metadata.GetDefaultSchema() ?? BlogContext.DEFAULT_SCHEMA;
		builder.ToTableSnakeCase(tableName, schema);
	}

	public static void ToTableSnakeCaseDefaultTableName<T>(this EntityTypeBuilder<T> builder)
		where T : class
	{
		var tableName = builder.Metadata.GetDefaultTableName() ?? builder.Metadata.ShortName();
		var schema = builder.Metadata.GetSchema() ?? builder.Metadata.GetDefaultSchema() ?? BlogContext.DEFAULT_SCHEMA;
		builder.ToTableSnakeCase(tableName, schema);
	}

	public static void ToTableSnakeCase<T>(this EntityTypeBuilder<T> builder, string tableName)
		where T : class
	{
		if (string.IsNullOrWhiteSpace(tableName))
		{
			throw new ArgumentException($"'{nameof(tableName)}' cannot be null or whitespace.", nameof(tableName));
		}

		builder.ToTable(tb =>
		{
			tb.Metadata.SetTableName(StringHelpers.ConvertToSnakeCase(tableName));
		});
	}

	public static void ToTableSnakeCase<T>(this EntityTypeBuilder<T> builder, string tableName, string schema)
		where T : class
	{
		if (string.IsNullOrWhiteSpace(tableName))
		{
			throw new ArgumentException($"'{nameof(tableName)}' cannot be null or whitespace.", nameof(tableName));
		}

		if (string.IsNullOrWhiteSpace(schema))
		{
			throw new ArgumentException($"'{nameof(schema)}' cannot be null or whitespace.", nameof(schema));
		}

		builder.ToTable(tb =>
		{
			tb.Metadata.SetTableName(StringHelpers.ConvertToSnakeCase(tableName));
			tb.Metadata.SetSchema(StringHelpers.ConvertToSnakeCase(schema));
		});
	}
}
