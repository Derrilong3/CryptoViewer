using Caliburn.Micro;
using CryptoViewer.Base.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.ReflectionModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace CryptoViewer
{
    internal class AppBootstrapper : BootstrapperBase
    {
        private List<Assembly> _priorityAssemblies;

        protected CompositionContainer Container { get; set; }

        internal IList<Assembly> PriorityAssemblies
            => _priorityAssemblies;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            PopulateAssemblySource();

            _priorityAssemblies = SelectAssemblies().ToList();
            var priorityCatalog = new AggregateCatalog(_priorityAssemblies.Select(x => new AssemblyCatalog(x)));
            var priorityProvider = new CatalogExportProvider(priorityCatalog);

            var mainCatalog = new AggregateCatalog(
                AssemblySource.Instance
                    .Where(assembly => !_priorityAssemblies.Contains(assembly))
                    .Select(x => new AssemblyCatalog(x)));
            var mainProvider = new CatalogExportProvider(mainCatalog);

            Container = new CompositionContainer(priorityProvider, mainProvider);
            priorityProvider.SourceProvider = Container;
            mainProvider.SourceProvider = Container;

            var batch = new CompositionBatch();

            BindServices(batch);
            batch.AddExportedValue(mainCatalog);

            Container.Compose(batch);
        }

        protected virtual void PopulateAssemblySource()
        {
            string currentWorkingDir = Path.GetDirectoryName(Path.GetFullPath(@"./"));
            string baseDirectory = Path.GetDirectoryName(Path.GetFullPath(AppContext.BaseDirectory));

            PopulateAssemblySourceUsingDirectoryCatalog(currentWorkingDir);
            if (currentWorkingDir != baseDirectory)
            {
                PopulateAssemblySourceUsingDirectoryCatalog(baseDirectory);
            }
        }

        protected void PopulateAssemblySourceUsingDirectoryCatalog(string path)
        {
            var directoryCatalog = new DirectoryCatalog(path);
            AssemblySource.Instance.AddRange(
                directoryCatalog.Parts
                    .Select(part => ReflectionModelServices.GetPartType(part).Value.Assembly)
                    .Where(assembly => !AssemblySource.Instance.Contains(assembly)));
        }

        protected virtual IEnumerable<Assembly> PublishSingleFileBypassAssemblies
            => Enumerable.Empty<Assembly>();

        protected virtual void BindServices(CompositionBatch batch)
        {
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(Container);
            batch.AddExportedValue(this);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = Container.GetExports<object>(contract);

            if (exports.Any())
                return exports.First().Value;

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
            => Container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));

        protected override void BuildUp(object instance)
            => Container.SatisfyImportsOnce(instance);

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            DisplayRootViewForAsync<IShell>();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
            => new[] { Assembly.GetEntryAssembly() };
    }
}
