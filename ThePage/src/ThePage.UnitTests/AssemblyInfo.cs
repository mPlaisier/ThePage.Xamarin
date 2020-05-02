using Xunit;

// Tests running in parallel cause exceptions when creating singletons
[assembly: CollectionBehavior(DisableTestParallelization = true)]