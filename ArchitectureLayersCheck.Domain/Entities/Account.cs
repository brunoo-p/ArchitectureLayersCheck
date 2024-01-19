
namespace ArchitectureLayersCheck.Domain.Entities
{
    internal class Account : Entity
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; private set; }

        public Account( string firstName, string lastName, string email  ) : base()
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public Account setEmail(string newEmail)
        {
            Email = newEmail;
            return this;
        }
    }
}
