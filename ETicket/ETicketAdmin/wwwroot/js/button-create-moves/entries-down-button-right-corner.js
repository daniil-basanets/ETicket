$(document).ready(function () {
    var buttonId = '#buttonCreate';
    var entriesLength = '#dataTable_length';
    var filter = '#dataTables_filter';
    var info = '#dataTable_info';
    var paginate = '#dataTable_paginate';

    var item = '#dataTable_length';
    var targetId = '#dataTable_wrapper';

    //Input create button to control line
    var button = $(item).get(0).outerHTML;
    var target = $(targetId).children(".row").eq(-1);
    //alert(button);

    //Clear old div
    $(item).html('');

    //Change bootstrap grid
    //target.children().removeClass();
    //target.children().addClass('col-md-12 col-lg-4');


    var targetHtml = target.html();

    //Insert button to control line
    target.html(
        '<div class="col-md-12 col-lg-3" style="text-align:left">' + button + '</div>'
        + targetHtml);

    $(entriesLength).parent().removeClass();
    $(entriesLength).parent().addClass('col-md-12 col-lg-3');
    $(info).parent().removeClass();
    $(info).parent().addClass('col-md-12 col-lg-5');
    $(paginate).parent().removeClass();
    $(paginate).parent().addClass('col-md-12 col-lg-4');

    var buttonId = '#buttonCreate';
    var targetId = '#dataTable_wrapper .row';

    //Input create button to control line
    var button = $(buttonId).html();
    var target = $(targetId).first();

    //Clear old div
    $(buttonId).remove();

    ////Change bootstrap grid
    //target.children().removeClass('col-md-6');
    //target.children().addClass('col-md-4');
    $(targetId).children().first().remove();
    //Insert button to control line
    $(targetId).first().html(
        '<div class="col-sm-12 col-md-6" style="text-align:left">' + button + '</div>'
        + target.html());
});

/*
 dataTable_length - Show entries
 dataTables_filter - Search
 dataTable_info - Showing 1 to 4 of 4 entries
 dataTable_paginate - Paginate
*/