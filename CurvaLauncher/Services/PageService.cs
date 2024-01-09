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
        private readonly IServiceProvider _serviceProvider;

        public PageService(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T? GetPage<T>() where T : class => _serviceProvider.GetRequiredService<T>();
        public FrameworkElement? GetPage(Type pageType) => _serviceProvider.GetRequiredService(pageType) as FrameworkElement;
    }
}
