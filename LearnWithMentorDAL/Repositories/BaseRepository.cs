﻿using System.Collections.Generic;
using LearnWithMentorDAL.Entities;
using System.Data.Entity;

namespace LearnWithMentorDAL.Repositories
{
    public class BaseRepository<T>: IRepository<T> where T : class
    {
        protected readonly LearnWithMentor_DBEntities context;
        public BaseRepository(LearnWithMentor_DBEntities _context)
        {
            context = _context;
        }
        public IEnumerable<T> GetAll()
        {
            return context.Set<T>();
        }
        public void Add(T item)
        {
            context.Set<T>().Add(item);
        }
        public void Update(T item)
        {
            context.Entry(item).State = EntityState.Modified;
        }
        public void Remove(T item)
        {
            context.Set<T>().Remove(item);
        }
    }
}
