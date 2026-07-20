function createCourse() {
    var data = {
        Title: $('#title').val().trim(),
        Duration: $('#duration').val().trim(),
        Price: $('#price').val(),
        Description: $('#description').val().trim()
    };
    if (!data.Title || !data.Duration || !data.Price) {
        errorAlert("Please fill in all required fields.");
        return;
    }
    $.ajax({
        type: 'POST',
        url: '/Course/Create',
        dataType: 'json',
        data: data,
        success: function (result) {
            if (!result.isError) {
                successAlertWithRedirect(result.msg || "Course created successfully!", result.returnUrl || "/Course/Index");
            } else {
                errorAlert(result.msg);
            }
        },
        error: function () {
            errorAlert("An error occurred while creating the course.");
        }
    });
}