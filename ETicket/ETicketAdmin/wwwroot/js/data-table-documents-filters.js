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
        ]

    });

    $('#document-number-input').keyup(function () {
        table.draw();
    });

    $('#document-type-select').change(function () {
        table.draw();
    });

    //Delete container from loyout only for Index
    $('.container').removeClass('container');

});
