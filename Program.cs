using Models;
using System.Net.Http.Json;
using System.Linq;

public partial class Program
{
    static readonly HttpClient client = new HttpClient();

    static async Task Main()
    {
        if (int.TryParse(Console.ReadLine(), out int limit))
        {
            if (limit >= 0)
            {
                Console.WriteLine(string.Join(Environment.NewLine, await topArticles(limit)));
            }
            else
            {
                Console.WriteLine("Input must be a positive integer");
            }
        }
        else
        {
            Console.WriteLine("Could not parse input as integer");
        }
    }

    static async Task<string[]> topArticles(int limit)
    {
        int page = 1;
        int? totalPages = null;
        List<Article> articles = new List<Article>();

        do
        {
            var currentPage = await GetArticles(page);

            if (currentPage?.total_pages == null)
                return new string[0];

            // We only set totalPages once, hence the use of ??=
            totalPages ??= currentPage?.total_pages;

            articles.AddRange(
                currentPage?.data?
                    .Where(x => x.title != null || x.story_title != null) // Ignore if article.title and article.story_title are both null
                    .Select(x => new Article(x.title ?? x.story_title, x.num_comments ?? 0)) // Try "title", but default to "story_title" if "title" is not available
                ?? Enumerable.Empty<Article>()
            );

            page++;
        } while(totalPages != null && page <= totalPages);

        return 
            articles
                .OrderByDescending(x => x.CommentCount) // Order by decreasing comment count first
                    .ThenByDescending(x => x.ArticleName) // For same comment count, order by decreasing alphabetically
                .Select(x => x.ArticleName)
                .Take(limit)
                .ToArray();
    }

    static async Task<GetArticlesResponseDTO> GetArticles(int page = 1)
    {
        return await client.GetFromJsonAsync<GetArticlesResponseDTO>($"https://jsonmock.hackerrank.com/api/articles?page={page}");
    }
}

