// Call the dataTables jQuery plugin
$.fn.dataTable.ext.search.push(
    function (settings, data, dataIndex) {
        var totalPriceInput = ($('#total-price-input').val());
        var countInput = ($('#count-input').val());
        var ticketTypeInput = ($('#type-select option:selected').text());

        var totalPrice = String(data[0]);
        totalPrice = totalPrice.split(',')[0];
        var ticketType = String(data[2]);
        var count = data[3];
      
        if (((totalPriceInput && totalPrice.includes(totalPriceInput)) || !totalPriceInput) &&
            (countInput == count || !countInput) &&
            ((ticketTypeInput && ticketType.includes(ticketTypeInput)) || !ticketTypeInput) )
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
            url: 'TransactionHistory/GetCurrentPage',
            datatype: 'json',
            type: 'POST'
        },

        //Columns data order       
        columns: [
            { data: "totalPrice" },
            {
                data: "date",
                render: function (data, type, row) {
                    if (data != null) {
                        var date = new Date(Date.parse(data));
                        return date.toLocaleString();
                    }
                }
            },
            { data: "ticketType.typeName" },
            { data: "count"},
            {
                data: null,
                //Set default buttons (Edit, Delete)
                //href = "#" because <a> without href have a special style
                defaultContent:
                    '<a class="btn btn-info btn-sm" href = "#" id = "detailsButton">Details</a>'
            }
        ],
        language: {
            //Set message for pop-up window
            processing: "Take data from server. Please wait..."
        }
    });

    //Event listener for Details button 
    $("#dataTable tbody").on('click', '#detailsButton', function () {
        var data = table.row($(this).parents('tr')).data();
        location.href = "/TransactionHistory/Details/" + data.id;
    })

    $('#total-price-input, #count-input').keyup(function () {
        table.draw();
    });
    $('#type-select').change(function () {
        table.draw();
    });
    //Delete container from loyout only for Index
    $('.container').removeClass('container');

});
