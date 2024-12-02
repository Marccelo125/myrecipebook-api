using MyRecipeBook.Application.UserCases.User.Delete;

namespace MyRecipeBook.API.BackgroundServices
{
    public class DeleteUserServiceTimerUseCase : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<DeleteUserServiceTimerUseCase> _logger;

        public DeleteUserServiceTimerUseCase(IServiceProvider serviceProvider, ILogger<DeleteUserServiceTimerUseCase> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogWarning("Delete User Service Timer Ativo");
        
            await DoWork();
        
            using PeriodicTimer timer = new(TimeSpan.FromSeconds(10));
        
            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await DoWork();
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Delete User Service Timer parado");
            }
        }

        private async Task DoWork()
        {
            var scope = _serviceProvider.CreateScope();

            var deleteUserAccountUseCase = scope.ServiceProvider.GetRequiredService<IDeleteUserAccountTimerUseCase>();

            var userId = await deleteUserAccountUseCase.Execute();

            if (userId > 0)
                _logger.LogWarning($"Usuário de ID: {userId} excluído com sucesso!");
            else
                _logger.LogInformation($"Nenhum usuário para deletar");
        }
    }
}
