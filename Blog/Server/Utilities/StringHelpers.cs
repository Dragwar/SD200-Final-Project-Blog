using EFCore.NamingConventions.Internal;
using System.Globalization;

namespace Blog.Server.Utilities;
public static class StringHelpers
{
	private static readonly SnakeCaseNameRewriter _snakeCaseNameRewriter = new(CultureInfo.InvariantCulture);
	public static string ConvertToSnakeCase(string value) => _snakeCaseNameRewriter.RewriteName(value);
}
