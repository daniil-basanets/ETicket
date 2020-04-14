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
        ]

    });

    $('#total-price-input, #count-input').keyup(function () {
        table.draw();
    });
    $('#type-select').change(function () {
        table.draw();
    });
    //Delete container from loyout only for Index
    $('.container').removeClass('container');

});
