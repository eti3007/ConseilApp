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
    
    // récupère l'information du tableau en cours :
    var typeTbl = element.attr('data-type');
    
    // si le tableau est "sollicite de l'aide" ou "propose de l'aide" alors on récupère l'identifiant du conseil :
    var conseil = 0;
    if (typeTbl == "d2" || typeTbl == "p2") {
        conseil = element.attr('data-conseil');
    }

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
            .success(function (data) { GestionModal(element, data, btnText, pseudo, typeTbl, id, conseil); })
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
            .success(function (data) { GestionModal(element, data, btnText, pseudo, typeTbl, id, conseil); })
            .fail(function () { alert("Une erreur est survenue lors de la récupération des photos !"); });
        }
    }
}


function GestionModal($element, datas, btnText, pseudo, typeTbl, id, conseil) {
    if (datas != "\r\n") {
        // IMPORTANT : permet d'agrandir la popup
        $('#recherche').css("width", "697px");
        $('#recherche div.modal-body').html(datas);

        // Complète le titre de la popup
        var title = $('#hdTitlePopup').val(); //$('#recherche div.modal-header #myModalLabel').html();
        $('#recherche div.modal-header #myModalLabel').html(title + ' de ' + pseudo);

        // Ajoute le bouton de validation
        var footerContent = $('#recherche div.modal-footer').html();
        $('#recherche div.modal-footer').html("<input class='btn btn-success' style='float:left' type='button' value='" + btnText +
            "' onclick='AppelActionAide(\"" + typeTbl + "\", " + id + ", " + conseil + ")'>" + footerContent);
        CarouselHorizontalSetting();

        // calcul automatique du width du carousel
        UpdateWidthForInLinePictures(488);

        // enlève les évènements de suppression sur les images
        RemoveDeleteLink();

        // affiche la popup modal
        $('#recherche').modal('show');
    }
    else { $('#recherche').modal('hide'); alert("Aucune photo disponible !"); }
}

function AppelActionAide(typeTbl, id, conseil)
{
    var div = $("div.accordion")[0];
    var url = jQuery.data(div, "urlRecherche").urlAction;
    var style = jQuery.data(div, "styleEnCours").styleId;
    var page = jQuery.data(div, "urlRecherche").pageName;
    var datas = undefined;

    console.log(url + '  ' + style + '  ' + page + '  ' + typeTbl);


    switch (typeTbl) {
        case "d1":
            datas = { conseillerId: id, styleId: style, pageName: page };
            break;
        case "d2":
            datas = { conseilId: conseil, conseillerId: id, styleId: style, pageName: page };
            break;
        case "p1":
            datas = { demandeurId: id, styleId: style, pageName: page };
            break;
        case "p2":
            datas = { conseilId: conseil, demandeurId: id, styleId: style, pageName: page };
            break;
    }

    console.log(datas);

    if (datas != undefined) {
        var jqxhr = $.ajax({
            url: url,
            cache: true,
            type: 'POST',
            data: datas
        })
        .success(function (data) {
            $('#recherche').modal('hide'); alert(data);
        })
        .fail(function () {
            $('#recherche').modal('hide'); alert("Une erreur est survenue lors de l'exécution de l'action !");
        });
    }
}