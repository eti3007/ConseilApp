jQuery(document).ready(function () {
    // Charge les informations de notification
    ChargerNotification();
});

function ChargerNotification() {
    // récupère le div qui contient les données sauvegardées :
    var div = $("div.accordion")[0];
    // Récupère le style en cours dans la page
    var styleId = jQuery.data(div, "styleEnCours").styleId;

    var url = jQuery.data(div, "urlRecherche").urlNotification;
    console.log(styleId + "   "+ url);
    var jqxhr = $.ajax({
        url: url,
        cache: true,
        type: 'GET',
        data: { style: styleId }
    })
    .success(function (data) { AfficheNotification(data); })
    .fail(function () { alert("Une erreur est survenue lors de la récupération des notifications !"); });
}

function AfficheNotification(datas) {
    if (datas != "\r\n") {
        console.log(datas);

        $('#Notifications').html(datas);

        //CarouselHorizontalSetting();

        // calcul automatique du width du carousel
        //UpdateWidthForInLinePictures(488);

        // enlève les évènements de suppression sur les images
        //RemoveDeleteLink();

    }
    else { $('#Notifications').html("Aucune notification trouvée !"); }
}
