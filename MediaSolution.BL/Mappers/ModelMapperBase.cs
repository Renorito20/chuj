﻿using System.Collections.Generic;
using System.Linq;
using MediaSolution.BL.Mappers.Interfaces;

namespace MediaSolution.BL.Mappers
{
    public abstract class ModelMapperBase<TEntity, TListModel, TDetailModel> : IModelMapper<TEntity, TListModel, TDetailModel>
    {
        public abstract TListModel MapToListModel(TEntity? entity);

        public IEnumerable<TListModel> MapToListModel(IEnumerable<TEntity> entities)
            => entities.Select(MapToListModel);

        public abstract TDetailModel MapToDetailModel(TEntity entity);
        public abstract TEntity MapToEntity(TDetailModel model);
    }
}