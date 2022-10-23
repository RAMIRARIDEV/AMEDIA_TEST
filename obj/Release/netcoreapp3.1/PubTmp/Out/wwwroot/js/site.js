$("#btnSignIn").click(function () {
    if (!validateInputs()) {
        $("#lblError").html("Por favor, ingrese los campos obligatorios.");
    }
    else {
        $("#lblError").html("");
        $("#formLogin").submit();
    }
});

function validateInputs() {
    let ret = true;
    $('form').find('input').each(function () {
        if ($(this).prop('required')) {
            if (this.value === "") {
                ret = false;
            }
        }
    });

    return ret;
}

$("#btnConfirmUpdate").click(function () {
    if (!validateInputs()) {
        $("#lblError").html("Por favor, ingrese los campos obligatorios.");
    }
    else {
        $("#lblError").html("");
        $("#formUpdate").submit();
    }
});

$("#btnSignUp").click(function () {
    if (!validateInputs()) {
        $("#lblError").html("Por favor, ingrese los campos obligatorios.");
    }
    else {
        $("#lblError").html("");
        $("#formRegister").submit();
    }
});
$("#btnConfirmAdd").click(function () {
    if (!validateInputs()) {
        $("#lblError").html("Por favor, ingrese los campos obligatorios.");
    }
    else {
        $("#lblError").html("");
        $("#formAdd").submit();
    }
});


function edit(id) {
    window.location.href = "../../User/Update?code=" + id;
    window.history.replaceState(null, null, window.location.pathname);
}

function changeState(id) {
    if (confirm("¿Estás seguro?")) {
        window.location.href = "../../User/ChangeState?code=" + id;
        window.history.replaceState(null, null, window.location.pathname);
    }
    return false;

}


