﻿//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

using Hexa.Core.Domain.Specification;
using Hexa.Core.Logging;

namespace Hexa.Core.Domain
{
    /// <summary>
    /// Default base class for repostories. This generic repository 
    /// is a default implementation of <see cref="Hexa.Core.Domain.IRepository{TEntity}"/>
    /// and your specific repositories can inherit from this base class so automatically will get default implementation.
    /// IMPORTANT: Using this Base Repository class IS NOT mandatory. It is just a useful base class:
    /// You could also decide that you do not want to use this base Repository class, because sometimes you don't want a 
    /// specific Repository getting all these features and it might be wrong for a specific Repository. 
    /// For instance, you could want just read-only data methods for your Repository, etc. 
    /// in that case, just simply do not use this base class on your Repository.
    /// </summary>
    /// <typeparam name="TEntity">Type of elements in repostory</typeparam>
    public class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {

        #region Members

        private ILogger _logger = null;
        protected ILogger Logger { get { return _logger; } }

        private IQueryableUnitOfWork _context;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for GenericRepository
        /// </summary>
        /// <param name="traceManager">Trace Manager dependency</param>
        /// <param name="context">A context for this repository</param>
        public BaseRepository(ILoggerFactory loggerFactory)
        {
            Guard.IsNotNull(loggerFactory, "loggerFactory");

            if (UnitOfWorkContext.Current == null)
                UnitOfWorkContext.Start();

            var context = UnitOfWorkContext.Current;

            //check preconditions
            if (context == (IQueryableUnitOfWork)null)
                throw new ArgumentNullException("context", "No context in scope.");

            //set internal values
            _context = context as IQueryableUnitOfWork;
            _logger = loggerFactory.Create(this.GetType());

            _logger.Debug(
                string.Format(CultureInfo.InvariantCulture,
                            "",
                             typeof(TEntity).Name));
        }

        #endregion

        #region IRepository<TEntity> Members

