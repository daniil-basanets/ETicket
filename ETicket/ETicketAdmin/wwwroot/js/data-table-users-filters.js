// Call the dataTables jQuery plugin
$.fn.dataTable.ext.search.push(
    function (settings, data, dataIndex) {
        var firstNameInput = ($('#first-name-input').val());
        var lastNameInput = ($('#last-name-input').val());
        var privilegeInput = $('#privilege-select option:selected').text();
        var documentInput = ($('#document-input').val());

        
        var firstN = String(data[0]);
        var lastN = String(data[1]);
        var privilege = String(data[3]);
        var document = String(data[4]);

        if (((firstNameInput && firstN.includes(firstNameInput)) || !firstNameInput) &&

            ((lastNameInput && lastN.includes(lastNameInput)) || !lastNameInput) &&

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
        ],
        oLanguage: {
            sLengthMenu: "_MENU_",
        }

    });

    $('#first-name-input, #last-name-input, #privilege-select, #document-input').keyup(function () {
        table.draw();
    });

    $('#privilege-select').change(function () {
        table.draw();
    });

    //Delete container from loyout only for Index
    $('.container').removeClass('container');

});
