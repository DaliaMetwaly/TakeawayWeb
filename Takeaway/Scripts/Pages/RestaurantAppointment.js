var table;
function Change(ID) {
    var URL = '/RestaurantAppointment/ChangeStatus/' + ID;
    ChangeStatus(URL, function (flag) {
        if (parseInt(ResultType.Success) == flag) {
            ShowMsg('حسنا تم التفعيل  وكل شيء يبدو جيدا', MessageType.Success, 5);
            table.destroy();
            LoadData();
        }
        else if (parseInt(ResultType.Error) == flag) {
            ShowMsg('خطأ حاول مرة اخرى', MessageType.Error, 5);
        }
    });
}

function LoadData() {
    table = $('#myTable').DataTable({
        "ajax": {
            "url": "/RestaurantAppointment/LoadData",
            "type": "GET",
            "datatype": "json",
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.13/i18n/Arabic.json"
        },
        "columns": [
             {
                 "className": 'details-control',
                 "orderable": false,
                 "data": null,
                 "defaultContent": '+',

             },
                { "data": "RestaurantID", "autoWidth": true, "className": "text-center", "className": "hidden id" },
                { "data": "RestaurantName", "autoWidth": true, "className": "text-center" },
                { "data": "IsAppoinment", "autoWidth": true, "className": "text-center" },
                { "data": "IsActive", "autoWidth": true, "className": "text-center" },
                { "data": "Edit", "className": "text-center", "sortable": false },
               
        ]
    });
}


function format(d) {
    // `d` is the original data object for the row

    var sOut = '';
    sOut += '<table id="tblQueueMessageDetails" class="table table-striped table-bordered table-hover Details">';
    sOut += '<thead>';
    sOut += '<tr class="heading">';
    sOut += '<th class="text-center">اليوم</th>';
    sOut += '<th class="text-center">من</th>';
    sOut += '<th class="text-center">الى</th>';
    sOut += '</tr>';

    sOut += '</thead>';
    sOut += '<tbody>';
    for (var i = 0; i < d.length; i++) {
        sOut += '<tr role="row" >';
        sOut += '<td class="text-center">' + d[i].Day + '</td>';
        sOut += '<td class="text-center">' + d[i].OpeningTime + ' </td>';
        sOut += '<td class="text-center">' + d[i].CloseTime + ' </td>';

        sOut += '</tr>';
    }
    sOut += '</tbody>';
    sOut += '</table>';
    return sOut;
}