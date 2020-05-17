//This script can move default DataTables controls and add external controls
//Item must be inside div 
//Example: 
//<div>
//    <div id="buttonCreate" class="button_create">
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
    let headerExternalClass = ["col-sm-6 col-md-6", "col-sm-6 col-md-6"];

    let footerItemsId = [entriesLength, info, paginate];
    let footerExternalClass = ["col-md-2 col-lg-1", "col-md-10 col-lg-4", "col-md-12 col-lg-7"];

    moveTableRowItems(header, headerItemsId);
    changeClassInExternalDiv(headerItemsId, headerExternalClass);

    moveTableRowItems(footer, footerItemsId);
    changeClassInExternalDiv(footerItemsId, footerExternalClass)
});

function moveTableRowItems(JQRow, arrIdItems) {
    //Take element html
    for (var i = 0; i < arrIdItems.length; i++) {
        //Input item with external div into JQRow
        $(arrIdItems[i]).parent("div").appendTo(JQRow);
    }
}

function changeClassInExternalDiv(arrIdItems, arrNewClassNames) {
    for (var i = 0; i < arrIdItems.length; i++) {
        //Clear external div class
        $(arrIdItems[i]).parent("div").removeClass();
        //Add new classes for external div class
        $(arrIdItems[i]).parent("div").addClass(arrNewClassNames[i]);
    }
}

function changeClassInItem(arrIdItems, arrNewClassNames) {
    for (var i = 0; i < arrIdItems.length; i++) {
        //Clear item class
        $(arrIdItems[i]).removeClass();
        //Add new classes for item class
        $(arrIdItems[i]).addClass(arrNewClassNames[i]);
    }
}

//var buttonId = '#buttonCreate';
//var entriesLength = '#dataTable_length';
//var filter = '#dataTables_filter';
//var info = '#dataTable_info';
//var paginate = '#dataTable_paginate';