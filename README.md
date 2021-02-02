## Hotchocolate bug repository

**Describe the bug**

The use of persisted queries in conjunction with batching produces the following error:

```
{"errors":[{"message":"Unexpected Execution Error","extensions":{"message":"Object reference not set to an instance of an object.","stackTrace":"   at HotChocolate.Execution.Batching.BatchExecutor.BatchExecutorEnumerable.ExecuteNextAsync(IReadOnlyQueryRequest request, CancellationToken cancellationToken)"}}]}
```

**To Reproduce**

Steps to reproduce the behavior:
1. Create HotChocolate-based GraphQL server with persisted queries (InMemory, File, Redis)
2. Issue a batched query:
```
curl -X POST \
  http://localhost:5000/graphql \
  -H 'Content-Type: application/json' \
  -H 'cache-control: no-cache' \
  -d '[{"id":"1"},{"id":"2"}]'
```
3. See error:
```
{"errors":[{"message":"Unexpected Execution Error","extensions":{"message":"Object reference not set to an instance of an object.","stackTrace":"   at HotChocolate.Execution.Batching.BatchExecutor.BatchExecutorEnumerable.ExecuteNextAsync(IReadOnlyQueryRequest request, CancellationToken cancellationToken)"}}]}
```

**Expected behavior**

Return the same result as batching without persisted queries.

**Desktop (please complete the following information):**
 - OS: Win 10
 - Version 11.0.9

**Additional context**

The following repository has been created to reproduce the bug:

[https://github.com/beasync/hotchocolate-persisted-queries-batching](https://github.com/beasync/hotchocolate-persisted-queries-batching)

**Repro Steps**

Run test:

```
dotnet test
```

Expected Output:

```
Passed!  - Failed:     0, Passed:     3, Skipped:     0, Total:     3, Duration: 760 ms - Sample.Test.dll (net5.0)
```

Actual Output:

```
[xUnit.net 00:00:02.43]     Sample.Test.Scenarios.hotchocolate_should_support_persisted_queries_with_batching [FAIL]
  Failed Sample.Test.Scenarios.hotchocolate_should_support_persisted_queries_with_batching [18 ms]
  Error Message:
   Assert.Equal() Failure
                                 ↓ (pos 69)
Expected: ···-8\nContent-Length: 27\n\n{\"data\":{\"spanish\":\"Hola\"}}\n···
Actual:   ···-8\nContent-Length: 53\n\n{\"errors\":[{\"message\":\"Unexpec···
                                 ↑ (pos 69)

Failed!  - Failed:     1, Passed:     2, Skipped:     0, Total:     3, Duration: 750 ms - Sample.Test.dll (net5.0)
```