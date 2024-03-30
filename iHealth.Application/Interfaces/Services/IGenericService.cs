namespace iHealth.Core.Application.Interfaces.Services
{
    public interface IGenericService<Entity, EntityViewModel, SaveEntityViewModel> 
        where Entity : class
        where EntityViewModel : class
        where SaveEntityViewModel : class
    {
        public Task<List<EntityViewModel>> GetAll();
        public Task<SaveEntityViewModel> Add(SaveEntityViewModel vm);
        public Task<SaveEntityViewModel> GetById(int Id);
        public Task Update(SaveEntityViewModel vm);
        public Task Delete(int Id);
        public abstract Task<Entity> VMToModel(EntityViewModel vm);
        public abstract Task<EntityViewModel> ModToViewModel(Entity m);
        public abstract Task<SaveEntityViewModel> VMToSave(EntityViewModel vm);
        public abstract Task<EntityViewModel> SaveToVM(SaveEntityViewModel sm);

    }
}
