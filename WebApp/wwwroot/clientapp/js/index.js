$(document).ready(function () {
    loadMainCategory();
});

const loadMainCategory = async function () {
    await $.post("/LoadMainCategory").done(res => {
        alert(res);
    }).fail(xhr => {
        an.title = "Oops! Error";
        an.content = xhr.responseText;
        an.alert(an.type.failed);
    }).always(() => "");
}