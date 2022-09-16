namespace CardStorageService.Domain;

public class ConnectionString
{
    public string DataSource { get; set; } = null!;
    public string InitialCatalog { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string MultipleActiveResultSets { get; set; }
    public string App { get; set; } = null!;

    public override string ToString()
    {
        return $"data source={DataSource};initial catalog={InitialCatalog};User Id={UserId};Password={Password};MultipleActiveResultSets={MultipleActiveResultSets};App={App}";
    }
}