        /// <summary>
        /// Return a context in this repository
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context as IUnitOfWork;
            }
        }

        /// <summary>
        /// Return a context in this repository
        /// </summary>
        protected IQueryableUnitOfWork Context
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// <see cref="Hexa.Core.Domain.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="item"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        public virtual void Add(TEntity item)
        {
            //check item
            if (item == (TEntity)null)
                throw new ArgumentNullException("item");

            //add object to IObjectSet for this type
            (_context.CreateSet<TEntity>()).AddObject(item);

            _logger.Debug(
                string.Format(CultureInfo.InvariantCulture,
                             "Added a {0} entity",
                              typeof(TEntity).Name));
        }

        /// <summary>
        /// <see cref="Hexa.Core.Domain.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="item"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        public virtual void Remove(TEntity item)
        {
            //check item
            if (item == (TEntity)null)
                throw new ArgumentNullException("item");


            IEntitySet<TEntity> objectSet = (_context.CreateSet<TEntity>());

            //Attach object to context and delete this
            // this is valid only if T is a type in model
            objectSet.Attach(item);

            //delete object to IObjectSet for this type
            objectSet.DeleteObject(item);

            _logger.Debug(
               string.Format(CultureInfo.InvariantCulture,
                            "Deleted a {0} entity",
                            typeof(TEntity).Name));
        }

        /// <summary>
        /// <see cref="Hexa.Core.Domain.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="item"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        public void Attach(TEntity item)
        {
            if (item == (TEntity)null)
                throw new ArgumentNullException("item");

            (_context.CreateSet<TEntity>()).Attach(item);

            _logger.Debug(
               string.Format(CultureInfo.InvariantCulture,
                            "Attached {0} to context",
                            typeof(TEntity).Name));
        }

        public virtual void Modify(TEntity item)
        {
            //check arguments
            if (item == (TEntity)null)
                throw new ArgumentNullException("item");

            ////Set modifed state if change tracker is enabled
            //if (item.ChangeTracker != null)
            //    item.MarkAsModified();

            //apply changes for item object
            _context.SetChanges(item);

            _logger.Info(
                           string.Format(CultureInfo.InvariantCulture,
                                        "Applied changes to: {0}",
                                        typeof(TEntity).Name));
        }

        /// <summary>
        /// <see cref="Hexa.Core.Domain.IRepository{TEntity}"/>
        /// </summary>
        /// <returns><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></returns>
        public IEnumerable<TEntity> GetAll()
        {

            _logger.Debug(
                           string.Format(CultureInfo.InvariantCulture,
                                        "Getting all {0}",
                                        typeof(TEntity).Name));

            //Create IObjectSet and perform query 
            return (_context.CreateSet<TEntity>()).AsEnumerable<TEntity>();
        }

        /// <summary>
        /// <see cref="Hexa.Core.Domain.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="specification"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        /// <returns><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></returns>
        public IEnumerable<TEntity> GetBySpec(ISpecification<TEntity> specification)
        {
            if (specification == (ISpecification<TEntity>)null)
                throw new ArgumentNullException("specification");

            _logger.Debug(
                           string.Format(CultureInfo.InvariantCulture,
                                        "Getting {0} by specification",
                                        typeof(TEntity).Name));

            return (_context.CreateSet<TEntity>()
                            .Where(specification.SatisfiedBy())
                            .AsEnumerable<TEntity>());

        }

        /// <summary>
        /// <see cref="Hexa.Core.Domain.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="filter"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        /// <returns><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></returns>b
        public IEnumerable<TEntity> GetFilteredElements(Expression<Func<TEntity, bool>> filter)
        {
            //checking query arguments
            if (filter == (Expression<Func<TEntity, bool>>)null)
                throw new ArgumentNullException("filter");

            _logger.Debug(
                           string.Format(CultureInfo.InvariantCulture,
                                        "Getting filtered elements {0}",
                                        typeof(TEntity).Name, filter.ToString()));

            //Create IObjectSet and perform query
            return _context.CreateSet<TEntity>()
                             .Where(filter)
                             .ToList();
        }

        /// <summary>
        /// <see cref="Hexa.Core.Domain.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="filter"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        /// <param name="orderByExpression"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        /// <param name="ascending"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        /// <returns><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></returns>
        public IEnumerable<TEntity> GetFilteredElements<S>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, S>> orderByExpression, bool ascending)
        {
            //Checking query arguments
            if (filter == (Expression<Func<TEntity, bool>>)null)
                throw new ArgumentNullException("filter");

            if (orderByExpression == (Expression<Func<TEntity, S>>)null)
                throw new ArgumentNullException("orderByExpression");

            _logger.Debug(
                           string.Format(CultureInfo.InvariantCulture,
                                        "Getting filtered elements {0}",
                                        typeof(TEntity).Name, filter.ToString()));

            //Create IObjectSet for this type and perform query
            IEntitySet<TEntity> objectSet = _context.CreateSet<TEntity>();

            return (ascending)
                                ?
                                    objectSet
                                     .Where(filter)
                                     .OrderBy(orderByExpression)
                                     .ToList()
                                :
                                    objectSet
                                     .Where(filter)
                                     .OrderByDescending(orderByExpression)
                                     .ToList();
        }

        /// <summary>
        /// <see cref="Hexa.Core.Domain.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="pageIndex"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        /// <param name="pageCount"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        /// <param name="orderByExpression"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        /// <param name="ascending"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        /// <returns><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></returns>
        public PagedElements<TEntity> GetPagedElements<S>(int pageIndex, int pageCount, System.Linq.Expressions.Expression<Func<TEntity, S>> orderByExpression, bool ascending)
        {
            //checking arguments for this query 
            if (pageIndex < 0)
                throw new ArgumentException("pageIndex");

            if (pageCount <= 0)
                throw new ArgumentException("pageCount");

            if (orderByExpression == (Expression<Func<TEntity, S>>)null)
                throw new ArgumentNullException("orderByExpression");

            _logger.Debug(
                           string.Format(CultureInfo.InvariantCulture,
                                        "Getting paged elements {0}",
                                        typeof(TEntity).Name, pageIndex, pageCount, orderByExpression.ToString()));

            //Create associated IObjectSet and perform query

            IEntitySet<TEntity> objectSet = _context.CreateSet<TEntity>();

            var total = objectSet.Count();

            return (ascending)
                                ?
                                    new PagedElements<TEntity>(
                                        objectSet.OrderBy(orderByExpression)
                                        .Skip(pageIndex * pageCount)
                                        .Take(pageCount)
                                        .ToList(), total)
                                :
                                    new PagedElements<TEntity>(
                                        objectSet.OrderByDescending(orderByExpression)
                                        .Skip(pageIndex * pageCount)
                                        .Take(pageCount)
                                        .ToList(), total);
        }

        /// <summary>
        /// <see cref="Hexa.Core.Domain.IRepository{TEntity}"/>
        /// </summary>
        /// <typeparam name="S"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></typeparam>
        /// <param name="pageIndex"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        /// <param name="pageCount"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        /// <param name="orderByExpression"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        /// <param name="specification"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        /// <param name="ascending"><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></param>
        /// <returns><see cref="Hexa.Core.Domain.IRepository{TEntity}"/></returns>
        public PagedElements<TEntity> GetPagedElements<S>(int pageIndex, int pageCount, Expression<Func<TEntity, S>> orderByExpression, ISpecification<TEntity> specification, bool ascending)
        {
            //checking arguments for this query 
            if (pageIndex < 0)
                throw new ArgumentException("pageIndex");

            if (pageCount <= 0)
                throw new ArgumentException("pageCount");

            if (orderByExpression == (Expression<Func<TEntity, S>>)null)
                throw new ArgumentNullException("orderByExpression");

            if (specification == (ISpecification<TEntity>)null)
                throw new ArgumentNullException("specification");

            _logger.Debug(
                           string.Format(CultureInfo.InvariantCulture,
                                        "Getting paged elements {0}",
                                        typeof(TEntity).Name, pageIndex, pageCount, orderByExpression.ToString()));

            //Create associated IObjectSet and perform query

            IEntitySet<TEntity> objectSet = _context.CreateSet<TEntity>();

            var query = objectSet.Where(specification.SatisfiedBy());
            var total = query.Count();

            return (ascending)
                                ?
                                    new PagedElements<TEntity>(
                                        query.OrderBy(orderByExpression)
                                        .Skip(pageIndex * pageCount)
                                        .Take(pageCount)
                                        .ToList(), total)
                                :
                                    new PagedElements<TEntity>(
                                        query.OrderByDescending(orderByExpression)
                                        .Skip(pageIndex * pageCount)
                                        .Take(pageCount)
                                        .ToList(), total);
        }

        public PagedElements<TEntity> GetPagedElements<S>(int pageIndex, int pageCount, Expression<Func<TEntity, S>> orderByExpression, Expression<Func<TEntity, bool>> filter, bool ascending)
        {
            //checking arguments for this query 
            if (pageIndex < 0)
                throw new ArgumentException("pageIndex");

            if (pageCount <= 0)
                throw new ArgumentException("pageCount");

            if (orderByExpression == (Expression<Func<TEntity, S>>)null)
                throw new ArgumentNullException("orderByExpression");

            //checking query arguments
            if (filter == (Expression<Func<TEntity, bool>>)null)
                throw new ArgumentNullException("filter");

            _logger.Debug(
                           string.Format(CultureInfo.InvariantCulture,
                                        "Getting paged elements {0}",
                                        typeof(TEntity).Name, pageIndex, pageCount, orderByExpression.ToString()));

            //Create associated IObjectSet and perform query

            IEntitySet<TEntity> objectSet = _context.CreateSet<TEntity>();

            var query = objectSet.Where(filter);
            var total = query.Count();

            return (ascending)
                                ?
                                    new PagedElements<TEntity>(
                                        query.OrderBy(orderByExpression)
                                        .Skip(pageIndex * pageCount)
                                        .Take(pageCount)
                                        .ToList(), total)
                                :
                                    new PagedElements<TEntity>(
                                        query.OrderByDescending(orderByExpression)
                                        .Skip(pageIndex * pageCount)
                                        .Take(pageCount)
                                        .ToList(), total);
        }

        public PagedElements<TEntity> GetPagedElements(int pageIndex, int pageCount, IOrderBySpecification<TEntity> orderBySpecification, ISpecification<TEntity> specification)
        {
            //checking arguments for this query 
            if (pageIndex < 0)
                throw new ArgumentException("pageIndex");

            if (pageCount <= 0)
                throw new ArgumentException("pageCount");

            if (orderBySpecification == (IOrderBySpecification<TEntity>)null)
                throw new ArgumentNullException("orderBySpecification");

            //checking query arguments
            if (specification == (ISpecification<TEntity>)null)
                throw new ArgumentNullException("specification");

            _logger.Debug(
                           string.Format(CultureInfo.InvariantCulture,
                                        "Getting paged elements {0}",
                                        typeof(TEntity).Name, pageIndex, pageCount, orderBySpecification.ToString()));

            //Create associated IObjectSet and perform query
            IEntitySet<TEntity> objectSet = _context.CreateSet<TEntity>();

            var query = objectSet.Where(specification.SatisfiedBy());
            var total = query.Count();

            return new PagedElements<TEntity>(
                            query
                            .OrderBySpecification(orderBySpecification)
                            .Skip(pageIndex * pageCount)
                            .Take(pageCount)
                            .ToList(), total);
        }

        public PagedElements<TEntity> GetPagedElements(int pageIndex, int pageCount, IOrderBySpecification<TEntity> orderBySpecification, Expression<Func<TEntity, bool>> filter)
        {
            //checking arguments for this query 
            if (pageIndex < 0)
                throw new ArgumentException("pageIndex");

            if (pageCount <= 0)
                throw new ArgumentException("pageCount");

            if (orderBySpecification == (IOrderBySpecification<TEntity>)null)
                throw new ArgumentNullException("orderBySpecification");

            //checking query arguments
            if (filter == (Expression<Func<TEntity, bool>>)null)
                throw new ArgumentNullException("filter");

            _logger.Debug(
                           string.Format(CultureInfo.InvariantCulture,
                                        "Getting paged elements {0}",
                                        typeof(TEntity).Name, pageIndex, pageCount, orderBySpecification.ToString()));

            //Create associated IObjectSet and perform query
            IEntitySet<TEntity> objectSet = _context.CreateSet<TEntity>();

            var query = objectSet.Where(filter);
            var total = query.Count();

            return new PagedElements<TEntity>(
                            query
                            .OrderBySpecification(orderBySpecification)
                            .Skip(pageIndex * pageCount)
                            .Take(pageCount)
                            .ToList(), total);
        }

        #endregion
    }
}
