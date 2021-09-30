using System;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;

namespace AbeckDev.Dlrgdd.RegistrationTool.Functions.Services
{
    public class GraphApiService
    {
        public string clientId { get; set; }

        public string clientSecret { get; set; }

        public string tenantId { get; set; }

        public GraphServiceClient graphClient { get; set; }


        public GraphApiService()
        {
            //Get Environment Variable Configuration
            this.clientId = Environment.GetEnvironmentVariable("GraphApiAppClientId");
            this.clientSecret = Environment.GetEnvironmentVariable("GraphApiAppClientSecret");
            this.tenantId = Environment.GetEnvironmentVariable("GraphApiAppTenantId");
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            };

            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret, options);

            graphClient = new GraphServiceClient(clientSecretCredential, scopes);
        }
    }
}
