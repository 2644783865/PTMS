using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PTMS.Common;

namespace PTMS.BusinessLogic.Infrastructure
{
    public abstract class BusinessServiceAsync<TEntity>
        where TEntity: class, new ()
    {
        protected readonly IMapper _mapper;

        protected BusinessServiceAsync(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected TModel MapToModel<TModel>(TEntity entity)
        {
            return _mapper.Map<TEntity, TModel>(entity);
        }

        protected List<TModel> MapToModel<TModel>(List<TEntity> entities)
        {
            return entities.Select(MapToModel<TModel>).ToList();
        }

        protected PageResult<TModel> MapToModel<TModel>(PageResult<TEntity> pageResult)
        {
            var list = MapToModel<TModel>(pageResult.Page);
            return new PageResult<TModel>(list, pageResult.TotalCount);
        }

        protected TEntity MapFromModel<TModel>(TModel model)
        {
            return _mapper.Map<TModel, TEntity>(model);
        }
    }
}
