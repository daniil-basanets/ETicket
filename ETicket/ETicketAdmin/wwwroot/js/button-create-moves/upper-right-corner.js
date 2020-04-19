$(document).ready(function () {
    var buttonId = '#buttonCreate';
    var entriesLength = '#dataTable_length'; //Show entries
    var filter = '#dataTable_filter'; //Search
    var info = '#dataTable_info'; //Showing 1 to 4 of 4 entries
    var paginate = '#dataTable_paginate'; //Paginate
    var header = $('#dataTable_wrapper .row').first();
    var footer = $('#dataTable_wrapper .row').last();

    let headerItemsId = [filter, buttonId];
    let headerItemsClass = ["col-sm-12 col-md-6", "col-sm-12 col-md-6"];

    let footerItemsId = [entriesLength, info, paginate];
    let footerItemsClass = ["col-sm-12 col-md-2", "col-sm-12 col-md-4", "col-sm-12 col-md-6"];

    moveTableRowItems(header, headerItemsId, headerItemsClass);
    moveTableRowItems(footer, footerItemsId, footerItemsClass);
    
    //var buttonId = '#buttonCreate';
    //var targetId = '#dataTable_wrapper .row';

    ////Input create button to control line
    //var button = $(buttonId).html();
    //var target = $(targetId).first();

    ////Clear old div
    //$(buttonId).html('');

    ////Change bootstrap grid
    //target.children().removeClass('col-md-6');
    //target.children().addClass('col-md-4');

    ////Insert button to control line
    //$(targetId).first().html(
    //    target.html()
    //    + '<div class="col-sm-12 col-md-4" style="text-align:right">' + button + '</div>');


});

//function setPlaceForDataTableControls(headerIdItems, headerClasses, footerIdItems, footerClasses) {

//}
function moveTableRowItems(JQRow, arrIdItems, arrClassNames) {
    let i = 0;
    let itemHtml = [];
    //appendTo
    //Take element html
    for (const item of arrIdItems) {
        //itemHtml[i] = $(item).prop('outerHTML');
        //$(item).html("");
        //i++;
        //alert($(item).parent("div").prop('outerHTML'));
        $(item).parent("div").removeClass();
        $(item).parent("div").addClass(arrClassNames[i]);
        $(item).parent("div").appendTo(JQRow);
        i++;
    }

    //i = 0;
    ////Clear row
    //JQRow.html("");

    ////Set element html with new classes
    //for (const item of itemHtml) {
    //    JQRow.html(JQRow.html() +
    //        '<div class ="' + arrClassNames[i] + '">'
    //        + item
    //        + '</div>');
    //    i++;
    //}
}


    //function drowTableRow(JQRow, arrIdItems, arrClassNames) {
    //    let i = 0;
    //    let itemHtml = [];
    //    //appendTo
    //    //Take element html
    //    for (const item of arrIdItems) {
    //        itemHtml[i] = $(item).prop('outerHTML');
    //        $(item).html("");
    //        i++;
    //    }

    //    i = 0;
    //    //Clear row
    //    JQRow.html("");

    //    //Set element html with new classes
    //    for (const item of itemHtml) {
    //        JQRow.html(JQRow.html() +
    //            '<div class ="' + arrClassNames[i] + '">'
    //            + item
    //            + '</div>');
    //        i++;
    //    }
    //}
    

//function drowTableFooter(JQRow, arrIdItems, arrClassNames) {
//    //var heared = $('#dataTable_wrapper .row').last();

//    let i = 0;
//    let itemHtml = [];

//    //Take element html
//    for (const item of arrIdItems) {
//        itemHtml[i] = $(item).prop('outerHTML');
//        $(item).remove();
//        i++;
//    }

//    i = 0;
//    //clear header
//    JQRow.html("");

//    //set element html with new classes
//    for (const item of itemHtml) {
//        JQRow.html(JQRow.html() +
//            '<div class ="' + arrClassNames[i] + '">'
//            + item
//            + '</div>');
//        i++;
//    }
//}
/*
 dataTable_length - Show entries
 dataTables_filter - Search
 dataTable_info - Showing 1 to 4 of 4 entries
 dataTable_paginate - Paginate
 */