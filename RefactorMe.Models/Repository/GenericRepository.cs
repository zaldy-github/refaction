using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RefactorMe.Models.Repository
{
    public class GenericRepository<T> where T : class
    {
        // private member vars
        internal RefactorMeDbEntities Entities;
        internal DbSet<T> DbSet;

        // public constructor
        public GenericRepository(RefactorMeDbEntities entities)
        {
            this.Entities = entities;
            this.DbSet = entities.Set<T>();
        }

        #region Public member methods
        /// <summary>
        /// Generic method to get entity using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById(object id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// Generic method to insert an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(T entity)
        {
            DbSet.Add(entity);
        }

        /// <summary>
        /// Generic method to delete entity using id
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(object id)
        {
            T entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        /// <summary>
        /// Generic method to delete entity
        /// </summary>
        /// <param name="entityToDelete"></param>
        public virtual void Delete(T entityToDelete)
        {
            if (Entities.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// Generic method to update entity
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void Update(T entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Entities.Entry(entityToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        /// Generic method to get entities using criteria specified
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll(Func<T, bool> where)
        {
            return DbSet.Where(where).ToList();
        }

        /// <summary>
        /// Generic method to get entities using criteria specified
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T Get(Func<T, bool> where)
        {
            return DbSet.Where(where).FirstOrDefault<T>();
        }

        /// <summary>
        /// Generic method to delete entities using criteria specified
        /// </summary>
        /// <param name="where"></param>
        public void Delete(Func<T, bool> where)
        {
            IQueryable<T> objects = DbSet.Where<T>(where).AsQueryable();
            foreach (T obj in objects)
            {
                DbSet.Remove(obj);
            }
        }

        /// <summary>
        /// Generic method to get all entities
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }
        #endregion
    }
}
