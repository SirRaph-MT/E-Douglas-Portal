function login() {
    var $submitBtn = $("#submit_btn");
    $submitBtn.prop("disabled", true).text("Please wait...");

    var email = $('#email').val().trim();
    var password = $('#password').val();

    if (!email) {
        errorAlert("Email is required.");
        $submitBtn.prop("disabled", false).text("Login");
        return;
    }

    if (!password) {
        errorAlert("Password is required.");
        $submitBtn.prop("disabled", false).text("Login");
        return;
    }

    $.ajax({
        type: 'POST',
        url: '/Account/Login',
        dataType: 'json',
        data: {
            email: email,
            password: password
        },
        success: function (result) {
            $submitBtn.prop("disabled", false).text("Login");

            if (!result.isError) {
                location.href = result.returnUrl;
            } else {
                if (result.data != null) {
                    newSuccessAlert(result.msg, result.url);
                } else {
                    errorAlert(result.msg || "Login failed. Please try again.");
                }
            }
        },
        error: function (ex) {
            $submitBtn.prop("disabled", false).text("Login");
            errorAlert("An error has occurred. Please try again or contact support if the issue persists.");
        }
    });
}

function registerUser() {
    var $submitBtn = $("#submit_btn");
    $submitBtn.prop("disabled", true).text("Please wait...");

    $("#checkboxWarning").hide();

    var surname = $('#surname').val().trim();
    var email = $('#email').val().trim();
    var firstName = $('#firstName').val().trim();
    var password = $('#password').val();
    var confirmPassword = $('#confirmPassword').val();
    var phone = $('#phone').val().trim();
    var lastName = $('#lastName').val().trim();

    if (!email) {
        errorAlert("Email is required.");
        $submitBtn.prop("disabled", false).text("Register");
        return;
    }
    if (!firstName) {
        errorAlert("First Name is required.");
        $submitBtn.prop("disabled", false).text("Register");
        return;
    }
    if (!password) {
        errorAlert("Password is required.");
        $submitBtn.prop("disabled", false).text("Register");
        return;
    }
    if (!confirmPassword) {
        errorAlert("Please confirm your password.");
        $submitBtn.prop("disabled", false).text("Register");
        return;
    }
    if (password !== confirmPassword) {
        errorAlert("Passwords do not match.");
        $submitBtn.prop("disabled", false).text("Register");
        return;
    }

    var data = {
        Email: email,
        PhoneNumber: phone,
        FirstName: firstName,
        LastName: lastName,
        Password: password,
        ConfirmPassword: confirmPassword,
        Surname: surname
    };

    $.ajax({
        type: 'POST',
        url: '/Account/Register',
        dataType: 'json',
        data: {
            userData: JSON.stringify(data)
        },
        success: function (result) {
            if (!result.isError) {
                var url = '/Account/Login';
                newSuccessAlert(result.msg, url);
            } else {
                errorAlert(result.msg);
                $submitBtn.prop("disabled", false).text("Sign Up");
            }
        },
        error: function (ex) {
            errorAlert("An error has occurred, try again. Please contact support if the error persists.");
            $submitBtn.prop("disabled", false).text("Sign Up");
        }
    });
}

//this will refactored to our js pattern later, just want to get it working first
function saveStudentProfile() {
    var $btn = $('#saveProfileBtn');
    $btn.prop('disabled', true).text('Please wait...');

    var form = $('#completeProfileForm')[0];
    var formData = new FormData(form);

    $.ajax({
        url: '/Student/CompleteProfile',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            $btn.prop('disabled', false).text('Save');
            if (!result.isError) {
                newSuccessAlert(result.msg, result.returnUrl || '/Student/Index');
            } else {
                errorAlert(result.msg || 'Unable to save profile');
            }
        },
        error: function () {
            $btn.prop('disabled', false).text('Save');
            errorAlert('An error occurred while saving your profile.');
        }
    });
}

$(document).on('click', '#saveProfileBtn', function () {
    saveStudentProfile();
});