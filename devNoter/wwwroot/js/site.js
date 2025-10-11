// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {
    var textarea = document.getElementById("editor");
    if (!textarea) return;

    // Check om textarea er inde i en div med class "view-note"
    var isReadOnly = textarea.closest(".view-note") !== null;

    var editor = CodeMirror.fromTextArea(textarea, {
        lineNumbers: true,
        mode: "text/x-csharp",
        theme: "dracula",
        indentUnit: 4,
        tabSize: 4,
        matchBrackets: true,
        readOnly: isReadOnly ? "nocursor" : false
    });

    if (!isReadOnly) {
        var form = document.querySelector("form");
        if (form) {
            form.addEventListener("submit", function () {
                textarea.value = editor.getValue();
            });
        }
    }
});




document.querySelectorAll(".image-card").forEach(function (card) {
    const btn = card.querySelector(".remove-btn");
    const hiddenInput = card.querySelector("input[name='RemoveImages']");
    const imgPath = card.dataset.img;

    btn.addEventListener("click", function () {
        if (hiddenInput.value === "") {
            // Marker billedet til fjernelse
            card.style.opacity = "0.5";
            hiddenInput.value = imgPath;
            btn.textContent = "↺"; // “tilføj igen”
            btn.classList.remove("btn-danger");
            btn.classList.add("btn-success");
        } else {
            // Fortryd fjernelse
            card.style.opacity = "1";
            hiddenInput.value = "";
            btn.textContent = "×";
            btn.classList.remove("btn-success");
            btn.classList.add("btn-danger");
        }
    });
});