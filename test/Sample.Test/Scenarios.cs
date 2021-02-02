using FluentAssertions;
using HotChocolate.Execution;
using HotChocolate.Language;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Snapshooter.Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sample.Test
{
    [Collection(nameof(ServerFixtureCollection))]
    public class Scenarios
    {
        private readonly ServerFixture Given;
        private readonly Dictionary<string, (string Id, DocumentNode Document)> Queries;

        private const string SPANISH_QUERY = nameof(SPANISH_QUERY);
        private const string ENGLISH_QUERY = nameof(ENGLISH_QUERY);

        public Scenarios(ServerFixture fixture)
        {
            Given = fixture;

            Queries = new Dictionary<string, (string Id, DocumentNode Document)>
            {
                {
                    SPANISH_QUERY,
                    (
                        Guid.NewGuid().ToString("N"),
                        Utf8GraphQLParser.Parse("{ spanish }")
                    )
                },
                {
                    ENGLISH_QUERY,
                    (
                        Guid.NewGuid().ToString("N"),
                        Utf8GraphQLParser.Parse("{ english }")
                    )
                }
            };
        }

        [Fact]
        public async Task hotchocolate_should_support_persisted_queries()
        {
            Queries.TryGetValue(SPANISH_QUERY, out var spanishQuery);

            await Given.WriteQueryAsync(
                queryId: spanishQuery.Id,
                query: new QueryDocument(spanishQuery.Document));

            var response = await Given
                .Server
                .CreateRequest("/graphql")
                .WithJsonBody(new { id = spanishQuery.Id })
                .PostAsync();

            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var body = await response.Content.ReadAsStringAsync();

            body.MatchSnapshot();
        }

        [Fact]
        public async Task hotchocolate_should_support_batching()
        {
            Queries.TryGetValue(SPANISH_QUERY, out var spanishQuery);
            Queries.TryGetValue(ENGLISH_QUERY, out var englishQuery);

            var response = await Given
                .Server
                .CreateRequest("/graphql")
                .WithJsonBody(new object[]
                {
                    new { query = spanishQuery.Document.ToString() },
                    new { query = englishQuery.Document.ToString() },
                })
                .PostAsync();

            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var body = await response.Content.ReadAsStringAsync();

            body.MatchSnapshot();
        }

        [Fact]
        public async Task hotchocolate_should_support_persisted_queries_with_batching()
        {
            Queries.TryGetValue(SPANISH_QUERY, out var spanishQuery);
            Queries.TryGetValue(ENGLISH_QUERY, out var englishQuery);

            await Given.WriteQueryAsync(
                queryId: spanishQuery.Id,
                query: new QueryDocument(spanishQuery.Document));

            await Given.WriteQueryAsync(
                queryId: englishQuery.Id,
                query: new QueryDocument(englishQuery.Document));

            var response = await Given
                .Server
                .CreateRequest("/graphql")
                .WithJsonBody(new object[]
                {
                    new { id = spanishQuery.Id },
                    new { id = englishQuery.Id },
                })

                /* Used to generate snapshot */

                //.WithJsonBody(new object[]
                //{
                //    new { query = spanishQuery.Document.ToString() },
                //    new { query = englishQuery.Document.ToString() },
                //})
                .PostAsync();

            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var body = await response.Content.ReadAsStringAsync();

            body.MatchSnapshot();
        }
    }
}
