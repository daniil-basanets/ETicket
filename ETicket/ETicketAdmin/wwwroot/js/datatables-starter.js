// Call the dataTables jQuery plugin

$(document).ready(function () {
    $.noConflict();
    var table = $('#dataTable').DataTable({
        columnDefs: [
            //Remove sorting button from last column
            { orderable: false, targets: -1 }
        ],
        processing: true,
        serverSide: true,
        ajax: {
            url: 'DocumentTypes/GetPage',
            datatype: 'json',
            //contentType: 'application/json',
            type: 'POST',
            //data: function (d) {
            //    return JSON.stringify(d);
            //},
            //success: function (response) {
            //    console.log(response);
            //}

        },

        //Columns data order       
        columns: [
            { data: "name" },
            {
                data: null,
                //Set default buttons (Edit, Delete)
                //href = "#" because <a> without href have a special style
                defaultContent:
                    '<a class="btn btn-warning btn-sm" href = "#" id = "editButton">Edit</a>' + ' '
                    + '<a class="btn btn-danger btn-sm" href = "#" id = "deleteButton">Delete</a>'
            }
        ],
        language: {
            //Set message for pop-up window
            processing: "Take data from server. Please wait..."
        }
    });

    //Event listener for Edit button 
    $("#dataTable tbody").on('click', '#editButton', function () {
        var data = table.row($(this).parents('tr')).data();
        location.href = "/DocumentTypes/Edit/" + data.id;
    })
    //Event listener for Delete button 
    $("#dataTable tbody").on('click', '#deleteButton', function () {
        var data = table.row($(this).parents('tr')).data();
        location.href = "/DocumentTypes/Delete/" + data.id;
    })

    //Delete container from loyout only for Index
    $('.container').removeClass('container');
});
