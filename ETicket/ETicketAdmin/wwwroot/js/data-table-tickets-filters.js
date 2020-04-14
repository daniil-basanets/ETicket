// Call the dataTables jQuery plugin
$.fn.dataTable.ext.search.push(
    function (settings, data, dataIndex) {
        var ticketTypeInput = ($('#ticket-type-select option:selected').text());
        var userNameInput = ($('#user-name-input').val());
        var transactionInput = ($('#transaction-input').val());
        var ticketType = String(data[0]);
        var userN = String(data[4]); 
        var trans = String(data[5]);
        if (((ticketTypeInput && ticketType.includes(ticketTypeInput)) || !ticketTypeInput) &&

            ((userNameInput && userN.includes(userNameInput)) || !userNameInput) &&

            ((transactionInput && trans.includes(transactionInput)) || !transactionInput)) {
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

    $('#user-name-input, #transaction-input').keyup(function () {
        table.draw();
    });

    $('#ticket-type-select').change(function () {
        table.draw();
    });
    //Delete container from loyout only for Index
    $('.container').removeClass('container');

});
