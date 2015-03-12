jQuery(document).ready(function () {

    // sauvegarde le titre de la popup :
    if ($('#hdTitlePopup').val() == "") { $('#hdTitlePopup').val($('#recherche div.modal-header #myModalLabel').html()); }

    $('tr.row-Modal').click(function (event) { EventRowClick($(this)); });
});

function EventRowClick(element) {
    // récupère le div qui contient les données sauvegardées :
    var div = $("div.accordion")[0];

    $('#recherche').modal('hide');
    $('#recherche div.modal-body').html('');
    $('#recherche div.modal-header #myModalLabel').html('');
    $('#recherche div.modal-footer input.btn-success').remove();

    // récupère l'id de la personne, le pseudo et le texte pour le bouton de validation
    var id = element.attr('data-identifier');
    var pseudo = element.attr('data-pseudo');
    var btnText = element.parents('table').attr('data-validemessage') + pseudo;

    // il faut savoir si la ligne cliqué concerne des photos d'habillage ou de vêtement
    var isHabillage = element.hasClass("habillage");

    // Récupère le style en cours dans la page
    var styleId = jQuery.data(div, "styleEnCours").styleId;

    if (id != undefined) {
        var datas = "\r\n";
        $('#hdIdentifier').val(id);
        var url = '';
        if (isHabillage) {
            url = jQuery.data(div, "urlRecherche").urlHabillage;
            var jqxhr = $.ajax({
                url: url,
                cache: true,
                type: 'GET',
                data: { style: styleId, personne: id }
            })
            .success(function (data) { GestionModal(element, data, btnText, pseudo); })
            .fail(function () { alert("Une erreur est survenue lors de la récupération des photos !"); });
        }
        else {
            console.log('vetement');
            url = jQuery.data(div, "urlRecherche").urlVetement;
            var jqxhr = $.ajax({
                url: url,
                cache: true,
                type: 'GET',
                data: { style: styleId, vetement: 0, personne: id }
            })
            .success(function (data) { GestionModal(element, data, btnText, pseudo); })
            .fail(function () { alert("Une erreur est survenue lors de la récupération des photos !"); });
        }
    }
}


function GestionModal($element, datas, btnText, pseudo) {
    if (datas != "\r\n") {
        // IMPORTANT : permet d'agrandir la popup
        $('#recherche').css("width", "697px");
        $('#recherche div.modal-body').html(datas);

        // Complète le titre de la popup
        var title = $('#hdTitlePopup').val(); //$('#recherche div.modal-header #myModalLabel').html();
        $('#recherche div.modal-header #myModalLabel').html(title + ' de ' + pseudo);

        // Ajoute le bouton de validation
        var footerContent = $('#recherche div.modal-footer').html();
        $('#recherche div.modal-footer').html("<input class='btn btn-success' style='float:left' type='submit' value='" + btnText + "'>" + footerContent);
        CarouselHorizontalSetting();

        // calcul automatique du width du carousel
        UpdateWidthForInLinePictures(488);

        // enlève les évènements de suppression sur les images
        $('#mycarousel tr').each(function () {
            var obj = $element.find('td');

            // supprime le premier td qui contient le texte Supprimer
            if (obj.html() == "<span class=\"text-info\">Supprimer</span>") { $element.remove(); }
            else {
                $element.unbind("click");
                $element.attr("onclick", "");
            }
        });

        $('#mycarousel td').each(function () {
            $element.unbind("click");
            $element.attr("onclick", "");
        });

        // affiche la popup modal
        $('#recherche').modal('show');
    }
    else { $('#recherche').modal('hide'); alert("Aucune photo disponible !"); }
}