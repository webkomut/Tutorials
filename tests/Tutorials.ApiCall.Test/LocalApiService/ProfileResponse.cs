using System;

namespace Tutorials.ApiCall.Test.LocalApiService
{
    public class ProfileResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public string Language { get; set; }
        public bool InActive { get; set; }
    }

    public class EmailResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public bool IsConfirm { get; set; }
        public bool IsDefault { get; set; }
    }
}