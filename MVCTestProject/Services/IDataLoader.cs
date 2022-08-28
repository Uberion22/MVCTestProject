namespace MVCTestProject.Services
{
    public interface IDataLoader: IHostedService
    {
        public void SetService(IServiceProvider hostedService);
    }
}
