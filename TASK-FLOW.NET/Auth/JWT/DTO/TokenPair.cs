namespace TASK_FLOW.NET.Auth.JWT.DTO
{
    public class TokenPair
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required string RefreshTokenHasher { get; set; }
    }
}
