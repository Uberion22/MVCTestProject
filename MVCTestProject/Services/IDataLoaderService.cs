namespace MVCTestProject.Services
{
    public interface IDataLoaderService: IHostedService
    {
        public void SetService(IServiceProvider hostedService);
    }
}
