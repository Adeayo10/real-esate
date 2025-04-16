
namespace server_real_estate.Model;

using System.ComponentModel.DataAnnotations.Schema;
using server_real_estate.Database;

public class RefreshToken
{
    public Guid Id { get; set; }
    
    [ForeignKey(nameof(User))]
    public string UserId { get; set; }
    
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
    public string Token { get; set; }
    

}


public class RefreshTokenRequest
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
  
}