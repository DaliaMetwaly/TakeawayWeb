var table;
function Change(ID) {
    var URL = '/Comment/ChangeStatus/' + ID;
    ChangeStatus(URL, function (flag) {
        if (parseInt(ResultType.Success) == flag) {
            ShowMsg('حسنا تم تفعيل التعليق وكل شيء يبدو جيدا.', MessageType.Success, 5);
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
            "url": "/Comment/LoadData",
            "type": "GET",
            "datatype": "json",
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.13/i18n/Arabic.json"
        },
        "columns": [
                { "data": "Comment", "autoWidth": true, "className": "text-center" },
                { "data": "RestaurantName", "autoWidth": true, "className": "text-center" },
               { "data": "UserName", "autoWidth": true, "className": "text-center" },
                { "data": "CreationDate", "autoWidth": true, "className": "text-center" },
                 { "data": "IsActive", "autoWidth": true, "className": "text-center" },
                { "data": "Deleted", "className": "text-center", "sortable": false }

        ]
    });
}