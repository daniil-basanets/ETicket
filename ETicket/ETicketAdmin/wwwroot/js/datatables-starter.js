// Call the dataTables jQuery plugin

$(document).ready(function () {
    $.noConflict();
    var table = $('#dataTable').DataTable({
        columnDefs: [
            { orderable: false, targets: -1 }
        ]

    });

    //Delete container from loyout only for Index
    $('.container').removeClass('container');


    //Input create button to control line

    //var button = $('#buttonCreate').html();
    //var target = $('#dataTable_wrapper .row').first();

    //$('#buttonCreate').html('');


    //target.children().removeClass('col-md-6');
    //target.children().addClass('col-md-4');
    //$('#dataTable_wrapper .row').first().html(
    //    target.html()
    //    + '<div class="col-sm-12 col-md-4" style="text-align:right">' + button + '</div>');


    


    //var button = $('#buttonCreate').html();
    //var target = $('#dataTable_filter').html();
    //$('#buttonCreate').first().html('');
    //$('#dataTable_filter').html('<lable>' + button + '</lable>' + target);
});
