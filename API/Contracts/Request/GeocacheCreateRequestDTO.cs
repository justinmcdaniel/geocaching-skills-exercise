namespace API.Contracts.Request
{
    public record GeocacheCreateRequestDTO
    {
        public string Name { get; init; }
        public decimal Latitude { get; init; }
        public decimal Longitude { get; init; }
    }
}
