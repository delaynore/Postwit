using System.Text;
using Postwit.Application;

namespace Postwit.Infrastructure.Services;

public sealed class SlugGenerator : ISlugGenerator
{
    private static readonly char[] ReplaceableChars = " `~!@#$%^&*()+=_\\|]}[{;:'\"/.>,<?-\n\t".ToCharArray();
    
    public string GenerateSlug(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }
        
        var titleSplit = input.AsSpan().SplitAny(ReplaceableChars);
        var slug = new StringBuilder(input.Length);
        foreach (var range in titleSplit)
        {
            if (string.IsNullOrWhiteSpace(input[range]))
            {
                continue;
            }
            slug.Append(input[range]);
            slug.Append('-');
        }

        if (slug.Length != 0)
        {
            slug.Remove(slug.Length - 1, 1);    
        }
        
        return slug.ToString();
    }
}
