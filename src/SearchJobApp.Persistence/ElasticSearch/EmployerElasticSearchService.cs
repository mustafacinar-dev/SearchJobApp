using Nest;
using SearchJobApp.Application.Interfaces.Services;
using SearchJobApp.Domain.Entities;

namespace SearchJobApp.Persistence.ElasticSearch;

public class EmployerElasticSearchService : IEmployerElasticSearchService
{
    private readonly ElasticClient _elasticClient;
    private const string IndexName = "employers";

    public EmployerElasticSearchService(ElasticClientProvider elasticClientProvider)
    {
        _elasticClient = elasticClientProvider.CreateClient();
    }

    public async Task InsertAsync(Employer employer)
    {
        var indexExist = await CheckIndex(IndexName);
        if (indexExist)
        {
            await _elasticClient.CreateAsync(employer, i => i.Index(IndexName));
        }
    }

    public async Task UpdateAsync(Guid id, Employer employer)
    {
        await _elasticClient.UpdateAsync<Employer>(id, e => e.Index(IndexName).Doc(employer));
    }

    public async Task<Employer> GetAsync(Guid id)
    {
        var response = await _elasticClient.GetAsync<Employer>(id, i => i.Index(IndexName));
        if (!response.IsValid)
        {
            throw response.OriginalException;
        }

        return response.Source;
    }

    public async Task<IEnumerable<Employer>> GetAllAsync()
    {
        var response = await _elasticClient.SearchAsync<Employer>(e => e.Index(IndexName));

        return response.Documents.ToList();
    }

    public async Task RemoveAsync(Guid id)
    {
        await _elasticClient.DeleteAsync<Employer>(id, p => p.Index(IndexName));
    }

    private async Task<bool> CheckIndex(string indexName)
    {
        var existsResponse = await _elasticClient.Indices.ExistsAsync(indexName);
        if (existsResponse.Exists) return true;

        var response = await _elasticClient.Indices.CreateAsync(indexName,
            ci => ci
                .Index(indexName)
                .Map<Employer>(m => m.AutoMap())
                .Settings(s => s.NumberOfShards(1).NumberOfReplicas(1))
        );

        return response.IsValid;
    }
}