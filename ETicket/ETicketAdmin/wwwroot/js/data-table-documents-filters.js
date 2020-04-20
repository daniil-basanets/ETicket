// Call the dataTables jQuery plugin
$.fn.dataTable.ext.search.push(
    function (settings, data, dataIndex) {
        var documentNumberInput = ($('#document-number-input').val());
        var documentTypeInput = $('#document-type-select option:selected').text();

        var docN = String(data[1]); 
        var docT = String(data[0]).split(' ')[0];

        if (((documentNumberInput && docN.includes(documentNumberInput)) || !documentNumberInput) &&
            ((documentTypeInput && docT.includes(documentTypeInput)) || !documentTypeInput))
        {
            return true;
        }

        return false;
    }
);

$(document).ready(function () {
    $.noConflict();
    var table = $('#dataTable').DataTable({
        columnDefs: [
            { orderable: false, targets: -1 }
        ],
        processing: true,
        serverSide: true,
        order: [[1, "desc"]],
        ajax: {
            url: 'Documents/GetCurrentPage',
            datatype: 'json',
            type: 'POST'
        },

        //Columns data order       
        columns: [
            { data: "documentType.name" },
            { data: "number" },
            {
                data: "expirationDate",
                defaultContent: ""
            },
            {
                data: "isValid",
                render: function (data, type, row) {
                    if (data == true) {
                        return "Yes";
                    }
                    else {
                        return "No";
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

    $("#dataTable tbody").on('click', '#editButton', function () {
        var data = table.row($(this).parents('tr')).data();
        location.href = "/Documents/Edit/" + data.id;
    })
    //Event listener for Details button 
    $("#dataTable tbody").on('click', '#detailsButton', function () {
        var data = table.row($(this).parents('tr')).data();
        location.href = "/Documents/Details/" + data.id;
    })
    //Event listener for Delete button 
    $("#dataTable tbody").on('click', '#deleteButton', function () {
        var data = table.row($(this).parents('tr')).data();
        location.href = "/Documents/Delete/" + data.id;
    })

    $('#document-number-input').keyup(function () {
        table.draw();
    });

    $('#document-type-select').change(function () {
        table.draw();
    });

    //Delete container from loyout only for Index
    $('.container').removeClass('container');

});
