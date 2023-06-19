using Microsoft.Extensions.Configuration;
using Nest;

namespace SearchJobApp.Persistence.ElasticSearch;

public class ElasticClientProvider
{
    private readonly IConfiguration _configuration;

    public ElasticClientProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ElasticClient CreateClient()
    {
        var connectionSettings =
            new ConnectionSettings(new Uri(_configuration["ElasticSearchConnectionSettings:ElasticSearchHost"]))
                .BasicAuthentication(_configuration["ElasticSearchConnectionSettings:ElasticSearchUsername"],
                    _configuration["ElasticSearchConnectionSettings:ElasticSearchPassword"])
                .SniffOnStartup(false)
                .SniffOnConnectionFault(false);

        return new ElasticClient(connectionSettings);
    }
}