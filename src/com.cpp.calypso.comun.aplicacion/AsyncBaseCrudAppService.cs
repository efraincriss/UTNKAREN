﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.cpp.calypso.comun.aplicacion
{


    public class AsyncBaseCrudAppService<TEntity, TEntityDto, TGetAllInput> :
       AsyncBaseCrudAppService<TEntity, TEntityDto, TGetAllInput, TEntityDto>
       where TEntity : class, IEntity<int>, IEntity
       where TEntityDto : IEntityDto<int>
       where TGetAllInput : IPagedAndSortedResultRequest
    {
        public AsyncBaseCrudAppService(
            IBaseRepository<TEntity> repository) : base(repository)
        {
        }


    }


    public class AsyncBaseCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput> :
        AsyncCrudAppService<TEntity, TEntityDto, int, TGetAllInput, TCreateInput>,
        IAsyncBaseCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput>
       where TEntity : class, IEntity<int>, IEntity
       where TEntityDto : IEntityDto<int>
       where TCreateInput : IEntityDto<int>
       where TGetAllInput : IPagedAndSortedResultRequest
    {
        private  IHandlerExcepciones manejadorExcepciones;


        public virtual IHandlerExcepciones ManejadorExcepciones {
            get => manejadorExcepciones; set => manejadorExcepciones = value; }

        public AsyncBaseCrudAppService(
            IBaseRepository<TEntity> repository) : base(repository)
        {
        }



        public override async Task<PagedResultDto<TEntityDto>> GetAll(TGetAllInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);


            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<TEntityDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
        }


        /// <summary>
        ///   apply filter if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> query, TGetAllInput input)
        {
            //Try to sort query if available
            var filterInput = input as PagedAndFilteredResultRequestDto;
            if (filterInput != null && filterInput.Filter != null)
            {
                return query.Where(filterInput.Filter);
            }

            //No filter
            return query;
        }


        public virtual async Task<TEntityDto> InsertOrUpdateAsync(TCreateInput input)
        {
            var baseRepository = Repository as IBaseRepository<TEntity>;
            if (baseRepository == null)
            {
                throw new ArgumentException(string.Format("The repository should be type IBaseRepository<TEntity>. {0}", Repository.GetType()), "input");
            }

            var entity = MapToEntity(input);

            await baseRepository.InsertOrUpdateAsync(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        public virtual async Task<IList<TEntityDto>> GetAll()
        {
            var items = await Repository.GetAllListAsync();
            return items.Select(MapToEntityDto).ToList();
        }

        public virtual IList<TEntityDto> GetAllIncluding()
        {
            var items = Repository.GetAllIncluding().ToList();
            return items.Select(MapToEntityDto).ToList();
        }
    }
}
