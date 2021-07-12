using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Infraestruture.Data;

namespace curso.api.Infraestruture.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CourseDbContext _context;

        public UserRepository(CourseDbContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _context.User.Add(user);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
