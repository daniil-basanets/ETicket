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

function changeClassInItem(arrIdItems, arrNewClassNames) {
    let i = 0;
    for (const item of arrIdItems) {
        //Clear item class
        $(item).removeClass();
        //Add new classes for item class
        $(item).addClass(arrNewClassNames[i]);
        i++;
    }
}

//var buttonId = '#buttonCreate';
//var entriesLength = '#dataTable_length';
//var filter = '#dataTables_filter';
//var info = '#dataTable_info';
//var paginate = '#dataTable_paginate';