// fonction qui affiche le carousel à l'horizontal
function CarouselHorizontalSetting(selector) {
    var divMaitre = '';
    if (selector != undefined) { if (selector != '') { divMaitre = selector + ' '; } }

    jQuery(divMaitre + '#mycarousel').jcarousel({
        vertical: false,
        scroll: 2
    });
    $(divMaitre + 'div.jcarousel-prev.jcarousel-prev-horizontal.jcarousel-prev-disabled.jcarousel-prev-disabled-horizontal').css('top', '115px');
    $(divMaitre + 'div.jcarousel-prev.jcarousel-prev-horizontal').css('top', '115px');
    $(divMaitre + 'div.jcarousel-next.jcarousel-next-horizontal.jcarousel-next-disabled.jcarousel-next-disabled-horizontal').css('top', '115px');
    $(divMaitre + 'div.jcarousel-next.jcarousel-next-horizontal').css('top', '115px');
}

// fonction qui calcule le width à appliquer pour voir les images à l'horizontal
function UpdateWidthForInLinePictures(minWidth, selector) {
    if (minWidth != undefined) {
        var divMaitre = '';
        if (selector != undefined) { if (selector != '') { divMaitre = selector + ' '; } }

        var nbLi = $(divMaitre + '#mycarousel li').length;
        var nbWidth = (nbLi * 184) + 40;
        if (nbWidth > minWidth) { jQuery(divMaitre + '#mycarousel').width(nbWidth); }
        else { jQuery(divMaitre + '#mycarousel').width(minWidth); }
    }
}