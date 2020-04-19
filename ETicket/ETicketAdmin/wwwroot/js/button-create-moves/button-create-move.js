//This script can move default DataTables controls and add external controls
//Item must be inside div 
//Example: 
//<div>
//    <div id="buttonCreate" class="button_Create">
//        <a asp-action="Create" class="btn btn-success btn-sm">Create</a>
//    </div>
//</div>


$(document).ready(function () {
    var buttonId = '#buttonCreate';
    var entriesLength = '#dataTable_length'; //Show entries
    var filter = '#dataTable_filter'; //Search
    var info = '#dataTable_info'; //Showing 1 to 4 of 4 entries
    var paginate = '#dataTable_paginate'; //Paginate
    var header = $('#dataTable_wrapper .row').first();
    var footer = $('#dataTable_wrapper .row').last();

    let headerItemsId = [filter, buttonId];
    let headerExternalClass = ["col-sm-12 col-md-6", "col-sm-12 col-md-6"];

    let footerItemsId = [entriesLength, info, paginate];
    let footerExternalClass = ["col-sm-12 col-md-2", "col-sm-12 col-md-4", "col-sm-12 col-md-6"];

    moveTableRowItems(header, headerItemsId);
    changeClassInExternalDiv(headerItemsId, headerExternalClass);

    moveTableRowItems(footer, footerItemsId);
    changeClassInExternalDiv(footerItemsId, footerExternalClass)
});

function moveTableRowItems(JQRow, arrIdItems) {
    let i = 0;
    //Take element html
    for (const item of arrIdItems) {
        //Input item with external div into JQRow
        $(item).parent("div").appendTo(JQRow);
        i++;
    }
}

function changeClassInExternalDiv(arrIdItems, arrNewClassNames) {
    let i = 0;
    for (const item of arrIdItems) {
        //Clear external div class
        $(item).parent("div").removeClass();
        //Add new classes for external div class
        $(item).parent("div").addClass(arrNewClassNames[i]);
        i++;
    }
}

//var buttonId = '#buttonCreate';
//var entriesLength = '#dataTable_length';
//var filter = '#dataTables_filter';
//var info = '#dataTable_info';
//var paginate = '#dataTable_paginate';

//function drowTableHeader(arrIdItems, arrClassNames) {
//    var heared = $('#dataTable_wrapper .row').first();

//    let i = 0;
//    let itemHtml = [];
//    for (const item of arrIdItems) {
//        $(item).removeClass();
//        $(item).addClass(arrClassNames[i]);

//        itemHtml = $(item).get(0).outerHTML;
//        heared.html(itemHtml);
//    }
//}