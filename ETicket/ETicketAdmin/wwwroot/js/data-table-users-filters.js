// Call the dataTables jQuery plugin
$.fn.dataTable.ext.search.push(
    function (settings, data, dataIndex) {
        var firstNameInput = ($('#first-name-input').val());
        var lastNameInput = ($('#last-name-input').val());
        var privilegeInput = $('#privilege-select option:selected').text();
        var date = ($('#user-birth-date').val());
        var documentInput = ($('#document-input').val());
        alert(date);

        
        var firstN = String(data[0]); // use data for the first name col
        var lastN = String(data[1]);// use data for the last name col
        var birth = String(data[2]).split(' ')[0];
        var privilege = String(data[3]);
        var document = String(data[4]);
        //alert("input: " + dateInput + "actual: " + birth);

        if (((firstNameInput && firstN.includes(firstNameInput)) || !firstNameInput) &&

            ((lastNameInput && lastN.includes(lastNameInput)) || !lastNameInput) &&

            //((date && birth === dateInput) || !Date.parse(date)) &&

            ((privilegeInput && privilege.includes(privilegeInput)) || !privilegeInput) &&

            ((documentInput&& document.includes(documentInput)) || !documentInput))
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

    $('#first-name-input, #last-name-input, #privilege-select, #document-input').keyup(function () {
        table.draw();
    });

    $('#privilege-select').change(function () {
        table.draw();
    });

    $('#user-birth-date').select(function() {
        //alert($('#user-birth-date').val());
        table.draw();
    });
    //Delete container from loyout only for Index
    $('.container').removeClass('container');

});
