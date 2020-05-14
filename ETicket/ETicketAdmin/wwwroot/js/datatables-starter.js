// Call the dataTables jQuery plugin

$(document).ready(function () {
    $.noConflict();
    var table = $('#dataTable').DataTable({
        columnDefs: [
            { orderable: false, targets: -1 }
        ],
		oLanguage: {
            sLengthMenu: "_MENU_",
        }
    });

    //Delete container from loyout only for Index
    $('.container').removeClass('container');
});
