using LibraryManagement.Models;

namespace LibraryManagement.Repository.IRepository
{
    public interface IMemberRepo:IRepository<Member>
    {
        void Update(Member member);
    }
}
