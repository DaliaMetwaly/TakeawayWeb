var table;
function Change(ID) {
    var URL = '/Package/ChangeStatus/' + ID;
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
            "url": "/Package/LoadData",
            "type": "GET",
            "datatype": "json",
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.13/i18n/Arabic.json"
        },
        "columns": [
                { "data": "PackageNameAr", "autoWidth": true, "className": "text-center" },
                { "data": "PackageNameEn", "autoWidth": true, "className": "text-center" },
               { "data": "PackagePrice", "autoWidth": true, "className": "text-center" },
                { "data": "PackagePeriod", "autoWidth": true, "className": "text-center" },
                { "data": "PackageStartDate", "autoWidth": true, "className": "text-center" },
                { "data": "PackageEndDate", "autoWidth": true, "className": "text-center" },
                 { "data": "IsActive", "autoWidth": true, "className": "text-center" },
                { data: "Edit", "className": "text-center", "sortable": false },
                 { "data": "Deleted", "className": "text-center", "sortable": false }

        ]
    });
}