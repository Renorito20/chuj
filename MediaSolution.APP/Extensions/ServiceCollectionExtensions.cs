
using MediaSolution.APP.ViewModels;
using MediaSolution.APP.Views;
using ServiceScan.SourceGenerator;

namespace MediaSolution.APP.Extensions;

public static partial class ServiceCollectionExtensions
{
    [GenerateServiceRegistrations(AssignableTo = typeof(ContentPageBase), AsSelf = true,
        Lifetime = ServiceLifetime.Transient)]
    public static partial IServiceCollection AddViews(this IServiceCollection services);
        
    [GenerateServiceRegistrations(AssignableTo = typeof(ViewModelBase), AsSelf = true, 
        Lifetime = ServiceLifetime.Transient)]
    public static partial IServiceCollection AddViewModels(this IServiceCollection services);
}