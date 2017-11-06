using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.Entity.Validation;
using RefactorMe.Models.Repository;

namespace RefactorMe.Models.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        // private member vars
        private RefactorMeDbEntities _entities = null;
        private GenericRepository<Product> _productRepository;
        private GenericRepository<ProductOption> _productOptionRepository;

        // public constructor
        public UnitOfWork()
        {
            _entities = new RefactorMeDbEntities();
        }

        #region Repository properties
        /// <summary>
        /// Get/Set property for the product repository
        /// </summary>
        public GenericRepository<Product> ProductRepository
        {
            get
            {
                if (this._productRepository == null)
                {
                    this._productRepository = new GenericRepository<Product>(_entities);
                }
                return _productRepository;
            }
        }

        /// <summary>
        /// Get/Set property for the product option repository
        /// </summary>
        public GenericRepository<ProductOption> ProductOptionRepository
        {
            get
            {
                if (this._productOptionRepository == null)
                {
                    this._productOptionRepository = new GenericRepository<ProductOption>(_entities);
                }
                return _productOptionRepository;
            }
        }
        #endregion

        /// <summary>
        /// Save method
        /// </summary>
        public void Save()
        {
            try
            {
                _entities.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var errors = new List<string>();
                foreach (var evErrors in e.EntityValidationErrors)
                {
                    errors.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, evErrors.Entry.Entity.GetType().Name, evErrors.Entry.State));
                    foreach (var vErrors in evErrors.ValidationErrors)
                    {
                        errors.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", vErrors.PropertyName, vErrors.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\Temp\errors.txt", errors);
                throw e;
            }
        }

        #region IDisposable implementation
        // Free up resources, i.e. connections and objects
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _entities.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
