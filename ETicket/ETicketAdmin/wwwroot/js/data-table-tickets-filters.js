
const columnCount = 5;
var filters = [
    { columnNumber: 0, inputId: "#ticket-type-select option:selected", isCheckBox: true},
    { columnNumber: 4, inputId: "#user-name-input", isCheckBox: false }
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
            result.set(filters[i].columnNumber, value);
        }
    }

    return result;
}

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
        //DataTable settings
        .DataTable({
            //cache: false,
            columnDefs: [
                { orderable: false, targets: -1 }
            ],
            processing: true,
            serverSide: true,
            order: [[1, "desc"]],
            ajax: {
                url: 'Ticket/GetCurrentPage',
                //To send an array correctly by query string
                traditional: true,
                type: 'GET',
                data: function (d) { 
                    var pagingData = {};
                    pagingData.DrawCounter = d.draw;

                    pagingData.PageSize = d.length;
                    pagingData.TotalEntries = totalRecords;
                    pagingData.PageNumber = pageNumber;

                    pagingData.SortColumnNumber = d.order[0]["column"];
                    pagingData.SortColumnDirection = d.order[0]["dir"];
                    
                    pagingData.SearchValue = d.search["value"];

                    var mapFilters = getFilterMapColumnValue();
                    pagingData.FilterColumnNumbers = Array.from(mapFilters.keys());
                    pagingData.FilterValues = Array.from(mapFilters.values());

                    return pagingData;
                }
            },

            //Columns data order       
            columns: [
                {
                    data: "ticketType",
                    render: function (data, type, row) {
                        if (data != null) {
                            return '<a href = "TicketType/Details/' + data.id + '">' + data.typeName + '</a>'
                        }
                    }
                },
                {
                    data: "createdUTCDate",
                    render: function (data, type, row) {
                        if (data != null) {
                            var date = new Date(Date.parse(data));
                            return date.toLocaleString();
                        }
                    }
                },
                {
                    data: "activatedUTCDate",
                    defaultContent: "",
                    render: function (data, type, row) {
                        if (data != null) {
                            var date = new Date(Date.parse(data));
                            return date.toLocaleString();
                        }
                    }
                },
                {
                    data: "expirationUTCDate",
                    defaultContent: "",
                    render: function (data, type, row) {
                        if (data != null) {
                            var date = new Date(Date.parse(data));
                            return date.toLocaleString();
                        }
                    }
                },
                {
                    data: "user",
                    defaultContent: "",
                    render: function (data, type, row) {
                        if (data != null) {
                            return '<a href = "User/Details/' + data.id + '">' + data.firstName + ' ' + data.lastName + '</a>'
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
            }
        });

    //Change event listener for search
    //Search after pressing Enter or defocusing the search input field
    $("#dataTable_filter input").unbind()
        .bind("change", function (e) {
            var searchValue = $(this).val();
            table.search(searchValue).draw();
        });

    //Event listener for Edit button 
    $("#dataTable tbody").on('click', '#editButton', function () {
        var data = table.row($(this).parents('tr')).data();
        location.href = "/Ticket/Edit/" + data.id;
    })
    //Event listener for Details button 
    $("#dataTable tbody").on('click', '#detailsButton', function () {
        var data = table.row($(this).parents('tr')).data();
        location.href = "/Ticket/Details/" + data.id;
    })
    //Event listener for Delete button 
    $("#dataTable tbody").on('click', '#deleteButton', function () {
        var data = table.row($(this).parents('tr')).data();
        location.href = "/Ticket/Delete/" + data.id;
    })

    $("#user-name-input").unbind()
        .bind("change", function (e) {
            table.draw();
        });

    $('#ticket-type-select').change(function () {
        table.draw();
    });

    //Delete container from loyout only for Index
    $('.container').removeClass('container');

});
