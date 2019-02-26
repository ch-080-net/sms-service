using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCustomerApp.Data;
using WebCustomerApp.Models;

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
