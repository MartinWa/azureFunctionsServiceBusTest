using System.Collections.Generic;
using Test.Enums;

namespace Test.Dto
{
    public class UserRightsDto
    {
        public int UserId { get; set; }
        public int PortalId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long UserUgam { get; set; }
        public int CustomerId { get; set; }
        public CustomerSubscription Subscription { get; set; }
        public IEnumerable<ZeroRole> Roles { get; set; }
        public IEnumerable<int> UserGroupIds { get; set; }
    }
}