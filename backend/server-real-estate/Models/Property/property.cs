namespace server_real_estate.Model;
public class Property
{
    public int Id { get; set; }
    public required string Name { get; set;}
    public required string Address { get; set; }
    public decimal Price { get; set; }
    public required string HouseType{get;set;}
    public required string Mode{get;set;}
}