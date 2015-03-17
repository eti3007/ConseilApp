﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConseilBLL.Interfaces;
using ConseilOBJ;
using ConseilApp.Models;

namespace ConseilApp.Controllers
{
    public class NotificationController : BaseController
    {
        private INotificationService _NotificationService;

        public NotificationController(INotificationService NotificationService) {
            this._NotificationService = NotificationService;
        }

        [Authorize]
        public PartialViewResult AfficheNotificationAbonne(string style, int personne)
        {
            if (!string.IsNullOrEmpty(style) && personne > 0) {
                // récupère la liste des notifications
                var lstNotification = this._NotificationService.RecupereListeNotification(Convert.ToInt32(style), personne);

                List<NotificationViewModel> viewModel = new List<NotificationViewModel>();

                // création du model pour la vue partielle
                lstNotification.ForEach(c => viewModel.Add(new NotificationViewModel() { DateNotif = c.DateCreation.ToShortDateString(), Message = c.Message }));
                lstNotification = null;

                return PartialView("_AfficheNotification", viewModel);
            }
            else
                return null;
        }
    }
}