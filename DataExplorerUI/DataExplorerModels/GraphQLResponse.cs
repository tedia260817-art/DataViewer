namespace DataExplorerModels;

public class GraphQLResponse
{
    public GraphQLData? Data { get; set; }
}

public class GraphQLData
{
    public PostCollection? Posts { get; set; }
}

public class PostCollection
{
    public List<Post>? Data { get; set; }
}