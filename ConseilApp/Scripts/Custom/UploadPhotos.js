jQuery(document).ready(function () {
    PrepareInputFile('#fsVetement', '#fsVetement #PhotoVetement_');
    PrepareInputFile('#fsHabillage', '#fsHabillage #PhotoHabillage_');
    
    ListeActions('#fsVetement', '#fsVetement #PhotoVetement_');
    ListeActions('#fsHabillage', '#fsHabillage #PhotoHabillage_');

    // gère l'évènement Submit des deux formulaires
    $('#frmUploadVetement').submit(function (event) {
        $('#fsVetement #VetementValidation').val(true);
    });
    $('#frmUploadHabillage').submit(function (event) {
        $('#fsVetement #VetementValidation').val(false);
    });
});

function ListeActions(selectorA, selectorB) {
    // pose un évènement sur la liste des styles et des vêtements
    $(selectorA + ' select').change(function () {
        if (selectorA == '#fsVetement') {
            if ($(this).attr("id") == "#PhotoVetement_Style") {
                $(selectorA + ' input[type=checkbox]').prop("checked", true);
            }
            var xstyle = $(selectorB + 'Style option:selected').val();
            var xvetement = $(selectorB + 'Vetement option:selected').val();
            if (xstyle != "") {
                ChargeImages(xstyle, xvetement);
            } else {
                $('#photosVetement').html("");
                if (xvetement != "") { $(selectorB + 'Vetement option:selected').prop("selected", false); }
            }
        }
        else {
            var xstyle = $(selectorB + 'Style option:selected').val();
            if (xstyle != "") {
                ChargeImagesHabillage(xstyle);
            } else {
                $('#photosHabillage').html("");
            }
        }

        $(selectorA + ' span.file-input-name').text('');
    });
    
    // sélectionne le style et le vêtement en cours dès qu'on arrive sur la page
    var vStyle = $(selectorB + 'Style option:selected').val();
    var vVetement = $(selectorB + 'Vetement option:selected').val();
    if (vStyle != "" && vStyle != undefined) {
        $(selectorB + 'Style').filter(function () {
            //may want to use $.trim in here
            return $(this).text() == vStyle;
        }).attr('selected', true);
        if (vVetement == "") { $(selectorB + 'Style'); }
    }
    if (vVetement != "" && vVetement != undefined) {
        $(selectorB + 'Vetement').filter(function () {
            //may want to use $.trim in here
            return $(this).text() == vVetement;
        }).attr('selected', true);//.trigger("changes");
    }

    if (vStyle != "") {
        if (selectorA == '#fsVetement') { ChargeImages(vStyle, vVetement); }
        else { ChargeImagesHabillage(vStyle); }
    }
}

function PrepareInputFile(selectorA, selectorB) {
    $(selectorA + ' input[type=file]').bootstrapFileInput();
    $(selectorA + ' .file-inputs').bootstrapFileInput();

    if (selectorA == '#fsVetement') {
        // transfert l'évènement du clic de l'input file sur le span
        $(selectorA + ' #fileSelectButton').click(function (event) {
            if ($(selectorB + 'Style option:selected').val() != "") {
                if ($(selectorB + 'Vetement option:selected').val() != "") {
                    $(selectorA + ' input[type=file]').click();
                }
            }
        });

        // transfert l'évènement du submit sur le change de l'input life
        $(selectorA + ' input[type=file]').bind('change', function (event) {
            var vVetement = $(selectorB + 'Vetement option:selected').val();

            if (vVetement == "") {
                event.stopPropagation();
                alert('Vous devez choisir un type de vêtement.');
            }
            else {
                $(selectorA + ' #valide').click();
            }
        });
    }
    else {
        $(selectorA + ' #fileSelectButton').click(function () {
            $(selectorA + ' input[type=file]').click();
        });

        $(selectorA + ' input[type=file]').bind('change', function (event) {
            $(selectorA + ' #valide').click();
        });
    }
}