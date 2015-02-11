using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConseilOBJ
{
    public static class ConstantMessages
    {
        public const string MSG_ENVOI_DEMANDE_CONSEILLER = "{0} vous demande de l'aide pour le style {1}";
        public const string MSG_ENVOI_DEMANDE_DEMANDEUR = "Votre demande a été envoyé à {0}";
        public const string MSG_ACCEPTE_PROPOSITION_CONSEILLER = "{0} a accepté votre aide pour le style {1}";
        public const string MSG_REFUSE_PROPOSITION_CONSEILLER = "{0} a refusé votre aide pour le style {1}";

        public const string MSG_ENVOI_PROPOSITION_DEMANDEUR = "{0} vous propose son aide pour le style {1}";
        public const string MSG_ENVOI_PROPOSITION_CONSEILLER = "Votre proposition d'aide a été envoyé à {0}";
        public const string MSG_ACCEPTE_DEMANDE_DEMANDEUR = "{0} a accepté de vous aider pour le style {1}";
        public const string MSG_REFUSE_DEMANDE_DEMANDEUR = "{0} a refusé de vous aider pour le style {1}";
    }
}
