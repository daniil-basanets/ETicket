// Call the dataTables jQuery plugin
$.fn.dataTable.ext.search.push(
    function (settings, data, dataIndex) {
        var ticketTypeInput = ($('#ticket-type-select option:selected').text());
        var userNameInput = ($('#user-name-input').val());
        var ticketType = String(data[0]);
        var userN = String(data[4]); 

        if (((ticketTypeInput && ticketType.includes(ticketTypeInput)) || !ticketTypeInput) &&

            ((userNameInput && userN.includes(userNameInput)) || !userNameInput)) {
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
            url: 'Ticket/GetCurrentPage',
            datatype: 'json',
            type: 'POST'
        },

        //Columns data order       
        columns: [
            { data: "ticketType.typeName" },
            { data: "createdUTCDate" },
            { data: "activatedUTCDate" },
            { data: "expirationUTCDate" },
            { data: "user.lastName" },
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
    ////Event listener for User reference 
    //$("#dataTable tbody").on('click', '#userDetailsButton', function () {
    //    var data = table.row($(this).parents('tr')).data();
    //    location.href = "/User/Detatils/" + data.user.id;
    //})
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

    $('#user-name-input').keyup(function () {
        table.draw();
    });

    $('#ticket-type-select').change(function () {
        table.draw();
    });
    //Delete container from loyout only for Index
    $('.container').removeClass('container');

});
