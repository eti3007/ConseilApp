function CreateMenu(url, page) {
    $.ajax({
        url: url,
        type: "GET",
        data: { pageEnCours: page }
    }).done(function (vue) { $('#dvMenu').html(vue); });
}