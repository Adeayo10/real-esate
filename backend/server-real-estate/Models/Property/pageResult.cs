namespace server_real_estate.Model;

public class pageResult<T>
{
    public List<T> items { get; set; }
    public int totalItems { get; set; }
}
