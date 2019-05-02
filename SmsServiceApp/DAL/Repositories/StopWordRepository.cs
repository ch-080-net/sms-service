using Model.Interfaces;
using System.Linq;
using WebApp.Data;
using WebApp.Models;

namespace DAL.Repositories
{
    public class StopWordRepository : BaseRepository<StopWord> , IStopWordRepository
    {
        public StopWordRepository(ApplicationDbContext context) : base(context)
        {
        }


        public void Create(string word)
        {
            StopWord stopWord = new StopWord() { Word = word };
            context.StopWords.Add(stopWord);
            context.SaveChanges();
        }

        public StopWord SearchByWord(string word)
        {
            StopWord phone = context.StopWords.FirstOrDefault(p => p.Word == word);
            return phone;
        }
    }
}
