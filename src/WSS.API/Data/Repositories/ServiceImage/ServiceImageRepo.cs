using System.Linq.Expressions;
using L.Core.Data.EFCore.DbContextFactory;
using L.Core.Data.EFCore.Repository;
using WSS.API.Data.Models;

namespace WSS.API.Data.Repositories.ServiceImage;

public class ServiceImageRepo : IServiceImageRepo
{
      private readonly IGenericRepository<Models.ServiceImage> _repo;
    private readonly IDbContextFactory _dbContextFactory;

    /// <summary>
    /// Init
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public ServiceImageRepo(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _repo = _dbContextFactory.UnitOfWork<WssContext, Models.ServiceImage>().Repository ?? throw new InvalidOperationException();
    }

    /// <inheritdoc />
    public IQueryable<Models.ServiceImage> GetServiceImages(Expression<Func<Models.ServiceImage, object>>[]? includeProperties = null)
    {
        return this._repo.GetAll(includeProperties);
    }

    /// <inheritdoc />
    public async Task<Models.ServiceImage> CreateServiceImage(Models.ServiceImage user, bool tempSave = false)
    {
        await this._repo.InsertAsync(user);

        _ = tempSave
            ? await this._dbContextFactory.UnitOfWork<WssContext, Models.ServiceImage>().SaveTempChangesAsync()
            : await this._dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.ServiceImage> UpdateServiceImage(Models.ServiceImage user, bool tempSave = false)
    {
        await this._repo.UpdateAsync(user);

        _ = tempSave
            ? await this._dbContextFactory.UnitOfWork<WssContext, Models.ServiceImage>().SaveTempChangesAsync()
            : await this._dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.ServiceImage> DeleteServiceImage(Models.ServiceImage user, bool tempSave = false)
    {
        await this._repo.DeleteAsync(user);
        
        _ = tempSave
            ? await this._dbContextFactory.UnitOfWork<WssContext, Models.ServiceImage>().SaveTempChangesAsync()
            : await this._dbContextFactory.SaveAllAsync();

        return user;
    }

    /// <inheritdoc />
    public async Task<Models.ServiceImage?> GetServiceImageById(Guid id, Expression<Func<Models.ServiceImage, object>>[]? includeProperties = null)
    {
        Models.ServiceImage? user = await this._repo.GetByIdAsync(id, includeProperties);
        return user;
    }
}