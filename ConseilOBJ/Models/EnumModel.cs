namespace ConseilOBJ
{
    public enum PersonneStatus
    {
        Abonne = 1,
        EnAttente,
        Conseiller
    }
    public enum PhotoType
    {
        Habille = 4,
        Vetement
    }
    public enum MessageType
    {
        Message = 6,
        Notification,
        Information,
    }
    public enum DemandeStatus
    {
        AttenteDemandeur = 9,
        AttenteConseiller,
        Accepte,
        RefusDemandeur,
        RefusConseiller,
        AnnulAdmin,
        Termine
    }
    public enum VetementType
    {
        Tete = 16,
        Buste,
        Jambe,
        Pied,
        Accessoire,
        Main
    }
    public enum CompteurType
    {
        Abonne = 22,
        Conseiller
    }
    public enum NotifType
    {
        DemandCreation = 24,
        DemandAccept,
        DemandReject,
        PropositionAccept,
        PropositionReject,
        PropositionCreation,
        Termine
    }
}
