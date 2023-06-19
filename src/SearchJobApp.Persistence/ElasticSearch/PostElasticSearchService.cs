using Nest;
using SearchJobApp.Application.Interfaces.Services;
using SearchJobApp.Application.Queries;
using SearchJobApp.Domain.Entities;

namespace SearchJobApp.Persistence.ElasticSearch;

public class PostElasticSearchService : IPostElasticSearchService
{
    private readonly ElasticClient _elasticClient;
    private const string IndexName = "posts";

    public PostElasticSearchService(ElasticClientProvider elasticClientProvider)
    {
        _elasticClient = elasticClientProvider.CreateClient();
    }

    public async Task InsertAsync(Post post)
    {
        var indexExist = await CheckIndex(IndexName);
        if (indexExist)
        {
            await _elasticClient.CreateAsync(post, i => i.Index(IndexName));
        }
    }

    public async Task UpdateAsync(Guid id, Post post)
    {
        await _elasticClient.UpdateAsync<Post>(id, p => p.Index(IndexName).Doc(post));
    }

    public async Task<Post> GetAsync(Guid id)
    {
        var response = await _elasticClient.GetAsync<Post>(id, i => i.Index(IndexName));
        if (!response.IsValid)
        {
            throw response.OriginalException;
        }

        return response.Source;
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        var searchResponse = await _elasticClient.SearchAsync<Post>(s => s
            .Query(q => GetDateRangeQuery(DateTime.Now))
            .Index(IndexName));

        return searchResponse.Documents.ToList();
    }

    public async Task<IEnumerable<Post>> GetPostsByEmployerId(string employerId)
    {
        var searchResponse = await _elasticClient.SearchAsync<Post>(p => p
            .Query(q => q
                .Term(t => t
                    .Field(f => f.EmployerId)
                    .Value(employerId)) && GetDateRangeQuery(DateTime.Now))
            .Index(IndexName));

        if (!searchResponse.IsValid)
        {
            throw searchResponse.OriginalException;
        }

        return searchResponse.Documents.ToList();
    }

    public async Task<IEnumerable<Post>> SearchAsync(GetSearchQuery searchQuery)
    {
        var searchQueryContainer = GetSearchQueryContainer(searchQuery);

        var searchResponse =
            await _elasticClient.SearchAsync<Post>(s => s
                .Query(q => searchQueryContainer)
                .Index(IndexName));

        if (!searchResponse.IsValid)
        {
            throw searchResponse.OriginalException;
        }

        return searchResponse.Documents.ToList();
    }

    public async Task RemoveAsync(Guid id)
    {
        await _elasticClient.DeleteAsync<Post>(id, p => p.Index(IndexName));
    }

    private DateRangeQuery GetDateRangeQuery(DateTime endDate) => new DateRangeQuery()
    {
        Field = Infer.Field<Post>(p => p.EndDate),
        GreaterThanOrEqualTo = endDate
    };

    private MultiMatchQuery GetKeywordMatchPhrasePrefixQuery(string keyword) => new MultiMatchQuery()
    {
        Fields = Infer.Field<Post>(p => p.Title)
            .And<Post>(p => p.Message)
            .And<Post>(p => p.EmployerTitle),
        Query = keyword,
        Type = TextQueryType.PhrasePrefix
    };

    private BoolQuery? GetTypeBoolQuery(string type, Field field)
    {
        if (string.IsNullOrWhiteSpace(type)) return null;

        var types = type.Trim(',').Split(',').ToList();

        var queryContainers = types
            .Select(t =>
                new QueryContainer(
                    new TermQuery
                    {
                        Field = field,
                        Value = t
                    }))
            .ToList();

        return new BoolQuery { Should = queryContainers };
    }

    private QueryContainer? GetSearchQueryContainer(GetSearchQuery searchQuery)
    {
        var dateRangeQuery = GetDateRangeQuery(DateTime.Now);

        var keywordMatchQuery = string.IsNullOrWhiteSpace(searchQuery.SearchText)
            ? null
            : GetKeywordMatchPhrasePrefixQuery(searchQuery.SearchText);

        var workTypeTermQuery = string.IsNullOrWhiteSpace(searchQuery.WorkType)
            ? null
            : GetTypeBoolQuery(searchQuery.WorkType, Infer.Field<Post>(p => p.WorkType));

        var positionLevelTypeTermQuery = string.IsNullOrWhiteSpace(searchQuery.PositionLevel)
            ? null
            : GetTypeBoolQuery(searchQuery.PositionLevel, Infer.Field<Post>(p => p.PositionLevel));

        return new QueryContainer(
            dateRangeQuery &&
            keywordMatchQuery &&
            workTypeTermQuery &&
            positionLevelTypeTermQuery);
    }

    private async Task<bool> CheckIndex(string indexName)
    {
        var existsResponse = await _elasticClient.Indices.ExistsAsync(indexName);
        if (existsResponse.Exists) return true;

        var response = await _elasticClient.Indices.CreateAsync(indexName,
            ci => ci
                .Index(indexName)
                .Map<Post>(m => m.AutoMap())
                .Settings(s => s.NumberOfShards(1).NumberOfReplicas(1))
        );

        return response.IsValid;
    }
}