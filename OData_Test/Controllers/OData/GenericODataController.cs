﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OData_Test.Data;
using OData_Test.Extensions;
using OData_Test.Services;

namespace OData_Test.Controllers.OData
{
    public abstract class GenericODataController<TEntity, TKey> : ODataController, IDisposable
        where TEntity : class
    {
        #region Non-Public Properties

        protected IGenericDataService<TEntity> Service { get; private set; }

        //protected ILogger Logger { get; private set; }

        private IRepositoryConnection<TEntity> disposableConnection = null;

        #endregion Non-Public Properties

        #region Constructors

        public GenericODataController(IGenericDataService<TEntity> service)
        {
            Service = service;
            //var loggerFactory = EngineContext.Current.Resolve<ILoggerFactory>();
            //Logger = loggerFactory.CreateLogger(GetType());
        }

        public GenericODataController(IRepository<TEntity> repository)
        {
            Service = new GenericDataService<TEntity>(repository);
            //var loggerFactory = EngineContext.Current.Resolve<ILoggerFactory>();
            //Logger = loggerFactory.CreateLogger(GetType());
        }

        #endregion Constructors

        #region Public Methods

        // NOTE: Change due to: https://github.com/OData/WebApi/issues/1235
        // GET: odata/<Entity>
        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual async Task<IEnumerable<TEntity>> Get(ODataQueryOptions<TEntity> options)
        {
            options.Validate(new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.All
            });

            var connection = GetDisposableConnection();
            var query = connection.Query();
            query = ApplyMandatoryFilter(query);
            var results = options.ApplyTo(query);
            return (results as IQueryable<TEntity>).ToHashSet();
        }

        // GET: odata/<Entity>(5)
        [EnableQuery]
        public virtual async Task<SingleResult<TEntity>> Get([FromODataUri] TKey key)
        {
            var entity = await Service.FindOneAsync(key);

            // TODO: CheckPermission(ReadPermission) is getting done twice.. once above, and once in CanViewEntity(). Unnecessary... see if this can be modified
            if (!CanViewEntity(entity))
            {
                return SingleResult.Create(Enumerable.Empty<TEntity>().AsQueryable());
            }

            return SingleResult.Create(new[] { entity }.AsQueryable());
        }

        // PUT: odata/<Entity>(5)
        public virtual async Task<IActionResult> Put([FromODataUri] TKey key, TEntity entity)
        {
            if (!CanModifyEntity(entity))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!key.Equals(GetId(entity)))
            {
                return BadRequest();
            }

            try
            {
                OnBeforeSave(entity);
                await Service.UpdateAsync(entity);
                OnAfterSave(entity);
            }
            catch (DbUpdateConcurrencyException x)
            {
                //Logger.LogError(new EventId(), x, x.Message);

                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        // POST: odata/<Entity>
        public virtual async Task<IActionResult> Post(TEntity entity)
        {
            if (!CanModifyEntity(entity))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SetNewId(entity);

            OnBeforeSave(entity);
            await Service.InsertAsync(entity);
            OnAfterSave(entity);

            return Created(entity);
        }

        // PATCH: odata/<Entity>(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public virtual async Task<IActionResult> Patch([FromODataUri] TKey key, Delta<TEntity> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TEntity entity = await Service.FindOneAsync(key);

            if (entity == null)
            {
                return NotFound();
            }

            if (!CanModifyEntity(entity))
            {
                return Unauthorized();
            }

            patch.Patch(entity);

            try
            {
                await Service.UpdateAsync(entity);
                //db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException x)
            {
                //Logger.LogError(new EventId(), x, x.Message);

                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        // DELETE: odata/<Entity>(5)
        public virtual async Task<IActionResult> Delete([FromODataUri] TKey key)
        {
            TEntity entity = await Service.FindOneAsync(key);

            if (entity == null)
            {
                return NotFound();
            }

            if (!CanModifyEntity(entity))
            {
                return Unauthorized();
            }

            await Service.DeleteAsync(entity);

            return NoContent();
        }

        #endregion Public Methods

        #region Non-Public Methods

        protected virtual bool EntityExists(TKey key)
        {
            return Service.FindOne(key) != null;
        }

        protected abstract TKey GetId(TEntity entity);

        /// <summary>
        /// Should only be necessary for Guid types
        /// </summary>
        /// <param name="entity"></param>
        protected abstract void SetNewId(TEntity entity);

        /// <summary>
        /// Add any filters which must be applied for the client. Mostly used for fields such as "TenantId", where you don't want
        /// the user viewing data for a different site (tenant)
        /// </summary>
        /// <param name="entity"></param>
        protected virtual IQueryable<TEntity> ApplyMandatoryFilter(IQueryable<TEntity> query)
        {
            // Do nothing, by default
            return query;
        }

        protected virtual bool CanViewEntity(TEntity entity)
        {
            if (entity == null)
            {
                return false;
            }

            return true;
        }

        protected virtual bool CanModifyEntity(TEntity entity)
        {
            if (entity == null)
            {
                return false;
            }

            return true;
        }

        protected virtual void OnBeforeSave(TEntity entity)
        {
        }

        protected virtual void OnAfterSave(TEntity entity)
        {
        }

        private IRepositoryConnection<TEntity> GetDisposableConnection()
        {
            if (disposableConnection == null)
            {
                disposableConnection = Service.OpenConnection();
            }
            return disposableConnection;
        }

        #endregion Non-Public Methods

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (disposableConnection != null)
                    {
                        disposableConnection.Dispose();
                        disposableConnection = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GenericODataController() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}