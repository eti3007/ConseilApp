using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using ConseilBLL;
using ConseilBLL.Interfaces;
using ConseilREP;
using ConseilREP.Interfaces;
using ConseilApp.Builders;
using ConseilApp.Builders.Interfaces;

namespace ConseilApp
{
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();

      DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
        var container = new UnityContainer();

        // register all your components with the container here
        // it is NOT necessary to register your controllers

        // e.g. container.RegisterType<ITestService, TestService>(); 
  
        // Service :
        container.RegisterType<IPersonneService, PersonneService>();
        container.RegisterType<IPhotoService, PhotoService>();
        container.RegisterType<IStatutHistoriqueService, StatutHistoriqueService>();
        container.RegisterType<IStyleService, StyleService>();
        container.RegisterType<ISuiviTelechargementService, SuiviTelechargementService>();
        container.RegisterType<ITypeVetementService, TypeVetementService>();
        container.RegisterType<IVetementService, VetementService>();
        container.RegisterType<IConseilService, ConseilService>();
        container.RegisterType<INotificationService, NotificationService>();
 
        // Repository :
        container.RegisterType<IConseilRepository,ConseilRepository>();
        container.RegisterType<INotificationRepository, NotificationRepository>();
        container.RegisterType<IPersonneRepository,PersonneRepository>();
        container.RegisterType<IPhotoRepository,PhotoRepository>();
        container.RegisterType<IStyleRepository,StyleRepository>();
        container.RegisterType<ISuiviTelechargeRepository,SuiviTelechargeRepository>();
        container.RegisterType<ITypeVetementRepository, TypeVetementRepository>();
        container.RegisterType<IStatutHistoriqueRepository, StatutHistoriqueRepository>();
        container.RegisterType<IVetementRepository, VetementRepository>();

        // Builder :
        container.RegisterType<IPersonneBuilder, PersonneBuilder>();
        container.RegisterType<IRechercheBuilder, RechercheBuilder>();
        container.RegisterType<IMenuBuilder, MenuBuilder>();


        RegisterTypes(container);

        return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
    
    }
  }
}