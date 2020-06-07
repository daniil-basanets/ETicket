var filters = [
    { columnName: "totalPrice", inputId: "#total-price-input", isCheckBox: false },
    { columnName: "referenceNumber", inputId: "#reference-number-input", isCheckBox: false },
    { columnName: "date", inputId: "#date-datepicker", isCheckBox: false }
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
var isNewSearch = false;

function makeSingleOnClickEvent(e) {
    if (!e) {
        var e = window.event;
    }
    e.cancelBubble = true;
    if (e.stopPropagation) {
        e.stopPropagation();
    }
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
                url: 'TransactionHistory/GetCurrentPage',
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
                    name: "totalPrice",
                    data: "totalPrice",
                },
                {
                    name: "date",
                    data: "date",
                    render: function (data, type, row) {
                        if (data != null) {
                            var date = new Date(Date.parse(data));
                            return date.toLocaleString();
                        }
                    }
                },
                {
                    name: "referenceNumber",
                    data: "referenceNumber",
                },
                {
                    data: null,
                    //Set default buttons (Edit, Delete)
                    //href = "#" because <a> without href have a special style
                    defaultContent: '<a class="btn btn-info btn-sm" href = "#" id = "detailsButton">Details</a>'
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

    $('#total-price-input, #reference-number-input, #date-datepicker').unbind()
        .bind("change", function (e) {
            isNewSearch = true;
            table.draw();
        });

    //Event listener for Details button 
    $("#dataTable tbody").on('click', '#detailsButton', function () {
        var data = table.row($(this).parents('tr')).data();
        location.href = "/TransactionHistory/Details/" + data.id;
    })

    //Delete container from loyout only for Index
    $('.container').removeClass('container');

});
