using Microsoft.AspNet.Identity.EntityFramework;
using onlinePlatby.Models;

namespace onlinePlatby.DbContexts
{
    public class IdentityDb : IdentityDbContext<ApplicationUser>
    {
        public IdentityDb()
            : base("DefaultConnection", throwIfV1Schema: false) // Default Connection will be changed
        {
        }

        public static IdentityDb Create()
        {
            return new IdentityDb();
        }
    }
}
