using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PTMS.Common;

namespace PTMS.BusinessLogic.Infrastructure
{
    public abstract class BusinessServiceAsync<TEntity, TModel>
        where TEntity: class, new ()
        where TModel : class, new()
    {
        protected readonly IMapper _mapper;

        protected BusinessServiceAsync(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected TModel MapToModel(TEntity entity)
        {
            return _mapper.Map<TEntity, TModel>(entity);
        }

        protected List<TModel> MapToModel(System.Collections.Generic.List<TEntity> entities)
        {
            return entities.Select(MapToModel).ToList();
        }

        protected PageResult<TModel> MapToModel(PageResult<TEntity> pageResult)
        {
            var list = MapToModel(pageResult.Page);
            return new PageResult<TModel>(list, pageResult.TotalCount);
        }

        protected TEntity MapFromModel(TModel model)
        {
            return _mapper.Map<TModel, TEntity>(model);
        }
    }
}
