var table;
function Change(ID) {
    var URL = '/Notification/ChangeStatus/' + ID;
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
            "url": "/Notification/LoadData",
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
               { "data": "ID", "autoWidth": true, "className": "hidden id" },
                { "data": "Name", "autoWidth": true, "className": "text-center", "orderable": true, "searchable": true },
                { "data": "UserCategory", "autoWidth": true, "className": "text-center" },
                { "data": "CreatedDate", "autoWidth": true, "className": "text-center" },
                { "data": "IsActive", "autoWidth": true, "className": "text-center" },
                { "data": "Deleted", "className": "text-center", "sortable": false },
                //{ "data": "Details", "className": "text-center", "sortable": false }

        ]
    });

}



function format(d) {
    // `d` is the original data object for the row

    var sOut = '';
    sOut += '<table id="tblQueueMessageDetails" class="table table-striped table-bordered table-hover Details">';
    sOut += '<thead>';
    sOut += '<tr class="heading">';
    sOut += '<th class="text-center">العميل</th>';
    sOut += '<th class="text-center">إرسال</th>';
    sOut += '</tr>';

    sOut += '</thead>';
    sOut += '<tbody>';
    for (var i = 0; i < d.length; i++) {
        sOut += '<tr role="row" >';
        sOut += '<td class="text-center">' + d[i].UserName + '</td>';
        sOut += '<td class="text-center">' + d[i].isSend + ' </td>';
        sOut += '</tr>';
    }
    sOut += '</tbody>';
    sOut += '</table>';
    return sOut;
}