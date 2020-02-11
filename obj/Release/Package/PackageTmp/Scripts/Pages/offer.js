var table;
function Change(ID) {
    var URL = '/Offer/ChangeStatus/' + ID;
    ChangeStatus(URL, function (flag) {
        if (parseInt(ResultType.Success) == flag) {
            ShowMsg('حسنا تم التفعيل  وكل شيء يبدو جيدا.', MessageType.Success, 5);
            table.destroy();
            LoadData();
        }
        else if (parseInt(ResultType.Error) == flag) {
            ShowMsg('خطأ حاول مرة اخرى', MessageType.Error, 5);
        }
    });
}

function LoadData() {
    debugger;
    table = $('#myTable').DataTable({
        "ajax": {
            "url": "/Offer/LoadData",
            "type": "GET",
            "datatype": "json",
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.13/i18n/Arabic.json"
        },
        "columns": [
                { "data": "UserName", "autoWidth": true, "className": "text-center" },
                { "data": "Name", "autoWidth": true, "className": "text-center" },
               { "data": "FeeType", "autoWidth": true, "className": "text-center" },
                { "data": "FeeValue", "autoWidth": true, "className": "text-center" },
                { "data": "StartDate", "autoWidth": true, "className": "text-center" },
                { "data": "EndDate", "autoWidth": true, "className": "text-center" },
                //{ "data": "CreationDate", "autoWidth": true, "className": "text-center" },
                 { "data": "IsActive", "autoWidth": true, "className": "text-center" },
                { "data": "Edit", "className": "text-center", "sortable": false },
                 { "data": "Deleted", "className": "text-center", "sortable": false }

        ]
    });
}