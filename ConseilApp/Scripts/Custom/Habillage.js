"use strict";

jQuery(document).ready(function () {
    $('#tbEnCours tr.row-Modal').click(function (event) { EventRowClick($(this)); });
    $('#ValideConseil').click(function (event) { ResetConceptionHabillage(); });
});

// Evènement lors du click sur un conseil du tableau
function EventRowClick(element) {
    // récupère les information de la ligne sélectionnée
    var conseil = element.attr('data-conseil');
    var isConception = element.attr('data-conception');
    var personne = element.attr('data-personne');
    
    $('#divHabillage').show();

    // sauvegarde le conseil id sélectionné ainsi que le type de la page
    var div = $("#divHabillage")[0];
    $.data(div, "Conseil", {
        conseilId: conseil,
        personneId: personne,
        isconception: isConception
    });

    // récupère les vues
    var url = '';
    var _data = { conseilId: conseil };
    if (isConception) {
        url = '@Url.Action("ListeHabillage", "Habillage")';
        var jqxhrA = $.ajax({
            url: url,
            cache: true,
            type: 'GET',
            data: _data
        })
        .success(function (data) { $('divVisualisation').html(data); })
        .fail(function () { alert("Une erreur est survenue lors de la récupération des informations du conseil !"); });

        url = '@Url.Action("ConcevoirHabillage", "Habillage")';
        var jqxhrB = $.ajax({
            url: url,
            cache: true,
            type: 'GET',
            data: _data
        })
        .success(function (data) { $('divConception').html(data); })
        .fail(function () { alert("Une erreur est survenue lors de l'affichage de l'étape de conception !"); });
    }
    else {
        url = '@Url.Action("VisualiserHabillage", "Habillage")';
        var jqxhrB = $.ajax({
            url: url,
            cache: true,
            type: 'GET',
            data: _data
        })
        .success(function (data) { $('divVisualisation').html(data); })
        .fail(function () { alert("Une erreur est survenue lors de la récupération des informations du conseil !"); });
    }
}

// Fonction appelée après qu'un conseiller clôture/ferme un conseil
function ResetConceptionHabillage() {
    // on apelle la méthode d'action pour clôturer le conseil
    var div = $("#divHabillage")[0];
    var id = jQuery.data(div, "Conseil").conseilId;
    var jqxhr = $.ajax({
        url: '@Url.Action("ClotureConseil", "Habillage")',
        cache: true,
        type: 'POST',
        data: { conseilId: id }
    })
    .success(function (data) {
        alert('Conseil clôturé avec succès !');
    })
    .fail(function () {
        alert("Une erreur est survenue lors de l'exécution de l'action clôture conseil !");
    });


    // on vide les div de conception et visualisation et masque le div parent
    $('#divConception').html('');
    $('#divVisualisation').html('');
    $('#divHabillage').hide();
}

// Fonction appelée après qu'un conseiller propose un habillage
function ChargeTableauConseil() {
    var divDataA = $("#divTableau")[0];
    var _style = Number.parseInt(jQuery.data(divDataA, "infos").style);
    var _personne = Number.parseInt(jQuery.data(divDataA, "infos").personne);

    var divDataB = $("#divHabillage")[0];
    var _isconception = Number.parseInt(jQuery.data(divDataB, "Conseil").isconception);
    
    var url = '@Url.Action("AfficheConseils", "Habillage")';
    var jqxhrB = $.ajax({
        url: url,
        cache: true,
        type: 'GET',
        data: { style: _style, personne: _personne, isVisualisation: _isconception }
    })
    .success(function (data) {
        $('#divTableau').html(data);

        if (data != undefined) {
            $('#tbEnCours tr.row-Modal').click(function (event) { EventRowClick($(this)); });
        }
    })
    .fail(function () { alert("Une erreur est survenue lors de la récupération de la liste des conseils !"); });
}