var filters = [
    { columnName: "firstName", inputId: "#first-name-input", isCheckBox: false },
    { columnName: "lastName", inputId: "#last-name-input", isCheckBox: false },
    { columnName: "privilege", inputId: "#privilege-select option:selected", isCheckBox: true },
    { columnName: "document", inputId: "#document-input", isCheckBox: false },
    { columnName: "dateOfBirth", inputId: "#date-birth-datepicker", isCheckBox: false }
   
];

function getFilterMapColumnValue() {
    var result = new Map();
    var value;

    for (var i = 0; i < filters.length; i++) {
        if (filters[i].isCheckBox) {
            value = $(filters[i].inputId).text();
        }
        else {
            value = $(filters[i].inputId).val();
        }
        if (value) {
            result.set(filters[i].columnName, value);
        }
    }

    return result;
}

function makeSingleOnClickEvent(e) {
    if (!e) {
        var e = window.event;
    }
    e.cancelBubble = true;
    if (e.stopPropagation) {
        e.stopPropagation();
    }
}

var isNewSearch = false;

$(document).ready(function () {
    $.noConflict();
    //Variable for count entries
    var totalRecords = -1;
    var pageNumber = 1;


    var table = $('#dataTable')
        //Read additional fields from server side
        .on('xhr.dt', function (e, settings, json, xhr) {
            totalRecords = json.recordsTotal;
        })
        .on('page.dt', function () {
            pageNumber = table.page() + 1;
        })
        .on('length.dt', function () {
            isNewSearch = true;
        })
        //DataTable settings
        .DataTable({
            columnDefs: [
                { orderable: false, targets: -1 }
            ],
            processing: true,
            serverSide: true,
            order: [[1, "desc"]],
            ajax: {
                url: 'User/GetCurrentPage',
                dataFilter: function (data) {
                    var json = jQuery.parseJSON(data);
                    json.draw = json.drawCounter;
                    json.recordsTotal = json.countRecords;
                    json.recordsFiltered = json.countFiltered;
                    json.data = json.pageData;

                    return JSON.stringify(json); // return JSON string
                },
                //To send an array correctly by query string
                traditional: true,
                type: 'GET',
                data: function (d) {
                    var pagingData = {};
                    pagingData.DrawCounter = d.draw;

                    pagingData.PageSize = d.length;
                    pagingData.TotalEntries = totalRecords;

                    if (isNewSearch) {
                        pageNumber = 1;
                        d.page = 1;
                    }

                    pagingData.PageNumber = pageNumber;

                    pagingData.SortColumnName = d.columns[d.order[0]["column"]]["name"];
                    pagingData.SortColumnDirection = d.order[0]["dir"];

                    pagingData.SearchValue = d.search["value"];

                    var mapFilters = getFilterMapColumnValue();
                    pagingData.FilterColumnNames = Array.from(mapFilters.keys());
                    pagingData.FilterValues = Array.from(mapFilters.values());

                    isNewSearch = false;
                    return pagingData;
                }
            },

            //Columns data order       
            columns: [
                {
                    name: "firstName",
                    data: "firstName"
                },
                {
                    name: "lastName",
                    data: "lastName"
                },
                {
                    name: "dateOfBirth",
                    data: "dateOfBirth",
                    render: function (data, type, row) {
                        if (data != null) {
                            var date = new Date(Date.parse(data));
                            return date.toLocaleDateString();
                        }
                    }
                },
                {
                    name: "privilege",
                    data: "privilegeName",
                    defaultContent: "",
                    render: function (data, type, row) {
                        if (data != null) {
                            return '<a href = "Privileges/Details/' + row.privilegeId + '">' + row.privilegeName + '</a>'
                        }
                    }
                },
                {
                    name: "document",
                    data: "documentNumber",
                    defaultContent: "",
                    render: function (data, type, row) {
                        if (data != null) {
                            return '<a href = "Documents/Details/' + row.documentId + '">' + row.documentNumber + '</a>'
                        }
                    }
                },
                {
                    data: null,
                    //Set default buttons (Edit, Delete)
                    //href = "#" because <a> without href have a special style
                    defaultContent:
                        '<a class="btn btn-warning btn-sm" href = "#" id = "editButton">Edit</a>' + ' '
                        + '<a class="btn btn-info btn-sm" href = "#" id = "detailsButton">Details</a>' + ' '
                        + '<a class="btn btn-danger btn-sm" href = "#" id = "deleteButton">Delete</a>'
                }
            ],
            language: {
                //Set message for pop-up window
                processing: "Take data from server. Please wait..."
            },
            oLanguage: {
                sLengthMenu: "_MENU_",
            }
        });

    //Change event listener for search
    //Search after pressing Enter or defocusing the search input field
    $("#dataTable_filter input").unbind()
        .bind("change", function (e) {
            var searchValue = $(this).val();
            isNewSearch = true;
            table.search(searchValue).draw();
        });

    $("#first-name-input, #last-name-input, #document-input, #date-birth-datepicker").unbind()
        .bind("change", function (e) {
            isNewSearch = true;
            table.draw();
        });

    $('#privilege-select')
        .change(function () {
            isNewSearch = true;
            table.draw();
        });

    //Event listener for Edit button 
    $("#dataTable tbody").on('click', '#editButton', function () {
        var data = table.row($(this).parents('tr')).data();
        location.href = "/User/Edit/" + data.id;
    })
    //Event listener for Details button 
    $("#dataTable tbody").on('click', '#detailsButton', function () {
        var data = table.row($(this).parents('tr')).data();
        location.href = "/User/Details/" + data.id;
    })
    //Event listener for Delete button 
    $("#dataTable tbody").on('click', '#deleteButton', function () {
        var data = table.row($(this).parents('tr')).data();
        location.href = "/User/Delete/" + data.id;
    })

    //Delete container from loyout only for Index
    $('.container').removeClass('container');

});

