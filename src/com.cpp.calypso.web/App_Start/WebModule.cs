using Abp.Application.Services;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Runtime.Session;
using Abp.Web.Mvc;
using Abp.Web.Mvc.Configuration;
using Abp.WebApi;
using Castle.MicroKernel.Registration;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.comun.entityframework;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.entityframework;
using com.cpp.calypso.seguridad.aplicacion;
using com.cpp.calypso.seguridad.entityframework;
using CommonServiceLocator;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace com.cpp.calypso.web.App_Start
{



    [DependsOn(
     typeof(ComunEntityFrameworkModule),
     typeof(SeguridadEntityFrameworkModule),
     typeof(ProyectoEntityFrameworkModule),

     typeof(ComunAplicacionModule),
     typeof(SeguridadAplicacionModule),
     typeof(ProyectoAplicacionModule),
     typeof(AbpWebMvcModule),
     typeof(AbpWebApiModule))]
    public class WebModule: AbpModule
    {

        public override void PreInitialize()
        {
        
            //LOG
            //Logs con Nlog, revisar la section nlog para las configuraciones
            IocManager.Register<ILoggerFactory, NLogFactory>(Abp.Dependency.DependencyLifeStyle.Singleton);
 
         
            //Gestion de Identidades 
            IocManager.Register<IIdentityUser, IdentityUser>(Abp.Dependency.DependencyLifeStyle.Singleton);
 
            //Gestion de Informacion de Usuarios Externos para sincronizacion de usuarios internos
            IocManager.Register<IExternalInfoUserProvider, ByPassInfoUserProvider>(Abp.Dependency.DependencyLifeStyle.Singleton);

 
            //VIEW
            IocManager.Register<IGenerateWidget, GenerateWidget>(Abp.Dependency.DependencyLifeStyle.Singleton);

            IocManager.Register<IAutoGeneratedView, AutoGeneratedView>(Abp.Dependency.DependencyLifeStyle.Singleton);

            IocManager.Register<ISerializerLayout, SerializerLayout>(Abp.Dependency.DependencyLifeStyle.Singleton);


            //TODO: Cambiar a PerWebRequest. Pendiente configuracion de Castle para soportar
            IocManager.IocContainer.Register(
                 Component
                 .For<IViewService>()
                 .ImplementedBy<ViewService>()
                 .LifestyleTransient()
                 //.LifestylePerWebRequest() 
             );

            //Aplicacion personalizada para esta aplicacion.  
            IocManager.IocContainer.Register(
                Component
                .For<IApplication>()
                .ImplementedBy<GenericApplication>()
                .LifestyleTransient()
                //.LifestylePerWebRequest() 
            );


            ////Implementacion Instancia Default
            IocManager.IocContainer.Register(
                Component.For<PrincipalBaseDbContext>().
                ImplementedBy<PrincipalDbContext>()
                .Named("PrincipalDbContext")
                .LifestyleTransient()
                .IsDefault()
             );


        }



        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());


           
            //IocManager.IocContainer.Register(
            //    Component
            //        .For<IAuthenticationManager>()
            //        .UsingFactoryMethod(() => HttpContext.Current.GetOwinContext().Authentication)
            //        .LifestyleTransient()
            //);


            //Establecer el Service Locator con Windsor
            var windsorMapDependencyScope = new WindsorServiceLocator(IocManager.IocContainer);
            ServiceLocator.SetLocatorProvider(() => windsorMapDependencyScope);



            IocManager.IocContainer.Register(
               Component
                   .For<IAuthenticationManager>()
                   .UsingFactoryMethod(() => HttpContext.Current.GetOwinContext().Authentication)
                   .LifestyleTransient()
           );


            AreaRegistration.RegisterAllAreas();

            //GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //Generar rest dinamicos desde los servicios de aplicacion
            //TODO: Determinar si por defecto se generan todos los servicios de aplicaciones, o unicamente
            //servicios que posean una marca "Interfaz"
            
            //  Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
          //.ForAll<IApplicationService>(typeof(ComunAplicacionModule).Assembly, "app")
          //.Build();

            //Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
            //   .ForAll<IApplicationService>(typeof(SeguridadAplicacionModule).Assembly, "app")
            //   .Build();

            //Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
            //      .ForAll<IApplicationService>(typeof(ProyectoAplicacionModule).Assembly, "app")
            //      .Build();

        }

        public override void PostInitialize()
        {
            //Desactivar validaciones de Abp en los controlladores
            Configuration.Modules.AbpMvc().IsValidationEnabledForControllers = false;

            //Desactivar antiforgery de abp
            Configuration.Modules.AbpWeb().AntiForgery.IsEnabled = false;


            //abp. Ya posee binder para fechas
            //Aplicar Binder personalizado de fechas a todo el proyecto
            //var loggerFactory = IocManager.Resolve<ILoggerFactory>();
            //ModelBinders.Binders.Add(typeof(DateTime), new CustomDateBinder(loggerFactory));
            //ModelBinders.Binders.Add(typeof(DateTime?), new NullableCustomDateBinder());
            var session = IocManager.Resolve<IAbpSession>();
            var settingManager = IocManager.Resolve<ISettingManager>();
            //var settingStore = IocManager.Resolve<ISettingStore>();

            //var settingValues = await settingStore.GetAllListAsync(null, null);

            //Form Dyamic
            var viewService = IocManager.Resolve<IViewService>();
            ModelBinders.Binders.Add(typeof(List<FilterEntity>), new FilterEntityModelBinder(viewService));

            //var fileStorage =
            //ServiceLocator.Current.GetInstance<IFileStorage>();


            //            ModelBinders.Binders.Add(typeof(ParametroSistema),
            //                new ParametroSistemaModelBinder(fileStorage));

            //ModelBinders.Binders.Add(typeof(AuditoriaCriteria), new AuditoriaCriteriaBinder());


            RegisterAdaptarDataAnnotationsCustom();
        }

        /// <summary>
        /// Registrar adaptadores para atributos DataAnnotations personalizados
        /// </summary>
        private void RegisterAdaptarDataAnnotationsCustom() {

            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(ObligadoAttribute),
                typeof(RequiredAttributeAdapter));


            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(LongitudMayorAttribute),
                typeof(MaxLengthAttributeAdapter));


            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RangoAttribute),
                typeof(RangeAttributeAdapter));  


        }
    }


}