
using TASK_FLOW.NET.Auth.DTO;
using TASK_FLOW.NET.Auth.JWT.DTO;

namespace TASK_FLOW_TESTING.Auth.Mock
{
    public static class AuthMock
    {
        public static AuthPropsDTO AuthDTOMock => new AuthPropsDTO
        {
            Username = "DMarzz",
            Password = "Pr@motheus98"
        };
        public static TokenPair TokenMock => new TokenPair
        {
            AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwicm9sIjoiMSIsIm5iZiI6MTczNTM5MDkzMiwiZXhwIjoxNzM1NTYzNzMyLCJpYXQiOjE3MzUzOTA5MzJ9.fxCAmD20OHRbD28D5PhuVkLkidcySTblRdT0geFQfO4",
            RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwicm9sIjoiMSIsIm5iZiI6MTczNTM5MDkzMiwiZXhwIjoxNzM1ODIyOTMyLCJpYXQiOjE3MzUzOTA5MzJ9.7WoceqK9cqsQvs6KEAymuY8nyU4ElAV_bUBFU8WEacs",

            RefreshTokenHasher = "$2a$11$4oaZ9eM55kz2WkDnazw7s.Uh66Pu/raUH0tue3qqRPd1V6NEJcf/."
        };
    }
}
