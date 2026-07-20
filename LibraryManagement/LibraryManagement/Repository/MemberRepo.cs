using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;

namespace LibraryManagement.Repository
{
    public class MemberRepo:Repository<Member>,IMemberRepo
    {
        private readonly ApplicationDbContext _db;

        public MemberRepo(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Member member)
        {
            _db.Members.Update(member);
        }
    }
}
