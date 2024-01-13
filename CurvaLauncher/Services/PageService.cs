using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Mvvm.Contracts;

namespace CurvaLauncher.Services
{
    public class PageService : IPageService
    {
        private readonly IServiceScope _serviceScope;

        public PageService(
            IServiceProvider serviceProvider)
        {
            _serviceScope = serviceProvider.CreateScope();
        }

        public T? GetPage<T>() where T : class => _serviceScope.ServiceProvider.GetRequiredService<T>();
        public FrameworkElement? GetPage(Type pageType) => _serviceScope.ServiceProvider.GetRequiredService(pageType) as FrameworkElement;
    }
}
