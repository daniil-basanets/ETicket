// Call the dataTables jQuery plugin

$(document).ready(function () {
    $.noConflict();
    var table = $('#dataTable').DataTable({
        columnDefs: [
            //Remove sorting button from last column
            { orderable: false, targets: -1 }
        ]   
    });

    //Delete container from loyout only for Index
    $('.container').removeClass('container');
});
