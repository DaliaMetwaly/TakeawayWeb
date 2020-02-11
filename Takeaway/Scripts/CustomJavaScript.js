var MessageType = {
    Success: 'success',
    Error: 'danger',
    Warning: 'warning',
    Info: 'info'
};

var ResultType = {
    Success: '1',
    Error: '2',
    Already: '3'
};

function DeleteRow() {
    ////////////////////to appear pop up when try to delete
    return confirm('هل انت متاكد من الحذف ؟');
}

function DeleteRecord(URL, cb) {
    var Result;
    bootbox.confirm("Are you sure want to delete ?", function (result) {
        if (result == true) {
            $.ajax({
                url: URL, //'/LanguageTemplate/Delete/' + ID,
                type: "GET",
                dataType: "JSON",
                contentType: "application/json",
                success: function (response) {
                    if (response == "1") {
                        cb(ResultType.Success);
                    }
                    else if (response == "2") {
                        cb(ResultType.Already);
                    }

                },
                error: function () {
                    cb(ResultType.Error);
                }
            });
        }

    })


}